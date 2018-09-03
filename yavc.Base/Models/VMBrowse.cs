using System;
using System.Linq;
using yavc.Base.Data;
using yavc.Base;
using System.Collections.Generic;
using yavc.Base.Commands;
using yavc.Base.Util;

namespace yavc.Base.Models
{
    public class VMBrowse : AVM
    {
        #region Private Members
        private const int JUMP_SIZE = GetList.PAGE_SIZE * 5;
        private const int MAX_TRIES = 2;
        private const int PAGE_SIZE = GetList.PAGE_SIZE;
        private int MaxLine;
        private Dictionary<int, ListItem> _Items = new Dictionary<int, ListItem>();
        private Stack<int> PreviousLines = new Stack<int>();
        private VMMain MainVM;
        #endregion

        public event EventHandler OnSelectedItem = delegate { };

        public bool CanGoBack { get { return !IsRefreshing && MenuLayer > 1; } }
        public bool CanPageUpJump { get { return !IsRefreshing && (CurrentLine - JUMP_SIZE > 0); } }
        public bool CanPageUp { get { return !IsRefreshing && (CurrentLine - PAGE_SIZE > 0); } }
        public bool CanPageDown { get { return !IsRefreshing && (CurrentLine + PAGE_SIZE <= MaxLine); } }
        public bool CanPageDownJump { get { return !IsRefreshing && (CurrentLine + JUMP_SIZE < MaxLine); } }
        public bool CanList
        {
            get { return GetValue<bool>(() => CanList); }
            private set { SetValue(value, () => CanList); }
        }

        public bool IsRefreshing
        {
            get { return GetValue<bool>(() => IsRefreshing); }
            private set
            {
                SetValue(value, () => IsRefreshing);
                NotifyAll();//-- This value affects all of the CanPage..., so we need to notify listeners because those properties may have changed.
            }
        }

        public int CurrentPage
        {
            get { return GetValue<int>(() => CurrentPage); }
            set { SetValue(value, () => CurrentPage); }
        }
        public int CurrentLine
        {
            get { return GetValue<int>(() => CurrentLine); }
            private set { SetValue(value, () => CurrentLine); }
        }
        public int ItemsCount
        {
            get { return GetValue<int>(() => ItemsCount); }
            private set { SetValue(value, () => ItemsCount); }
        }
        public int MenuLayer
        {
            get { return GetValue<int>(() => MenuLayer); }
            private set { SetValue(value, () => MenuLayer); }
        }
        public int TotalPages { get { return GetList.GetPageForLine(MaxLine); } }
        public ListItem[] Items { get { return _Items.Values.Where(i => i.ItemType != ListItemType.Unselectable).ToArray(); } }

        public string DeviceName { get { return MainVM.FriendlyName; } }
        public string MenuName
        {
            get { return GetValue<string>(() => MenuName); }
            private set { SetValue(value, () => MenuName); }
        }

        public Input Input
        {
            get { return GetValue<Input>(() => Input); }
            private set { SetValue(value, () => Input); }
        }
        public Zone Zone
        {
            get { return GetValue<Zone>(() => Zone); }
            private set { SetValue(value, () => Zone); }
        }

        public VMBrowse(VMMain main, Input i, Zone z)
            : base(main.TheController)
        {
            Input = i;
            Zone = z;
            MainVM = main;

            var source = main.TheController.Sources.FirstOrDefault(s => s.SourceName == i.Src_Name);
            if (source != null)
                CanList = source.CanList;
        }

        #region Public Methods
        public void Back()
        {
            var newLayer = MenuLayer - 1;
            IsRefreshing = true;
            TheController.ListMenuUp(Input, result =>
            {
                if (result.Success)
                {
                    EnsureMenuLayer(newLayer, 0, GetPreviousLine());
                }
                else
                {
                    IsRefreshing = false;
                    UI.Invoke(() =>
                    {
                        Factory.MessageBox.Show("There was an error going back.");
                    });
                }
            });
        }

