using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using yavc.Base.Data;
using yavc.Base;
using yavc.Base.Commands;

namespace yavc.Base.Models {
	public class VMPlayback : AVM {

		public MetaInfo[] Infos { get; set; }
		public PlaybackInfo PlayInfo { get; set; }

		public bool CanPlay { get { return Control.CanPlay && !IsPlaying; } }
		public bool CanPause { get { return Control.CanPause && PlayInfo == PlaybackInfo.Play; } }
		public bool CanStop { get { return Control.CanStop; } }
		public bool CanNext { get { return Control.CanNext; } }
		public bool CanPrevious { get { return Control.CanPrev; } }

		public bool IsPlaying { get { return PlayInfo == PlaybackInfo.Play; } }
		public bool IsPaused { get { return PlayInfo == PlaybackInfo.Pause; } }
		public bool IsStopped { get { return PlayInfo == PlaybackInfo.Stop; } }

		public string PlaybackString {
			get {
				switch (PlayInfo) {
					case PlaybackInfo.Play:
						return "Playing";
					case PlaybackInfo.Pause:
						return "Paused";
					case PlaybackInfo.Stop:
						return "Stopped";
					case PlaybackInfo.Unknown:
					default:
						return "Unknown";
				}
			}
		}

		private PlayControl Control;
		private bool InvalidState {
			get { return null == Zone || null == Zone.SelectedInput || string.IsNullOrEmpty(Zone.SelectedInput.Src_Name); }
		}
		private VMZone Zone;

		public VMPlayback(VMZone zone)
			: base(zone.TheController) {
			Zone = zone;
			Control = new PlayControl();
		}

		public void Refresh() {
			if (null == Zone.SelectedInput || string.IsNullOrEmpty(Zone.SelectedInput.Src_Name)) {
				Infos = new MetaInfo[0];
				PlayInfo = PlaybackInfo.Unknown;
				UI.Invoke(NotifyAll);
				return;
			}
			var info = new PlayInfo(Zone.SelectedInput);
			info.Send(TheController, result =>
			{
				if (result.Success) {
					Infos = info.MetaInfo;
					PlayInfo = info.Playback;
					UpdateCans();
					UI.Invoke(NotifyAll);
				} else {
					Infos = new MetaInfo[0];
					PlayInfo = PlaybackInfo.Unknown;
				}
			});
		}

		#region Playback Methods
		public void Next() {
			Skip(CanNext, SkipDirection.Forward);
		}

		public void Pause() {
			ChangePlayback(CanPause, PlaybackInfo.Pause);
		}

		public void Play() {
			ChangePlayback(CanPlay, PlaybackInfo.Play);
		}

		public void Previous() {
			Skip(CanPrevious, SkipDirection.Backward);
		}

		public void Stop() {
			ChangePlayback(CanStop, PlaybackInfo.Stop);
		}
		#endregion

		#region Helper Methods
		private void ChangePlayback(bool canChange, PlaybackInfo mode) {
			if (!canChange || InvalidState) return;
			var pb = new Playback(Zone.SelectedInput, mode);
			pb.Send(TheController, RefreshOnSuccess);
		}

		private void RefreshOnSuccess(SendResult result) {
			if (result.Success) Refresh();
		}

		private void Skip(bool canSkip, SkipDirection direction) {
			if (!canSkip || InvalidState) return;
			var skip = new Skip(Zone.SelectedInput, direction);
			skip.Send(TheController, RefreshOnSuccess);
		}

		private void UpdateCans() {
			if (
				null == TheController || null == TheController.Sources ||
				null == Zone || null == Zone.SelectedInput) {
				Control = new PlayControl();
				return;
			}
			var source = TheController.Sources.FirstOrDefault(s => s.SourceName == Zone.SelectedInput.Src_Name);

			if (null == source || null == source.Control) {
				Control = new PlayControl();
				return;
			}

			Control = source.Control;
		} 
		#endregion
	}
}
