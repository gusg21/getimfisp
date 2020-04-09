using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GETIMFISP.Extensions
{
	static class ConsoleColorExtensions
	{
		static Dictionary<ConsoleColor, Color> colorMap;

		static ConsoleColorExtensions()
		{
			colorMap = new Dictionary<ConsoleColor, Color> ();

			colorMap.Add (ConsoleColor.Black, Color.Black);
			colorMap.Add (ConsoleColor.Blue, Color.Blue);
			colorMap.Add (ConsoleColor.Cyan, Color.Cyan);
			colorMap.Add (ConsoleColor.DarkBlue, new Color (0, 2, 145));
			colorMap.Add (ConsoleColor.DarkCyan, new Color (0, 81, 99));
			colorMap.Add (ConsoleColor.DarkGray, new Color (92, 92, 92));
			colorMap.Add (ConsoleColor.DarkGreen, new Color (0, 102, 12));
			colorMap.Add (ConsoleColor.DarkMagenta, new Color (102, 0, 100));
			colorMap.Add (ConsoleColor.DarkRed, new Color (119, 0, 0));
			colorMap.Add (ConsoleColor.DarkYellow, new Color (125, 119, 0));
			colorMap.Add (ConsoleColor.Gray, new Color (176, 176, 176));
			colorMap.Add (ConsoleColor.Green, Color.Green);
			colorMap.Add (ConsoleColor.Magenta, Color.Magenta);
			colorMap.Add (ConsoleColor.Red, Color.Red);
			colorMap.Add (ConsoleColor.White, Color.White);
			colorMap.Add (ConsoleColor.Yellow, Color.Yellow);
		}

		public static Color ToSfmlColor(this ConsoleColor color)
		{
			return colorMap [color];
		}
	}
}
