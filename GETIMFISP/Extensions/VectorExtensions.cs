using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFML.Graphics;
using SFML.Window;
using SFML.System;
//using Microsoft.Xna.Framework;

namespace GETIMFISP.Extensions
{
	public static class VectorExtensions
	{
		public static float LengthSquared(this Vector2f v)
		{
			return v.X * v.X + v.Y * v.Y;
		}

		public static float Length(this Vector2f v)
		{
			return (float) Math.Sqrt (v.LengthSquared ());
		}

		public static Vector2f Dir(this Vector2f v)
		{
			var length = v.Length ();
			if (length == 0)
				return new Vector2f ();
			return v / length;
		}

		public static float Angle(this Vector2f v, bool radians = false)
		{
			var value = (float) Math.Atan2 (v.Y, v.X);
			if (!radians)
				value *= 180 / (float) Math.PI;
			return value;
		}

		public static Vector2f Rotate(this Vector2f v, Vector2f dir)
		{
			//return new Vector2f(v.X*dir.X - v.Y*dir.Y, v.X*dir.Y+v.X*dir.X);
			return new Vector2f (v.X * dir.X + v.Y * dir.Y, v.X * dir.Y - v.Y * dir.X);
		}

		public static float Dot(this Vector2f t, Vector2f v)
		{
			return t.X * v.X + t.Y * v.Y;
		}

		public static Vector2f Mul(this Vector2f a, Vector2f b)
		{
			return new Vector2f (a.X * b.X, a.Y * b.Y);
		}

		public static Vector2f Project(this Vector2f v, Vector2f axis)
		{
			return v.Dot (axis) / axis.LengthSquared () * axis;
		}

		public static Vector2f Rotate(this Vector2f v, float angle, bool radians = false)
		{
			if (!radians)
				angle = FloatMath.ToRadians (angle);
			float sin, cos;
			sin = (float) Math.Sin (angle);
			cos = (float) Math.Cos (angle);
			/*
            if (!radians)
                angle = angle / 180 * (float)Math.PI;
            var sin = (float)Math.Sin(angle);
            var cos = (float)Math.Cos(angle);
             */
			return new Vector2f (v.X * cos - v.Y * sin, v.X * sin + v.Y * cos);
		}

		public static Vector2f Lerp(this Vector2f a, Vector2f b, float ratio)
		{
			a.X = FloatMath.Lerp (a.X, b.X, ratio);
			a.Y = FloatMath.Lerp (a.Y, b.Y, ratio);
			return a;
		}

		public static bool Cmp(this Vector2f a, Vector2f b)
		{
			return a.X == b.X && a.Y == b.Y;
		}

		public static Vector2f To2f(this Vector2i v)
		{
			return new Vector2f (v.X, v.Y);
		}

		public static Vector2f To2f(this Vector2u v)
		{
			return new Vector2f (v.X, v.Y);
		}

		public static Vector2i To2i(this Vector2f v)
		{
			return new Vector2i ((int) v.X, (int) v.Y);
		}

		public static Vector2i To2i(this Vector2u v)
		{
			return new Vector2i ((int) v.X, (int) v.Y);
		}

		public static Vector2u To2u(this Vector2f v)
		{
			return new Vector2u ((uint) v.X, (uint) v.Y);
		}

		public static Vector2u To2u(this Vector2i v)
		{
			return new Vector2u ((uint) v.X, (uint) v.Y);
		}

		//public static Vector2 ToXna(this Vector2f v)
		//{
		//	return new Vector2 (v.X, v.Y);
		//}

		//public static Vector2 ToXna(this Vector2u v)
		//{
		//	return new Vector2 (v.X, v.Y);
		//}

		/// <summary>
		/// Calculates square with given vector in the center and size of margin*2
		/// </summary>
		public static FloatRect GetGlobalBounds(this Vector2f v, float margin)
		{
			return new FloatRect (v.X - margin, v.Y - margin, margin * 2, margin * 2);
		}

		public static FloatRect GetGlobalBounds(this Vector2f v, Vector2f size)
		{
			return v.GetGlobalBounds (size.X, size.Y);
		}

		public static FloatRect GetGlobalBounds(this Vector2f v, float width, float height)
		{
			return new FloatRect (v.X - width / 2, v.Y - height / 2, width, height);
		}

		public static Vector2f Abs(this Vector2f v)
		{
			return new Vector2f (Math.Abs (v.X), Math.Abs (v.Y));
		}

		public static float Cross(this Vector2f a, Vector2f b)
		{
			return a.X * b.Y - a.Y * b.X;
		}

		public static Vector2f Cross(this Vector2f a, float v)
		{
			return -new Vector2f (-v * a.Y, v * a.X);
		}

		public static Vector2f Perp(this Vector2f a)
		{
			return new Vector2f (-a.Y, a.X);
		}

		public static double Dist(this Vector2f a, Vector2f b)
		{
			return Math.Sqrt (Math.Pow ((a.X - b.X), 2) + Math.Pow ((a.Y - b.Y), 2));
		}

		public static Vector2f Norm(this Vector2f a)
		{
			if (a.Length () == 0)
				return new Vector2f ();
			return new Vector2f (a.X / a.Length (), a.Y / a.Length ());
		}
	}
}
