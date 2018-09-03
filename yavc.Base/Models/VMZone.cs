using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using yavc.Base.Data;
using yavc.Base;
using yavc.Base.Commands;
using yavc.Base.Util;

namespace yavc.Base.Models {
	public class VMZone : VMSelectable {

		#region Properties

		public bool CanViewPlayback {
			get {
				if (null == SelectedInput) return false;
				var source = TheController.Sources.FirstOrDefault(s => s.SourceName == SelectedInput.Src_Name);
				if (null == source) return false;
				return source.PlayInfo;
			}
		}
		public bool CanSelectDSP { get { return TheZone.Surround != null; } }
		public bool CanSelectScene { get { return IsOn && Scenes.Length > 0; } }
		public bool CanList { 
            get {
                if (List == null) return false;
                return  List.CanList && IsOn; 
            } 
        }
        public string DSPTitle
        {
            get { return GetValue(() => DSPTitle); }
            set { SetValue(value, () => DSPTitle); }
        }
		
        private readonly VMSelectable[] _DSPs;
		public VMSelectable[] DSPs { get { return _DSPs; } }

        public string InputTitle
        {
            get { return GetValue(() => InputTitle); }
            set { SetValue(value, () => InputTitle); }
        }
        public VMSelectable[] Inputs
        {
            get { return GetValue(() => Inputs); }
            set { SetValue(value, () => Inputs); }
        }
        
        public bool IsActiveZone { get { return MainVM.SelectedZone == this; } }

        public bool IsOn
        {
            get { return GetValue(() => IsOn); }
            set {
                if (SetValue(value, () => IsOn))
                {
                    Notify(() => IsOnString);
                    Volume.Refresh(); //-- Volume has an IsOn property that is called into b
                }
            }
        }
		
        public string IsOnString {
			get { return IsOn ? "on" : "off"; }
		}

        public bool IsPureDirectOn
        {
            get { return GetValue(() => IsPureDirectOn); }
            set { SetValue(value, () => IsPureDirectOn); }
        }

        public VMList List 
        {
            get { return GetValue(() => List ); }
            set { SetValue(value, () => List ); }
        }

        public VMPlayback Playback
        {
            get { return GetValue(() => Playback); }
            set { SetValue(value, () => Playback); }
        }

        public VMSelectable[] Scenes
        {
            get { return GetValue(() => Scenes); }
            set { SetValue(value, () => Scenes); }
        }

        public Input SelectedInput
        {
            get { return GetValue(() => SelectedInput); }
            set
            {
                if (SetValue(value, () => SelectedInput))
                {
                    Notify(() => CanViewPlayback);
                }
            }
        }
        
        public Zone TheZone { get; private set; }

        public VMVolume Volume
        {
            get { return GetValue(() => Volume); }
            set { SetValue(value, () => Volume); }
        }

        public string ZoneTitle
        {
            get { return GetValue(() => ZoneTitle); }
            set { SetValue(value, () => ZoneTitle); }
        }
        #endregion

		public VMZone(VMMain main, Zone zone, IController c)
			: base(c, main, zone, null) {
			if (zone.Inputs != null)
				Inputs = zone.Inputs.Select(i => new VMSelectable(TheController, main, i)).ToArray();
			else
				Inputs = new VMSelectable[0];

			if (zone.Scenes != null)
				Scenes = zone.Scenes.Select(s => new VMSelectable(TheController, main, s)).ToArray();
			else
				Scenes = new VMSelectable[0];

			Volume = new VMVolume(this, c);
			Playback = new VMPlayback(this);

			_DSPs = new VMSelectable[] {
					new VMSelectable(TheController, MainVM, Surround.DSP_Strait, VMSelectableType.DSP),
					new VMSelectable(TheController, MainVM, Surround.DSP_Standard, VMSelectableType.DSP),
					new VMSelectable(TheController, MainVM, Surround.DSP_Stereo_2CH, VMSelectableType.DSP),
					new VMSelectable(TheController, MainVM, Surround.DSP_Stereo_7CH, VMSelectableType.DSP),
					new VMSelectable(TheController, MainVM, Surround.DSP_SurroundDecoder, VMSelectableType.DSP),
					new VMSelectable(TheController, MainVM, Surround.DSP_ActionGame, VMSelectableType.DSP),
					new VMSelectable(TheController, MainVM, Surround.DSP_Adventure, VMSelectableType.DSP),
					new VMSelectable(TheController, MainVM, Surround.DSP_CellarClub, VMSelectableType.DSP),
					new VMSelectable(TheController, MainVM, Surround.DSP_Chamber, VMSelectableType.DSP),
					new VMSelectable(TheController, MainVM, Surround.DSP_Drama, VMSelectableType.DSP),
					new VMSelectable(TheController, MainVM, Surround.DSP_HallInMunich, VMSelectableType.DSP),
					new VMSelectable(TheController, MainVM, Surround.DSP_HallInViena, VMSelectableType.DSP),
					new VMSelectable(TheController, MainVM, Surround.DSP_MonoMovie, VMSelectableType.DSP),
					new VMSelectable(TheController, MainVM, Surround.DSP_MusicVideo, VMSelectableType.DSP),
					new VMSelectable(TheController, MainVM, Surround.DSP_RoleplayingGame, VMSelectableType.DSP),
					new VMSelectable(TheController, MainVM, Surround.DSP_SciFi, VMSelectableType.DSP),
					new VMSelectable(TheController, MainVM, Surround.DSP_Spectacle, VMSelectableType.DSP),
					new VMSelectable(TheController, MainVM, Surround.DSP_Sports, VMSelectableType.DSP),
					new VMSelectable(TheController, MainVM, Surround.DSP_TheBottomLine, VMSelectableType.DSP),
					new VMSelectable(TheController, MainVM, Surround.DSP_TheRoxyTheatre, VMSelectableType.DSP),
				};
			RefreshVM(zone);
		}

