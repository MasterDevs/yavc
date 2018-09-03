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
using Microsoft.Phone.Controls;

namespace yavc.Phone.Controls {
	public partial class BrowseListControl : UserControl {

		public event EventHandler ItemSelected = delegate { };
		public event EventHandler ContainerSelected = delegate { };

		private VMList VM { get { return DataContext as VMList; } }

		public BrowseListControl() {
			InitializeComponent();
		}

		private void btnBack_Click(object sender, RoutedEventArgs e) {
			VM.Back();
		}

		private void btnSelect_Click(object sender, RoutedEventArgs e) {
			var fe = sender as FrameworkElement;
			var item = fe.DataContext as ListItem;
			var vm = DataContext as VMList;

			if (null == vm || null == item) return;

			vm.SelectItem(item);
		}

		private void btnRefresh_Click(object sender, RoutedEventArgs e) {
			VM.Refresh();
		}

		private void LongListSelector_GroupViewOpened(object sender, GroupViewOpenedEventArgs e) {
			//Construct and begin a swivel animation to pop in the group view.
			IEasingFunction quadraticEase = new QuadraticEase { EasingMode = EasingMode.EaseOut };
			Storyboard _swivelShow = new Storyboard();
			ItemsControl groupItems = e.ItemsControl;

			foreach (var item in groupItems.Items) {
				UIElement container = groupItems.ItemContainerGenerator.ContainerFromItem(item) as UIElement;
				if (container != null) {
					Border content = VisualTreeHelper.GetChild(container, 0) as Border;
					if (content != null) {
						DoubleAnimationUsingKeyFrames showAnimation = new DoubleAnimationUsingKeyFrames();

						EasingDoubleKeyFrame showKeyFrame1 = new EasingDoubleKeyFrame();
						showKeyFrame1.KeyTime = TimeSpan.FromMilliseconds(0);
						showKeyFrame1.Value = -60;
						showKeyFrame1.EasingFunction = quadraticEase;

						EasingDoubleKeyFrame showKeyFrame2 = new EasingDoubleKeyFrame();
						showKeyFrame2.KeyTime = TimeSpan.FromMilliseconds(85);
						showKeyFrame2.Value = 0;
						showKeyFrame2.EasingFunction = quadraticEase;

						showAnimation.KeyFrames.Add(showKeyFrame1);
						showAnimation.KeyFrames.Add(showKeyFrame2);

						Storyboard.SetTargetProperty(showAnimation, new PropertyPath(PlaneProjection.RotationXProperty));
						Storyboard.SetTarget(showAnimation, content.Projection);

						_swivelShow.Children.Add(showAnimation);
					}
				}
			}

			_swivelShow.Begin();
		}

		private void LongListSelector_GroupViewClosing(object sender, GroupViewClosingEventArgs e) {
			//Cancelling automatic closing and scrolling to do it manually.
			e.Cancel = true;
			if (e.SelectedGroup != null) {
				selector.ScrollToGroup(e.SelectedGroup);
			}

			//Dispatch the swivel animation for performance on the UI thread.
			Dispatcher.BeginInvoke(() =>
			{
				//Construct and begin a swivel animation to pop out the group view.
				IEasingFunction quadraticEase = new QuadraticEase { EasingMode = EasingMode.EaseOut };
				Storyboard _swivelHide = new Storyboard();
				ItemsControl groupItems = e.ItemsControl;

				foreach (var item in groupItems.Items) {
					UIElement container = groupItems.ItemContainerGenerator.ContainerFromItem(item) as UIElement;
					if (container != null) {
						Border content = VisualTreeHelper.GetChild(container, 0) as Border;
						if (content != null) {
							DoubleAnimationUsingKeyFrames showAnimation = new DoubleAnimationUsingKeyFrames();

							EasingDoubleKeyFrame showKeyFrame1 = new EasingDoubleKeyFrame();
							showKeyFrame1.KeyTime = TimeSpan.FromMilliseconds(0);
							showKeyFrame1.Value = 0;
							showKeyFrame1.EasingFunction = quadraticEase;

							EasingDoubleKeyFrame showKeyFrame2 = new EasingDoubleKeyFrame();
							showKeyFrame2.KeyTime = TimeSpan.FromMilliseconds(125);
							showKeyFrame2.Value = 90;
							showKeyFrame2.EasingFunction = quadraticEase;

							showAnimation.KeyFrames.Add(showKeyFrame1);
							showAnimation.KeyFrames.Add(showKeyFrame2);

							Storyboard.SetTargetProperty(showAnimation, new PropertyPath(PlaneProjection.RotationXProperty));
							Storyboard.SetTarget(showAnimation, content.Projection);

							_swivelHide.Children.Add(showAnimation);
						}
					}
				}

				_swivelHide.Completed += _swivelHide_Completed;
				_swivelHide.Begin();

			});
		}

		private void _swivelHide_Completed(object sender, EventArgs e) {
			//--Close group view.
			if (selector != null) {
				selector.CloseGroupView();
			}
		}
	}
}
