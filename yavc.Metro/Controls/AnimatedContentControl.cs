using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace yavc.Metro.Controls {
	public class AnimatedContentControl : ContentControl {

		public AnimatedContentControl() : base() {
			Initialize();
		}

		private Storyboard ShowSB;
		private Storyboard HideSB;

		private void Initialize() {
			Name = Guid.NewGuid().ToString();
			Initialize(0, 1, 200, TimeSpan.FromMilliseconds(500D), ref ShowSB, null);
			Initialize(1, 0, -200, TimeSpan.FromMilliseconds(500D), ref HideSB, (s, e) => Visibility = Visibility.Collapsed);
		}

		private void Initialize(double from, double to, double fromHoriz, TimeSpan duration, ref Storyboard sb, EventHandler<object> onCompleted) {
			var opacityAnimiation = new DoubleAnimation();
			opacityAnimiation.From = from;
			opacityAnimiation.To = to;
			opacityAnimiation.Duration = duration;


			var repo = new RepositionThemeAnimation();
			repo.TargetName = Name;
			repo.FromHorizontalOffset = fromHoriz;

			sb = new Storyboard();
			sb.Duration = duration;
			sb.Children.Add(opacityAnimiation);
			sb.Children.Add(repo);
			if(onCompleted != null)
				sb.Completed += onCompleted;

			Storyboard.SetTarget(opacityAnimiation, this);
			Storyboard.SetTargetProperty(opacityAnimiation, "Opacity");
			Storyboard.SetTarget(repo, this);
		}

		public void Show() {
			Visibility = Visibility.Visible;
			ShowSB.Begin();
		}

		public void Hide() {
			HideSB.Begin();
		}
	}
}
