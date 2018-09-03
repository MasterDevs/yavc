using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using yavc.Base.Data;
using yavc.Base;
using yavc.Base.Commands;
using yavc.Base.Util;

namespace yavc.Base.Models {
	public class VMDevice : AVM {

		public Device Device;
		public VMStart StartVM;

		public VMDevice(VMStart startVM, Device d) : this(startVM, d, false) { }

		public VMDevice(VMStart startVM, Device d, bool isLoaded)
			: base(new Controller(d)) {
			Device = d;
			if (isLoaded)
				PercentageLoaded = 1;
			else
				PercentageLoaded = -1;
			StartVM = startVM;
		}

		public string FriendlyName { get { return Device.FriendlyName; } }
		public string HostnameOrIp { get { return Device.HostnameOrIp; } }
		public string ImageUri { get { return Device.ImageUri; } }
		public bool IsIndeterminate { get; private set; }
		public bool IsLoading { get; private set; }
		public bool IsLoaded { get { return PercentageLoaded == 1D; } }
		public string LoadingMessage {
			get { return Device.LoadingMesssage; }
			private set { Device.LoadingMesssage = value; }
		}
		public double PercentageLoaded { get; private set; }
		public bool InvalidDevice { 
			get { return Device.InvalidDevice; }
			private set { Device.InvalidDevice = value; }
		}

		public void Refresh() {
			if (IsLoading) return; 
			IsLoading = InvalidDevice = IsIndeterminate = true;
			PercentageLoaded = 0D;
			LoadingMessage = "Connecting to device . . . ";
			UI.Invoke(NotifyAll);
			TheController.TrySetDevice(Device, sr =>
			{
				if (sr.Success) {
					LoadReceiver();
				} else {
					InvalidDevice = true;
					IsLoading = IsIndeterminate = false;
					PercentageLoaded = 0D;
					LoadingMessage = "There was an error connecting to the device";
                    UI.Invoke(NotifyAll);
				}
			});
		}

		public void RefreshDevice(Device d) {
			Device = d;
			//-- If the user had a device tile, lets make sure it's name change is reflected
			Factory.TileService.UpdateTileIfExists(d);
			Refresh();
		}

		#region Helper Methods
		private void DownloadDeviceImage() {
			Factory.ImageCache.DownloadImage(ImageUri, UI.Wrap(() => Notify(() => ImageUri)));
		}

		private void LoadReceiver() {
			LoadingMessage = "Connecting to device . . .";
			UI.Invoke(NotifyAll); 
			
			var vm = new VMMain(TheController);
			
			DownloadDeviceImage();

			var images = new Dictionary<string, bool>();
			foreach (var z in vm.Zones) {
				foreach (var s in z.Scenes) {
					images[s.ImageUri] = false;
				}
				foreach (var i in z.Inputs) {
					images[i.ImageUri] = false;
				}
			}

			var all = images.Keys.ToList();

			if (all.Count < 0) {
				UI.Invoke(NotifyAll);
				return; 
			}

			DownloadImages(new VMMain(TheController), all, images.Count, 0);
		}

		private void DownloadImages(VMMain vm, List<string> images, double totalImages, double imagesLoaded) {
			if (images.Count == 0) {
				PercentageLoaded = 1;
				LoadingMessage = " ";
				IsLoading = IsIndeterminate = InvalidDevice = false;
				SessionManager.SaveState(StartVM, () =>
				{
					SessionManager.SaveState(vm, () => UI.Invoke(NotifyAll));
				});
				return;
			}

			var percentageComplete = (imagesLoaded / totalImages);
            UI.Invoke(() =>
            {
                IsIndeterminate = false;
                LoadingMessage = string.Format("Loading: {0}%", ((int)(percentageComplete * 100D)));
                PercentageLoaded = percentageComplete;
            });
			
			var img = images[0];
			images.RemoveAt(0);
			Factory.ImageCache.DownloadImage(img, () =>
			{
				DownloadImages(vm, images, totalImages, ++imagesLoaded);
			});
		}
		#endregion
	}
}
