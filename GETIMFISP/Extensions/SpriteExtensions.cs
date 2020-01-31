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
		public static void X(this Sprite sprite, float value)
		{
			sprite.Position = new Vector2f (value, sprite.Position.Y);
		}

		public static void Y(this Sprite sprite, float value)
		{
			sprite.Position = new Vector2f (sprite.Position.X, value);
		}

		public static void dX(this Sprite sprite, float value)
		{
			sprite.Position = new Vector2f (sprite.Position.X + value, sprite.Position.Y);
		}

		public static void dY(this Sprite sprite, float value)
		{
			sprite.Position = new Vector2f (sprite.Position.X, sprite.Position.Y + value);
		}

		public static Vector2f Center(this Sprite sprite)
		{
			return sprite.Position - (sprite.Texture.Size / 2).To2f();
		}
	}
}
