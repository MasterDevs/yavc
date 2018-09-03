using System;
using System.Linq;
using yavc.Base.Data;
using yavc.Base;
using System.Collections.Generic;
using yavc.Base.Commands;
using System.Threading;
using yavc.Base.Util;

namespace yavc.Base.Models {
	public class VMList : AVM {

		private const int MAX_TRIES = 2;
		private VMMain MainVM;

		public event EventHandler OnSelectedItem = delegate { };


		public Input Input { get; private set; }
		public Zone Zone { get; private set; }

		public string MenuName { get; private set; }
		public int MenuLayer { get; private set; }
		public int ItemsCount { get; private set; }
		public ListItem[] Items { get { return _Items.Values.Where(i => i.ItemType != ListItemType.Unselectable).ToArray(); } }
		public IEnumerable<ListGroup> GroupItems {
			get {
				var x = from item in Items
								group item by Group(item) into i
								orderby i.Key
								select new ListGroup(i.Key, i);
				return x;
			}
		}
		
		public bool CanGoBack { get { return MenuLayer > 1; } }
		public int CurrentPosition { get; private set; }
		public double PercentageComplete { get; private set; }
		public bool IsIndeterminate { get { return PercentageComplete < 0; } }

		public bool CanList { get; private set; }
		public bool IsRefreshing { get; private set; }

		private Dictionary<int, ListItem> _Items;
		public string DeviceName { get { return MainVM.FriendlyName; } }

		public VMList(VMMain main, Input i, Zone z)
			: base(main.TheController) {
			Input = i;
			Zone = z;
			_Items = new Dictionary<int, ListItem>();
			MainVM = main;

			var source = main.TheController.Sources.FirstOrDefault(s => s.SourceName == i.Src_Name);
			if (source != null)
				CanList = source.CanList;
		}

		#region Public Methods
		public void Back() {
			var newLayer = MenuLayer - 1;
			TheController.ListMenuUp(Input, result =>
			{
				if (result.Success) {
					ThreadPool.QueueUserWorkItem(state => EnsureMenuLayer(newLayer, 0));
				} else {
					UI.Invoke(() =>
					{
						Factory.MessageBox.Show("There was an error going back.");
					});
				}
			});
		}

		public void Refresh() {
			ThreadPool.QueueUserWorkItem(state =>
			{
				IsRefreshing = true;
				PercentageComplete = 0D;

				UI.Invoke(NotifyAll);
				
				var cmd = new GetList(Input);
				cmd.Send(TheController, result =>
				{
					if (result.Success) {
						int calls = (int)Math.Ceiling((double)cmd.MaxLine / 8D);
						_Items = new Dictionary<int, ListItem>(); 
		
						UI.Invoke(() =>
						{
                            Notify(() => GroupItems);
						});
						
						if (calls == 0) {
							UpdateFromList(cmd);
							IsRefreshing = false;
							PercentageComplete = 1;
							UI.Invoke(NotifyAll);
							return;
						}
						GetList(0, calls);
					}
				});
			});
		}

		public void SelectItem(ListItem i) {
			var newLayer = MenuLayer + 1;
			ThreadPool.QueueUserWorkItem(state =>
			{
				EnsureJump(i.LineNumber, 0, () =>
				{
					TheController.SelectCurrentListItem(Input, result =>
					{
						if (result.Success && i.ItemType == ListItemType.Container) {
							EnsureMenuLayer(newLayer, 0); //-- Calls Get info to refresh the list afterwards
						} else if (result.Success)
							OnSelectedItem(this, EventArgs.Empty);
					});
				});
			});
		}
		#endregion

		#region Helper Methods
		private void EnsureMenuLayer(int newLayer, int tries) {
			if (newLayer == MenuLayer) return;
			if (tries > MAX_TRIES) return;

			var cmd = new GetList(Input);
			cmd.Send(TheController, result =>
			{
				if (newLayer == cmd.MenuLayer) {
					Refresh();
					return;
				}

				Sleep(tries);

				EnsureMenuLayer(newLayer, ++tries);
			});
		}

		private void EnsureJump(int jumpPosition, int tries, Action onSuccessJump) {
			if (jumpPosition == CurrentPosition) {
				onSuccessJump.NullableInvoke();
				return;
			}
			if (tries > MAX_TRIES) return;

			var cmd = new JumpList(jumpPosition, Input);
			cmd.Send(TheController, result =>
			{
				if (result.Success) {
					if (jumpPosition == cmd.CurrentLine) {
						CurrentPosition = jumpPosition;
						onSuccessJump.NullableInvoke();
						return;
					}

					Sleep(tries);

					EnsureJump(jumpPosition, ++tries, onSuccessJump);
				}
			});
		}

		private void GetList(int currentCall, int totalCalls) {
			if (currentCall == totalCalls) {
				IsRefreshing = false;
				PercentageComplete = 0D;
				UI.Invoke(() =>
				{
					NotifyAll();
				});
				return;
			}

			int jump = (currentCall * 8) + 1;
			EnsureJump(jump, 0, () =>
			{
				//-- If we have success, then we list
				var cmd = new GetList(Input);
				cmd.Send(TheController, result =>
				{
					if (result.Success) {
						PercentageComplete = (double)currentCall / (double)totalCalls;
						UI.Invoke(() =>
						{
							NotifyAll();
							UpdateFromList(cmd);
						});
						GetList(++currentCall, totalCalls);
					}
				});

			});
		}

		private static string Group(ListItem item) {
			if (null == item || item.Text.IsNullOrEmpty()) return string.Empty;
			if (char.IsDigit(item.Text[0])) return "#";

			return item.Text.Substring(0, 1).ToUpper();
		}

		private void Sleep(int tries) {
#if WINDOWS_PHONE
			Thread.Sleep((int)Math.Pow(2, Math.Min(tries, MAX_TRIES)) * 100); 
#endif
		}

		private void UpdateFromList(GetList cmd) {
			MenuName = cmd.MenuName;
			MenuLayer = cmd.MenuLayer;
			foreach (var i in cmd.Items.Keys) {
				_Items[i] = cmd.Items[i];
			}
		}
		#endregion
	}

	public class ListGroup : IGrouping<string, ListItem> {
		private IGrouping<string, ListItem> Group;

		public ListGroup(string key, IGrouping<string, ListItem> group) {
			Key = key;
			Group = group;
		}

		private List<ListItem> Items { get; set; }

		#region IGrouping<string,List<ListItem>> Members

		public string Key { get; private set; }

		#endregion

		#region IEnumerable<List<ListItem>> Members

		public IEnumerator<ListItem> GetEnumerator() {
			return Group.GetEnumerator();
		}

		#endregion

		#region IEnumerable Members

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}

		#endregion

		public override bool Equals(object obj) {
			return Equals(obj as ListGroup);
		}
		public bool Equals(ListGroup group) {
			if (null == group) return false;
			return this.Key == group.Key;
		}
		public override int GetHashCode() {
			if (null == Key) return string.Empty.GetHashCode();
			return Key.GetHashCode();
		}
	}
}