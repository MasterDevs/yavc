using System;
using yavc.Base.Data;
using yavc.Base.Commands;

namespace yavc.Base {
	public interface IController {

		#region Properties
		string HostNameorAddress { get; }
		string RecieverName { get; }

		Device Device { get; }
		bool IsConnected { get; }
		bool IsPartyModeOn { get; }
		Zone[] Zones { get; }
		Source[] Sources { get; }
		IProcessRequest RequestProccessor { get; }
		#endregion

		void ChangeVolume(Zone z, Action<SendResult> OnComplete);

		void ListMenuUp(Input i, Action<SendResult> OnComplete);
		void SelectCurrentListItem(Input input, Action<SendResult> OnComplete);
		
		void SelectDSP(Zone z);
		void SelectInput(Zone zone, Input input, Action<SendResult> OnComplete);
		void SelectScene(Zone zone, Scene scene, Action<SendResult> OnComplete);

		void SetPartyMode(bool partyModeOn, Action<SendResult> OnComplete);
		void SetPureDirect(Zone zone, bool pureDirectOn, Action<SendResult> OnComplete);
		void ToggleMute(Zone zone, Action<SendResult> OnComplete);
		void TogglePower(Zone zone, Action<SendResult> OnComplete);
		void TrySetDevice(Device d, Action<SendResult> onComplete);
		void RefreshZone(Zone z, Action<SendResult> OnComplete);
		void UpdateStatus(Action<SendResult> OnComplete);
		void VolumeDown(Zone z, Action<SendResult> OnComplete);
		void VolumeUp(Zone z, Action<SendResult> OnComplete);
	}
}
