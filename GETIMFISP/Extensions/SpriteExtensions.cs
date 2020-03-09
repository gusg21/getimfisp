using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GETIMFISP.Extensions
{
	public static class SpriteExtensions
	{
		public static Vector2f Center(this Sprite sprite)
		{
			return sprite.Position - (sprite.Texture.Size / 2).To2f();
		}
	}
}
