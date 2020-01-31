using System;
using System.Collections.Generic;
using SFML.Graphics;
using SFML.Window;
using System.Collections;
using SFML.System;

namespace GETIMFISP.Extensions
{
	public static class RectExtensions
	{
		public static float Bottom(this FloatRect rec)
		{
			return rec.Top + rec.Height;
		}

		public static float Right(this FloatRect rec)
		{
			return rec.Left + rec.Width;
		}

		public static Vector2f Center(this FloatRect rec)
		{
			return new Vector2f (rec.Left + rec.Width / 2, rec.Top + rec.Height / 2);
		}

		public static FloatRect Abs(this FloatRect rec)
		{
			if (rec.Width < 0)
			{
				rec.Left += rec.Width;
				rec.Width *= -1;
			}
			if (rec.Height < 0)
			{
				rec.Top += rec.Height;
				rec.Height *= -1;
			}
			return rec;
		}

		public static bool Intersects(this FloatRect rectA, FloatRect rectB, out Vector2f depth)
		{
			//source: XNA Platformer Example

			// Calculate half sizes.
			float halfWidthA = rectA.Width / 2.0f;
			float halfHeightA = rectA.Height / 2.0f;
			float halfWidthB = rectB.Width / 2.0f;
			float halfHeightB = rectB.Height / 2.0f;

			// Calculate centers.
			var centerA = new Vector2f (rectA.Left + halfWidthA, rectA.Top + halfHeightA);
			var centerB = new Vector2f (rectB.Left + halfWidthB, rectB.Top + halfHeightB);

			// Calculate current and minimum-non-intersecting distances between centers.
			float distanceX = centerA.X - centerB.X;
			float distanceY = centerA.Y - centerB.Y;
			float minDistanceX = halfWidthA + halfWidthB;
			float minDistanceY = halfHeightA + halfHeightB;

			// If we are not intersecting at all, return (0, 0).
			if (Math.Abs (distanceX) >= minDistanceX || Math.Abs (distanceY) >= minDistanceY)
			{
				depth = new Vector2f ();
				return false;
			}

			// Calculate and return intersection depths.
			float depthX = distanceX > 0 ? minDistanceX - distanceX : -minDistanceX - distanceX;
			float depthY = distanceY > 0 ? minDistanceY - distanceY : -minDistanceY - distanceY;

			depth = new Vector2f (depthX, depthY);
			return true;
		}

		public static bool Contains(this FloatRect a, FloatRect b)
		{
			return
				b.Left > a.Left &&
				b.Right () < a.Right () &&
				b.Top > a.Top &&
				b.Bottom () < a.Bottom ();
		}

		public static FloatRect AABB(IEnumerable<Vector2f> vectors)
		{
			float minX, minY, maxX, maxY;
			minX = minY = float.MaxValue;
			maxX = maxY = float.MinValue;

			foreach (var v in vectors)
			{
				minX = Math.Min (minX, v.X);
				maxX = Math.Max (maxX, v.X);

				minY = Math.Min (minY, v.Y);
				maxY = Math.Max (maxY, v.Y);
			}

			return new FloatRect (minX, minY, maxX - minX, maxY - minY);
		}

		/// <summary>
		/// Computes how much area of recA is covered by recB (as percentage).
		/// </summary>
		public static float OverlapRatio(this FloatRect recA, FloatRect recB)
		{
			FloatRect overlap;
			if (recB.Intersects (recA, out overlap))
			{
				return Math.Min (1, overlap.Width * overlap.Height / recA.Width / recA.Height);
			}
			return 0;
		}

		public static bool Contains(this FloatRect rec, Vector2f v)
		{
			return rec.Contains (v.X, v.Y);
		}

		public static Vector2f Position(this FloatRect rec)
		{
			return new Vector2f (rec.Left, rec.Top);
		}

		public static Vector2f Size(this FloatRect rec)
		{
			return new Vector2f (rec.Width, rec.Height);
		}

		public static FloatRect Grow(this FloatRect rec, float margin)
		{
			rec.Left -= margin;
			rec.Top -= margin;
			rec.Width += margin * 2;
			rec.Height += margin * 2;

			return rec;
		}

	}
}