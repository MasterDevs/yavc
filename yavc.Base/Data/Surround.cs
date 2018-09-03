using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace yavc.Base.Data {
	public class Surround {


		public Surround() { }
		public Surround(bool straitOn, bool enhancerOn, string dsp) {
			StraitOn = straitOn;
			EnhancerOn = enhancerOn;
			DSP = dsp;
		}
		[XmlElement]
		public bool StraitOn { get; set; }
		[XmlElement]
		public bool EnhancerOn { get; set; }
		[XmlElement]
		public string DSP { get; set; }

		//Surrounds
		public const string DSP_Strait = "Strait";
		public const string DSP_Stereo_2CH = "2ch Stereo";
		public const string DSP_Stereo_7CH = "7ch Stereo";
		public const string DSP_SurroundDecoder = "Surround Decoder";
		public const string DSP_HallInMunich = "Hall in Munich";
		public const string DSP_HallInViena = "Hall in Vienna";
		public const string DSP_Chamber = "Chamber";
		public const string DSP_CellarClub = "Cellar Club";
		public const string DSP_TheRoxyTheatre = "The Roxy Theatre";
		public const string DSP_TheBottomLine = "The Bottom Line";
		public const string DSP_ActionGame = "Action Game";
		public const string DSP_Sports = "Sports";
		public const string DSP_RoleplayingGame = "Roleplaying Game";
		public const string DSP_MusicVideo = "Music Video";
		public const string DSP_Standard = "Standard";
		public const string DSP_SciFi = "Sci-Fi";
		public const string DSP_Spectacle = "Spectacle";
		public const string DSP_Adventure = "Adventure";
		public const string DSP_Drama = "Drama";
		public const string DSP_MonoMovie = "Mono Movie";
	}
}
