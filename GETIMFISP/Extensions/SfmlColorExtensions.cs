using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GETIMFISP.Extensions
{
	static class SfmlColorExtensions
	{
		public static Color Alpha(this Color color, byte a)
		{
			return new Color (color.R, color.G, color.B, a);
		}
	}
}
