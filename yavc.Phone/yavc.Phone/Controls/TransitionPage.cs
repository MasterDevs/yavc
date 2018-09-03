using System.Windows;
using Microsoft.Phone.Controls;
namespace yavc.Phone.Controls {
	/// <summary>
	/// See post by Paul Sinnema 
	/// http://rhizohm.net/irhetoric/post/2010/11/09/Page-Transition-Animations-and-Windows-Phone-7.aspx
	/// </summary>
	public class TransitionPage : PhoneApplicationPage {

		public TransitionPage() {
			SetTransition(this);
		}

		private static void SetTransition(UIElement element) {
			SetInTransition(element);
			SetOutTransition(element);
		}

		private static void SetInTransition(UIElement element) {
			var inTransition = new NavigationInTransition();
			
			inTransition.Backward = Create(SlideTransitionMode.SlideRightFadeIn);
			inTransition.Forward = Create(SlideTransitionMode.SlideLeftFadeIn);
			TransitionService.SetNavigationInTransition(element, inTransition);
		}

		private static void SetOutTransition(UIElement element) {
			var outTransition = new NavigationOutTransition();
			
			outTransition.Backward = Create(SlideTransitionMode.SlideRightFadeOut);
			outTransition.Forward = Create(SlideTransitionMode.SlideLeftFadeOut);
			TransitionService.SetNavigationOutTransition(element, outTransition);
		}

		private static TransitionElement Create(SlideTransitionMode forwardMode) {
			return new SlideTransition() { Mode = forwardMode };
		}
	}
}
