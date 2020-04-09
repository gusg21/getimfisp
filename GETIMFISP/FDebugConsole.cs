using GETIMFISP.Extensions;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GETIMFISP
{
	/// <summary>
	/// A simple gui that shows print statements in-game, along with a subscribable input box
	/// </summary>
	public class FDebugConsole : FActor
	{
		private static FDebugConsole instance;
		/// <summary>
		/// This class is a singleton, so use this to get the FDebugConsole to work with.
		/// 
		/// Note: this won't be drawn or updated in Release mode! You'd have to do that manually.
		/// </summary>
		public static FDebugConsole Instance { get
			{
				if (instance == null)
					instance = new FDebugConsole ();

				return instance;
			}
		}

		Text text;
		
		private FDebugConsole()
		{
			UseBuiltinRenderer = false;
			Persistent = true;

			text = new Text ("Test!", new Font("Resources/Ubuntu-R.ttf"), 17);
		}

		/// <summary>
		/// Draw the debug console. Drawn in screen space (GUI) so that it doesn't float away when the camera moves.
		/// </summary>
		/// <param name="target"></param>
		/// <param name="states"></param>
		public override void DrawGUI(RenderTarget target, RenderStates states)
		{
			base.DrawGUI (target, states);

			FDebug.WriteLine (Game.Window.Size.ToString());

			float y = 0;
			foreach (FDebugLine line in FDebug.DebugLines)
			{
				text.DisplayedString = $"[{line.LogLevel}] {line.Text}";
				text.Color = FDebug.ChannelColors [line.Channel].ToSfmlColor().Alpha(128);
				if (line.LogLevel == FLogLevel.ERROR)
				{
					text.Color = ConsoleColor.DarkRed.ToSfmlColor ();
				}
				text.Position = new Vector2f (5, Game.Window.Size.Y - 20 - 5 - (20 * y));

				target.Draw (text, states);

				y++;
			}
		}
	}
}
