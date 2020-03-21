using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace GETIMFISP
{
	/// <summary>
	/// A class that contains the settings for a window
	/// </summary>
	public struct FWindowSettings
	{
		static readonly string DEFAULT_GAME_NAME = "GETIMFISP Game";

		/// <summary>
		/// The title of the window
		/// </summary>
		public string Title;
		/// <summary>
		/// The VideoMode for the window
		/// </summary>
		public VideoMode WindowMode;
		/// <summary>
		/// The window style (fullscreen, titlebar, resizable, etc.)
		/// </summary>
		public Styles Style;

		/// <summary>
		/// A medium window that is resizable etc
		/// </summary>
		public static FWindowSettings SIMPLE_WINDOWED = new FWindowSettings(DEFAULT_GAME_NAME, new VideoMode(1024, 768), Styles.Default);
		/// <summary>
		/// A window as big as possible
		/// </summary>
		public static FWindowSettings MAXIMIZED_WINDOWED = new FWindowSettings (DEFAULT_GAME_NAME, VideoMode.DesktopMode, Styles.None);
		/// <summary>
		/// A fullscreen window
		/// </summary>
		public static FWindowSettings SIMPLE_FULLSCREEN = new FWindowSettings (DEFAULT_GAME_NAME, VideoMode.DesktopMode, Styles.Fullscreen);

		/// <summary>
		/// Create a window settings object
		/// </summary>
		/// <param name="title"></param>
		/// <param name="windowMode"></param>
		/// <param name="style"></param>
		public FWindowSettings(string title, VideoMode windowMode, Styles style)
		{
			this.Title = title;
			this.WindowMode = windowMode;
			this.Style = style;
		}
	}
}