        public void JumpDown()
        {
            var line = CurrentLine + JUMP_SIZE;
            if (line > MaxLine)
                line = MaxLine - PAGE_SIZE;

            Jump(line);
        }

        public void JumpUp()
        {
            var line = CurrentLine - JUMP_SIZE;
            if (line < 1)
                line = 1;

            Jump(line);
        }

        public void PageUp()
        {
            var line = CurrentLine - PAGE_SIZE;
            if (line < 1)
                line = 1;

            Jump(line);
        }

        public void PageDown()
        {
            var line = CurrentLine + PAGE_SIZE;
            if (line > MaxLine)
                line = MaxLine - PAGE_SIZE; //-- Show last 8 valid entries

            Jump(line);
        }

        public void Refresh()
        {
            IsRefreshing = true;

            var cmd = new GetList(Input);
            cmd.Send(TheController, result =>
            {
                if (result.Success)
                {
                    Refresh(cmd);
                    return;
                }
            });
        }

        public void SelectItem(ListItem i)
        {
            var newLayer = MenuLayer + 1;
            var previousLine = CurrentLine;
            IsRefreshing = true;
            Jump(i.LineNumber, 0, () =>
            {
                TheController.SelectCurrentListItem(Input, result =>
                {
                    if (result.Success && i.ItemType == ListItemType.Container)
                    {
                        PreviousLines.Push(previousLine);
                        EnsureMenuLayer(newLayer, 0, 1); //-- Calls Get info to refresh the list afterwards
                    }
                    else if (result.Success)
                        UI.Invoke(() => OnSelectedItem(this, EventArgs.Empty));
                });
            });
        }
        #endregion

        #region Helper Methods
        private void EnsureMenuLayer(int newLayer, int tries, int line)
        {
            if (newLayer == MenuLayer) return;
            if (tries > MAX_TRIES) return;

            var cmd = new GetList(Input);
            cmd.Send(TheController, result =>
            {
                if (newLayer == cmd.MenuLayer)
                {
                    //-- When we're going to a new layer, we should ensure 
                    // we've jumped back to a the position we came from.
                    Jump(line);
                    return;
                }

                Sleep(tries);

                EnsureMenuLayer(newLayer, ++tries, line);
            });
        }

        private int GetPreviousLine()
        {
            if (PreviousLines.Count == 0)
                return 1;

            return PreviousLines.Pop();
        }

        private void Jump(int line)
        {
            Jump(line, 0, () => Refresh());
        }

        private void Jump(int line, int tries, Action onFinished)
        {
            if (line == CurrentLine)
            {
                onFinished.NullableInvoke();
                return;
            }
            if (tries > MAX_TRIES) return;

            var cmd = new JumpList(line, Input);
            cmd.Send(TheController, result =>
            {
                if (result.Success)
                {
                    if (line == cmd.CurrentLine)
                    {
                        CurrentLine = line;
                        onFinished.NullableInvoke();
                        return;
                    }

                    Sleep(tries);

                    Jump(line, ++tries, onFinished);
                }
            });
        }

        private void Refresh(GetList cmd)
        {
            UpdateFromList(cmd);
            IsRefreshing = false;
        }

        private void Sleep(int tries)
        {
#if WINDOWS_PHONE
            //-- Exponential backoff based on # of tries
            var interval = Math.Pow(2, Math.Min(tries, MAX_TRIES)) * 100;
            System.Threading.Thread.Sleep((int)interval);
#endif
        }

        private void UpdateFromList(GetList cmd)
        {
            MenuName = cmd.MenuName;
            MenuLayer = cmd.MenuLayer;
            MaxLine = cmd.MaxLine;
            CurrentLine = cmd.CurrentLine;
            CurrentPage = cmd.CurrentPage;
            _Items = new Dictionary<int, ListItem>();
            foreach (var i in cmd.Items.Keys)
            {
                _Items[i] = cmd.Items[i];
            }
        }
        #endregion
    }
}