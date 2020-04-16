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
	/// Represents the data entered into the debug console.
	/// </summary>
	public class DebugConsoleEventArgs : EventArgs
	{
		/// <summary>
		/// The whole text
		/// </summary>
		public string Text;
		/// <summary>
		/// The first word without the /
		/// </summary>
		public string Command { get { return Text.Split (' ').First (); } }
		/// <summary>
		/// The rest of the words
		/// </summary>
		public string [] Args { get { return Text.Split (' ').Skip (1).ToArray(); } }
	}

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
		string inputString;

		/// <summary>
		/// Triggered when a /-led command is typed in console
		/// </summary>
		public event EventHandler<DebugConsoleEventArgs> OnCommand;
		/// <summary>
		/// Triggered when any text is submitted
		/// </summary>
		public event EventHandler<DebugConsoleEventArgs> OnText;

		
		private FDebugConsole()
		{
			UseBuiltinRenderer = false;
			Persistent = true;

			text = new Text ("Test!", new Font("Resources/Ubuntu-R.ttf"), 17);
		}

		public override void OnGraphicsReady()
		{
			base.OnGraphicsReady ();

			Game.Window.TextEntered += Window_TextEntered;
		}

		private void Window_TextEntered(object sender, SFML.Window.TextEventArgs e)
		{
			if (e.Unicode == "\b")
			{
				if (inputString.Length > 0)
				{
					inputString = inputString.Substring (0, inputString.Length - 1);
				}
				return;
			}

			if (e.Unicode == "\r")
			{
				FDebug.WriteLine ($"> {inputString}");
				OnText?.Invoke (this, new DebugConsoleEventArgs () { Text = inputString }); // TODO: Add built-in commands for actors?

				inputString = "";
				return;
			}

			inputString += e.Unicode;
		}

		/// <summary>
		/// Draw the debug console. Drawn in screen space (GUI) so that it doesn't float away when the camera moves.
		/// </summary>
		/// <param name="target"></param>
		/// <param name="states"></param>
		public override void DrawGUI(RenderTarget target, RenderStates states)
		{
			base.DrawGUI (target, states);

			// OUTPUT
			float y = 0;
			FDebug.DebugLines.Reverse ();
			foreach (FDebugLine line in FDebug.DebugLines)
			{
				text.DisplayedString = $"[{line.LogLevel}] {line.Text}";
				text.Color = FDebug.ChannelColors [line.Channel].ToSfmlColor().Alpha(128);
				if (line.LogLevel == FLogLevel.ERROR)
				{
					text.Color = ConsoleColor.DarkRed.ToSfmlColor ();
				}
				text.Position = new Vector2f (5, Game.Window.Size.Y - 20 - 45 - (20 * y));

				target.Draw (text, states);

				y++;

				if (y > 18)
				{
					break;
				}
			}
			FDebug.DebugLines.Reverse ();

			// INPUT
			text.DisplayedString = $"> {inputString}" + (Game.GameTime.Elapsed.AsSeconds() % 2 > 1 ? "|" : "");
			text.Position = new Vector2f (5, Game.Window.Size.Y - 25);
			text.Color = Color.White;
			target.Draw (text, states);
		}
	}
}
