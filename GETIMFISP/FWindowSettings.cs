using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace GETIMFISP
{
	public struct FWindowSettings
	{
		static readonly string GAME_NAME = "GETIMFISP Game";

		// The title of the window
		public string title;
		// The size of the window
		public VideoMode windowMode;
		// Window style (fullscreen, titlebar, etc.)
		public Styles style;

		// Presets
		public static FWindowSettings SIMPLE_WINDOWED = new FWindowSettings(GAME_NAME, new VideoMode(1024, 768), Styles.Default);
		public static FWindowSettings MAXIMIZED_WINDOWED = new FWindowSettings (GAME_NAME, VideoMode.DesktopMode, Styles.None);
		public static FWindowSettings SIMPLE_FULLSCREEN = new FWindowSettings (GAME_NAME, VideoMode.DesktopMode, Styles.Fullscreen);

		public FWindowSettings(string title, VideoMode windowMode, Styles style)
		{
			this.title = title;
			this.windowMode = windowMode;
			this.style = style;
		}
	}
}