using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace yavc.Phone.Controls {
	public partial class AddDevice : UserControl {

		public string IP {
			get {
				return string.Format("{0}.{1}.{2}.{3}",
					tb1.Text,
					tb2.Text,
					tb3.Text,
					tb4.Text);
			}
		}
		public string FriendlyName { get { return tbFriendly.Text; } }

		public AddDevice() {
			InitializeComponent();
		}

		public void Start() {
			tb1.Focus();
		}

		private void TextBox_KeyDown(object sender, KeyEventArgs e) {
			var tb = sender as TextBox;

			//-- Check for unknown keys
			//For some reason, when focus hits a TB, a key press is simulated. Not sure why
			if (e.Key == Key.Unknown) {
				e.Handled = true;
				//-- If the user pressed '.', then move to the next TB
				if (e.PlatformKeyCode == 190) MoveToNextTextBox(tb);
				return;
			} 
			
			//-- Grab the text. If we don't have any text, add the digit.
			if (string.IsNullOrEmpty(tb.Text)) return;

			//-- See what the new number would be if we would allow the digit pressed
			var newNum = GetNewNum(tb.Text, e.Key);
			if (newNum > 255 || newNum < 1) {
				e.Handled = true;
				return;
			}

			//-- If the digit is valid, see if we can help the user
			// by moving the focus to the next box
			if (tb.Text.Length == 2 || newNum > 25) {
				tb.Text = newNum.ToString();
				MoveToNextTextBox(tb);
				e.Handled = true;
			}
		}

		private void MoveToNextTextBox(TextBox tb) {
			if (tb == tb1)
				tb2.Focus();
			else if (tb == tb2)
				tb3.Focus();
			else if (tb == tb3)
				tb4.Focus();
			else if (tb == tb4)
				tbFriendly.Focus();
		}

		private int GetNewNum(string p, Key key) {
			switch (key) {
				case Key.D0:
					return int.Parse(p + "0");
				case Key.D1:
					return int.Parse(p + "1");
				case Key.D2:
					return int.Parse(p + "2");
				case Key.D3:
					return int.Parse(p + "3");
				case Key.D4:
					return int.Parse(p + "4");
				case Key.D5:
					return int.Parse(p + "5");
				case Key.D6:
					return int.Parse(p + "6");
				case Key.D7:
					return int.Parse(p + "7");
				case Key.D8:
					return int.Parse(p + "8");
				case Key.D9:
					return int.Parse(p + "9");
				default:
					return -1;
			}
		}

		public bool IPEntered {
			get {
				return
					!string.IsNullOrEmpty(tb1.Text) &&
					!string.IsNullOrEmpty(tb2.Text) &&
					!string.IsNullOrEmpty(tb3.Text) &&
					!string.IsNullOrEmpty(tb4.Text) &&
					!string.IsNullOrEmpty(tbFriendly.Text);
			}
		}
	}
}