		public void PinToStart() {
			Factory.TileService.CreateOrUpdateTile(TheController.Device, TheZone);
		}

		public void Refresh(Action onComplete) {
			TheController.RefreshZone(TheZone, result =>
			{
				if (result.Success)
					UI.Invoke(RefreshVM);
				onComplete.NullableInvoke();
			});
		}

        /// <summary>
        /// Notifies all listeners of all public properties.
        /// </summary>
        public void RefreshLocal()
        {
            NotifyAll();
            NotifyAll();
        }

		public void SelectDSP(string name) {
			if (TheZone.Surround == null) return; //-- Can't select DSP if we don't have a surround

			//-- Select the DSP locally to update the view immediatly.
			TheZone.Surround = new Surround(name == Surround.DSP_Strait, TheZone.Surround.EnhancerOn, name);

			//-- Refresh Locally
			RefreshVM();

			//-- Make the network call
			TheController.SelectDSP(TheZone);
		}

		public void SelectInput(Input input, Action OnSuccess) {
			SaveOld();

			//-- Update our local VM with the newly selected Input
			var selString = GetSelectString(input);
			foreach (var i in Inputs) {
				if (i.SelectString == selString) {
					i.IsSelected = true;
					ImageUri = i.ImageUri;
					InputTitle = i.DisplayName;
				} else {
					i.IsSelected = false;
				}
			}

			//-- Make the call to select the input
			TheController.SelectInput(TheZone, input, result =>
			{
				if (result.Success) {
					UI.Invoke(() =>
					{
						SelectedInput = input;
						List = new VMList(MainVM, input, TheZone);
						Playback.Refresh();
						OnSuccess.NullableInvoke();
					});
					return;
				};
				RestoreToOld();
			});
		}

		public void SelectScene(Scene scene, Action onFinished) {
			SaveOld();

			TheController.SelectScene(TheZone, scene, result =>
			{
				//-- For Scenes, we need to figure out which input
				// corresponds to the scene. So we update the zone after
				// we successfully selected the scene. 
				if (result.Success) {
					TheController.RefreshZone(TheZone, UpdateResult =>
					{
						if (!UpdateResult.Success) {
							RestoreToOld();
							return;
						}

						//-- If we successfully updated the zone we 
						// can refresh this VM then.
						UI.Invoke(() =>
						{
							RefreshVM();
							onFinished.NullableInvoke();
						});
					});
					return;
				}

				RestoreToOld();
			});
		}

		public void ToggelPower() {
            if (MainVM.IsRefreshing) return; 
            
            IsOn = !IsOn;
			TheController.TogglePower(TheZone, result =>
			{
				UI.Invoke(() =>
				{
					if (result.Success) {
						MainVM.RefreshVM();
					} else {
						IsOn = !IsOn;
					}
				});
			});
		}

		public void TogglePureDirect() {
			IsPureDirectOn = !IsPureDirectOn;
			TheController.SetPureDirect(TheZone, IsPureDirectOn, null);
			//-- No need to refresh our Zone because we already flipped it's IsPureDirectOn property.
		}

		#region Helper Methods / Variables
		private string oldSelectedInput;
		private string oldTitle;
		private string oldImage;

		private void SaveOld() {
			oldTitle = InputTitle;
			oldImage = ImageUri;
			foreach (var i in Inputs) {
				if (i.IsSelected) oldSelectedInput = i.SelectString;
			}
		}

		private void RefreshVM() {
			RefreshVM(TheZone);
		}

		private void RefreshVM(Zone zone) {
			TheZone = zone;

			if (null == zone.Surround) {
				DSPTitle = "Default";
			} else {
				if (zone.Surround.StraitOn)
					DSPTitle = Surround.DSP_Strait;
				else
					DSPTitle = zone.Surround.DSP;
			}
			foreach (var dsp in DSPs) {
				dsp.IsSelected = (dsp.SelectString == DSPTitle);
			}

			IsOn = zone.PowerOn;
			IsPureDirectOn = zone.PureDirectOn;

			//-- Updated Selected Input
			for (int i = 0; i < zone.Inputs.Length; i++) {
				if (zone.Inputs[i].Param == zone.SelectedInput) {
					InputTitle = Inputs[i].DisplayName;
					ImageUri = Inputs[i].ImageUri;
					Inputs[i].IsSelected = true;

					SelectedInput = zone.Inputs[i];
					List = new VMList(MainVM, zone.Inputs[i], zone);
				} else {
					Inputs[i].IsSelected = false;
				}
			}

			Volume.Refresh();
			Playback.Refresh();
			ZoneTitle = zone.DisplayName;
		}

		private void RestoreToOld() {
			UI.Invoke(() =>
			{
				ImageUri = oldImage;
				InputTitle = oldTitle;
				foreach (var i in Inputs) {
					i.IsSelected = i.SelectString == oldSelectedInput;
				}
			});
		}
		#endregion
    }
}
