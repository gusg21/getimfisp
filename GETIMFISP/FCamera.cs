using GETIMFISP.Extensions;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GETIMFISP
{
	/// <summary>
	/// A wrapper around a View
	/// </summary>
	public class FCamera
	{
		/// <summary>
		/// The actual position of the camera
		/// </summary>
		public Vector2f rawPosition;
		/// <summary>
		/// The position the camera wants to be
		/// </summary>
		public Vector2f targetPosition;
		/// <summary>
		/// The rotation of the camera (around the Y axis)
		/// </summary>
		public float rotation;
		/// <summary>
		/// The size of the window
		/// </summary>
		public Vector2u windowSize;
		/// <summary>
		/// The amount of zoom (2 = pixels 2x larger)
		/// </summary>
		public float zoom;

		/// <summary>
		/// Use lerping?
		/// </summary>
		public bool smooth = true;
		/// <summary>
		/// How fast to lerp, higher being "snappier"
		/// </summary>
		public float snappiness = 3f;
		/// <summary>
		/// The distance at which to stop going closer
		/// </summary>
		public float ignoreDist = 1f;
		/// <summary>
		/// Wacky mode
		/// </summary>
		public bool silly = false;

		/// <summary>
		/// The visible area of the camera (calculated with zoom)
		/// </summary>
		public Vector2f Size { get { return windowSize.To2f() * (1/zoom); } }

		/// <summary>
		/// Create the camera
		/// </summary>
		/// <param name="game">The FGame to derive the window size from</param>
		public FCamera(FGame game)
		{
			rawPosition = new Vector2f ();
			targetPosition = rawPosition;
			rotation = 0f;
			windowSize = game.Window.Size;
			zoom = 1f;

			game.Window.Resized += (sender, e) => { windowSize = new Vector2u (e.Width, e.Height); };
		}

		/// <summary>
		/// Calculate the view represented by the camera
		/// </summary>
		/// <returns>the view of the camera</returns>
		public View GetView()
		{
			View view = new View ();

			view.Center = rawPosition;
			view.Rotation = rotation;
			view.Size = Size;

			return view;
		}

		/// <summary>
		/// Go directly to a position, ignoring smoothing
		/// </summary>
		/// <param name="position"></param>
		public void Goto(Vector2f position)
		{
			targetPosition = position;
			rawPosition = position;
		}

		/// <summary>
		/// Target a position. Will get there eventually
		/// </summary>
		/// <param name="position"></param>
		public void Target(Vector2f position)
		{
			targetPosition = position;
		}

		/// <summary>
		/// Update the camera, calculating lerping and stuff like that
		/// </summary>
		/// <param name="gameTime"></param>
		public void Update(FGameTime gameTime)
		{
			float currentDist = (float) rawPosition.Dist (targetPosition);
			if (smooth && currentDist > ignoreDist)
			{
				rawPosition = rawPosition.Lerp (targetPosition, snappiness * gameTime.AsSeconds());
				if (silly)
				{
					rotation = currentDist / 20.0f;
					if (rotation < 0.1f)
					{
						rotation = 0f;
					}
				}
			}
		}
	}
}
