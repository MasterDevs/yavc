using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Yamaha.AVControl.Commands;
using Yamaha.AVControl.Commands.Data;

namespace Yamaha.AVControl.Desktop.Models {
	/// <summary>
	/// This class is designed to be able to provide enough
	/// info to select a Zone, Input, Scene or DSP program.
	/// </summary>
	public class VMSelectable : AVM {

		public string ImageUri {
			get { return (string)GetValue(ImageUriProperty); }
			set { SetValue(ImageUriProperty, value); }
		}
		public static readonly DependencyProperty ImageUriProperty =
				DependencyProperty.Register("ImageUri", typeof(string), typeof(VMSelectable));

		public string DisplayName {
			get { return (string)GetValue(DisplayNameProperty); }
			set { SetValue(DisplayNameProperty, value); }
		}
		public static readonly DependencyProperty DisplayNameProperty =
				DependencyProperty.Register("DisplayName", typeof(string), typeof(VMSelectable));

		public VMSelectableType SelectionType {
			get { return (VMSelectableType)GetValue(SelectionTypeProperty); }
			set { SetValue(SelectionTypeProperty, value); }
		}
		public static readonly DependencyProperty SelectionTypeProperty =
				DependencyProperty.Register("SelectionType", typeof(VMSelectableType), typeof(VMSelectable));

		private string _Name;
		private VMMain _Main;

		protected VMSelectable(Controller c, VMMain mainVM) : base(c) {
			_Main = mainVM;
		}
		public VMSelectable(Controller c, VMMain mainVM, Zone zone, string imageUri)
			: this(c, mainVM) {
			_Name = zone.Name;
			DisplayName = zone.DisplayName;
			ImageUri = imageUri;
			SelectionType = VMSelectableType.Zone;
		}

		public VMSelectable(Controller c, VMMain mainVM, Scene s)
			: this(c, mainVM) {
			_Name = s.Name;
			DisplayName = s.DisplayName;
			ImageUri = SelectSceneImageUri(s.Name);
			SelectionType = VMSelectableType.Scene;
		}

		private string SelectSceneImageUri(string sceneName) {
			switch (sceneName.ToLower()) {
				case "scene 1":
					return @"pack://application:,,,/Images/Xbox.png";
				case "scene 2":
					return @"pack://application:,,,/Images/MainZone.png";
				case "scene 3":
					return @"pack://application:,,,/Images/Scene.png";
				case "scene 4":
					return @"pack://application:,,,/Images/PC.png";
				default:
					return @"pack://application:,,,/Images/Scene.png";
			}
		}

		public VMSelectable(Controller c, VMMain mainVM, Input i)
			: this(c, mainVM) {
			_Name = i.Param;
			DisplayName = i.Title;
			ImageUri = string.Format("http://{0}{1}", TheController.Address, i.IconOn);
			SelectionType = VMSelectableType.Input;
		}

		public VMSelectable(Controller c, VMMain mainVM, string name, string displayName, string imagePath, VMSelectableType type)
			: base(c) {
			_Name = name;
			_Main = mainVM;
			ImageUri = imagePath;
			DisplayName = displayName;
			SelectionType = type;
		}

		public void Select() {
			switch (SelectionType) {
				case VMSelectableType.Zone:
					_Main.SelectZone(_Name);
					return;
				case VMSelectableType.Scene:
					_Main.SelectScene(_Name);
					return;
				case VMSelectableType.DSP:
					_Main.SelectDSP(_Name);
					return;
				case VMSelectableType.Input:
					_Main.SelectInput(_Name);
					return;
				default:
					break;
			}
		}

	}

	public enum VMSelectableType {
		Zone,
		Scene,
		DSP,
		Input
	}
}
