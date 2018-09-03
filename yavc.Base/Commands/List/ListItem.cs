using yavc.Base.Util;

namespace yavc.Base.Commands {

	public class ListItem : ANotifiable {
		public ListItemType ItemType { get; set; }
		public string Text { get; set; }
		public int LineNumber { get; set; }
		public bool IsSelected { get; set; }

		public ListItem() { }
		public ListItem(string text, string itemType, int lineNumber) {
			Text = text;
			LineNumber = lineNumber;
			switch (itemType.ToUpper()) {
				case "CONTAINER":
					ItemType = ListItemType.Container;
					break;
				case "ITEM":
					ItemType = ListItemType.Item;
					break;
				case "UNSELECTABLE":
				default:
					ItemType = ListItemType.Unselectable;
					break;
			}
		}

		public override string ToString() {
			return string.Format("{0}: {1} - {2}", LineNumber, ItemType, Text);
		}
	}

	public enum ListItemType {
		Container,
		Item,
		Unselectable
	}
}
