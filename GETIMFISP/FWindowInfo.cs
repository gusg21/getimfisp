using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace GETIMFISP
{
	public struct FWindowInfo
	{
		static readonly string GAME_NAME = "GETIMFISP Game";

		// The title of the window
		public string title;
		// The size of the window
		public VideoMode windowMode;
		// Window style (fullscreen, titlebar, etc.)
		public Styles style;

		// Presets
		public static FWindowInfo SIMPLE_WINDOWED = new FWindowInfo(GAME_NAME, new VideoMode(1024, 768), Styles.Default);
		public static FWindowInfo MAXIMIZED_WINDOWED = new FWindowInfo (GAME_NAME, VideoMode.DesktopMode, Styles.None);
		public static FWindowInfo SIMPLE_FULLSCREEN = new FWindowInfo (GAME_NAME, VideoMode.DesktopMode, Styles.Fullscreen);

		public FWindowInfo(string title, VideoMode windowMode, Styles style)
		{
			this.title = title;
			this.windowMode = windowMode;
			this.style = style;
		}
	}
}