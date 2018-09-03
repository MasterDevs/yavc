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
using yavc.Base.Commands;
using yavc.Base.Models;

namespace yavc.Phone.Controls {
	public partial class BrowsePageControl : UserControl {
		private VMBrowse Browse { get { return DataContext as VMBrowse; } }

		public BrowsePageControl() {
			InitializeComponent();
		}

		private void btnItem_Click(object sender, RoutedEventArgs e) {
			var fe = sender as FrameworkElement;
			var item = fe.DataContext as ListItem;

			Browse.SelectItem(item);
		}

        private void btnPageDown_Click(object sender, RoutedEventArgs e)
        {
            Browse.PageDown();
        }

        private void btnPageDownJump_Click(object sender, RoutedEventArgs e)
        {
            Browse.JumpDown();
        }
        
        private void btnPageUp_Click(object sender, RoutedEventArgs e) {
			Browse.PageUp();
		}

        private void btnPageUpJump_Click(object sender, RoutedEventArgs e)
        {
            Browse.JumpUp();
        }
	}
}
