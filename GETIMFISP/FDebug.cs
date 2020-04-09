using GETIMFISP.Extensions;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GETIMFISP
{
	/// <summary>
	/// The class for all things debugging.
	/// </summary>
	public static class FDebug
	{
		/// <summary>
		/// All the colors for each channel.
		/// </summary>
		public static Dictionary<int, ConsoleColor> ChannelColors;

		/// <summary>
		/// A list of all the the things printed to the console.
		/// </summary>
		public static List<FDebugLine> DebugLines { get; }

		static Queue<object> primitives;

		/// <summary>
		/// Create the debugger. This will set itself as the console output.
		/// </summary>
		static FDebug()
		{
			// Foreground colors by channel of output.
			ChannelColors = new Dictionary<int, ConsoleColor> ();

			// Default colors
			ChannelColors.Add (0, ConsoleColor.White); // Default color
			ChannelColors.Add (1, ConsoleColor.Green);
			ChannelColors.Add (2, ConsoleColor.Blue);
			ChannelColors.Add (3, ConsoleColor.Magenta);
			ChannelColors.Add (4, ConsoleColor.Yellow);

			DebugLines = new List<FDebugLine> ();
		}

		/// <summary>
		/// Write some text to the console in color.
		/// </summary>
		public static void WriteInColor(string text, ConsoleColor color)
		{
			Console.ForegroundColor = color;
			Console.Write (text);
			Console.ForegroundColor = ConsoleColor.White;
		}

		/// <summary>
		/// Write some text to the console with a channel and a log level.
		/// </summary>
		/// <param name="text"></param>
		/// <param name="channel"></param>
		/// <param name="logLevel"></param>
		public static void Write(string text, int channel = 0, FLogLevel logLevel = FLogLevel.INFO)
		{
			switch (logLevel)
			{
				case FLogLevel.INFO:
					WriteInColor (text, ChannelColors [channel]);
					break;
				case FLogLevel.WARNING:
					WriteInColor (text, ChannelColors [channel]);
					break;
				case FLogLevel.ERROR:
					WriteInColor (text, ConsoleColor.DarkRed);
					break;
			}

			DebugLines.Add (new FDebugLine () { Channel = channel, Text = text, LogLevel = logLevel });

			primitives = new Queue<object> ();
		}

		/// <summary>
		/// Write some text with a log level and a new line terminator.
		/// </summary>
		/// <param name="text"></param>
		/// <param name="channel"></param>
		/// <param name="logLevel"></param>
		public static void WriteLine(string text, int channel = 0, FLogLevel logLevel = FLogLevel.INFO)
		{
			Write (text + "\n", channel, logLevel);
		}

		/// <summary>
		/// Write an integer with a log level and a new line terminator.
		/// </summary>
		/// <param name="text"></param>
		/// <param name="channel"></param>
		/// <param name="logLevel"></param>
		public static void WriteLine(int text, int channel = 0, FLogLevel logLevel = FLogLevel.INFO)
		{
			Write (text.ToString() + "\n", channel, logLevel);
		}

		/// <summary>
		/// Write everything printed to the console to a file.
		/// </summary>
		public static void WriteLogFile(string directory = "Logs/")
		{
			// Create filename with date and time
			string path = $"{directory}/log-{DateTime.Now.ToString ("dd-MM-yy")}_{DateTime.Now.ToString ("hh-mm-ss")}.txt";
			WriteLine ($"Saving log file to {path}");

			// Make sure the directory exists
			Directory.CreateDirectory (directory);
			
			// Open the file for writing
			FileStream logFile = File.Create (path);

			// Write each line from the debug output
			foreach (FDebugLine line in DebugLines)
			{
				string tempLine = $"[{line.Channel}] [{line.LogLevel}] {line.Text}";
				logFile.Write(Encoding.UTF8.GetBytes(tempLine), 0, Encoding.UTF8.GetByteCount(tempLine));
			}

			// Don't forget to close the file!
			logFile.Close ();
		}

		/// <summary>
		/// Draw all debug primitives from the queue to the screen.
		/// </summary>
		/// <param name="target"></param>
		/// <param name="state"></param>
		public static void DrawOverlay(RenderTarget target, RenderStates state)
		{
			// Create empty shapes to reuse
			CircleShape circle = new CircleShape ();
			RectangleShape rect = new RectangleShape ();

			// Draw each primitive
			foreach (object primitive in primitives)
			{
				// Default properties
				circle.Radius = 3;
				circle.OutlineColor = Color.Black;
				circle.OutlineThickness = 2;

				rect.OutlineColor = Color.Black;
				rect.OutlineThickness = 2;

				// Match the type of the primitive
				switch (primitive)
				{
					// point?
					case Vector2f vector2f:
						// Shift point so it's centered for maximum accuracy
						circle.Position = vector2f - new Vector2f (circle.Radius, circle.Radius);
						circle.FillColor = new Color (55, 201, 2, 128); // Green
						target.Draw (circle);
						break;
					case Vector2i vector2i:
						// Shift point so it's centered for maximum accuracy
						circle.Position = vector2i.To2f() - new Vector2f(circle.Radius, circle.Radius);
						circle.FillColor = new Color (55, 19, 222, 128); // Blue
						target.Draw (circle);
						break;
					// rect?
					case FloatRect floatRect:
						rect.Position = floatRect.Position ();
						rect.Size = floatRect.Size ();
						rect.FillColor = new Color(219, 39, 1, 128); // Red
						target.Draw (rect);
						break;
				}
			}

			primitives.Clear ();
		}

		/// <summary>
		/// Debug a primitive.
		/// 
		/// Currently supported:
		/// - Vector2(f/i)
		/// - FloatRect
		/// </summary>
		/// <param name="primitive">The primitive to debug. For types, see above.</param>
		public static void Primitive(object primitive)
		{
			primitives.Enqueue (primitive);
		}
	}
}
