using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Yamaha.AVControl.Commands;
using Yamaha.AVControl.Commands.Data;

namespace Yamaha.AVControl.Desktop.Models {
	public class VMZone : AVM {

		public string DSPTitle {
			get { return (string)GetValue(DSPTitleProperty); }
			set { SetValue(DSPTitleProperty, value); }
		}
		public static readonly DependencyProperty DSPTitleProperty =
				DependencyProperty.Register("DSPTitle", typeof(string), typeof(VMZone));

		public string ImageUrl {
			get { return (string)GetValue(ImageUrlProperty); }
			set { SetValue(ImageUrlProperty, value); }
		}
		public static readonly DependencyProperty ImageUrlProperty =
				DependencyProperty.Register("ImageUrl", typeof(string), typeof(VMZone));

		public string InputImage {
			get { return (string)GetValue(InputImageProperty); }
			set { SetValue(InputImageProperty, value); }
		}
		public static readonly DependencyProperty InputImageProperty =
				DependencyProperty.Register("InputImage", typeof(string), typeof(VMZone));
		
		public string InputTitle {
			get { return (string)GetValue(InputTitleProperty); }
			set { SetValue(InputTitleProperty, value); }
		}
		public static readonly DependencyProperty InputTitleProperty =
				DependencyProperty.Register("InputTitle", typeof(string), typeof(VMZone));

		public bool IsOn {
			get { return (bool)GetValue(IsOnProperty); }
			set { SetValue(IsOnProperty, value); }
		}
		public static readonly DependencyProperty IsOnProperty =
				DependencyProperty.Register("IsOn", typeof(bool), typeof(VMZone));

		public string IsOnString {
			get { return (string)GetValue(IsOnStringProperty); }
			set { SetValue(IsOnStringProperty, value); }
		}
		public static readonly DependencyProperty IsOnStringProperty =
				DependencyProperty.Register("IsOnString", typeof(string), typeof(VMZone));

		public VMVolume Volume {
			get { return (VMVolume)GetValue(VolumeProperty); }
			set { SetValue(VolumeProperty, value); }
		}
		public static readonly DependencyProperty VolumeProperty =
				DependencyProperty.Register("Volume", typeof(VMVolume), typeof(VMZone));

		public string ZoneTitle {
			get { return (string)GetValue(ZoneTitleProperty); }
			set { SetValue(ZoneTitleProperty, value); }
		}
		public static readonly DependencyProperty ZoneTitleProperty =
				DependencyProperty.Register("ZoneTitle", typeof(string), typeof(VMZone));
		
		
		public Zone TheZone { get; protected set; }
		public VMMain MainVM { get; protected set; }
		public VMZone(VMMain main, Zone zone, Controller c, string imageUri) : base(c) {
			MainVM = main; 
			Update(zone, imageUri);
		}

		public void Update(Zone zone, string imageUri) {
			DSPTitle = zone.Surround.DSP;
			IsOn = zone.PowerOn;
			IsOnString = zone.PowerOn ? "On" : "Off";
			ImageUrl = imageUri;

			//-- Get Selected Input
			foreach (var i in zone.Inputs) {
				if (i.Param == zone.SelectedInput) {
					InputTitle = i.Title;
					InputImage = string.Format("http://{0}{1}", TheController.Address, i.IconOn);
					break;
				}
			}

			Volume = new VMVolume(zone, TheController);
			TheZone = zone;
			ZoneTitle = zone.DisplayName;
		}

		public void ToggelPower() {
			TheController.TogglePower(TheZone);
		}

		public void Refresh() {
			TheController.UpdateStatus();
		}
	}
}
