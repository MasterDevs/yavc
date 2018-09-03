using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace yavc.WinRT.Base.Media {
	public class Song {

		public string Album { get; set; }
		public string AlbumCoverArtUri { get; set; }
		public string Artist { get; set; }
		public string MusicUri { get; set; }
		public string Title { get; set; }
		public uint TrackNumber { get; set; }

		public static Song Sample01 {
			get {
				return new Song()
				{
					Album = "Outsider",
					AlbumCoverArtUri = @"http://192.168.0.102:50163/music/G-Eazy/Mercedes.Benz.Cover.jpg",
					Artist = "G-Eazy",
					MusicUri = @"http://192.168.0.102:50163/music/G-Eazy/Mercedes Benz (The American Dream) ft. Janis Joplin.mp3",
					Title = "Mercedes Benz"
				};
			}
		}

		public static Song Sample02 {
			get {
				return new Song()
				{
					Album = "Big",
					AlbumCoverArtUri = @"http://192.168.0.102:50163/music/G-Eazy/Big/Big.Cover.jpg",
					Artist = "G-Eazy",
					MusicUri = @"http://192.168.0.102:50163/music/G-Eazy/Big/G-Eazy - Big - 11 Apple Of My Eye ft. Erika Flowers.mp3",
					Title = "Apple of My Eye ft. Erika Flowers"
				};
			}
		}

		public static Song Sample03 {
			get {
				return new Song()
				{
					Album = "The Endless Summer",
					AlbumCoverArtUri = @"http://192.168.0.102:50163/music/G-Eazy/The Endless Summer/cover.jpg",
					Artist = "G-Eazy",
					MusicUri = @"http://192.168.0.102:50163/music/G-Eazy/Big/The Endless Summer/G-Eazy - The Endless Summer - 03 Runaround Sue ft. Greg Banks",
					Title = "Runaround Sue ft. Greg Banks"
				};
			}
		}
	}
}
