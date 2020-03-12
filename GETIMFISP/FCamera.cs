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
	public class FCamera
	{
		public Vector2f rawPosition;
		public Vector2f targetPosition;
		public float rotation;
		public Vector2f windowSize;
		public float zoom;

		public bool smooth = true;
		public float snappiness = 3f;
		public float ignoreDist = 1f;
		public bool silly = false;

		public Vector2f Size { get { return windowSize * (1/zoom); } }

		public FCamera(Vector2f windowSize)
		{
			rawPosition = new Vector2f ();
			targetPosition = rawPosition;
			rotation = 0f;
			this.windowSize = windowSize;
			zoom = 1f;
		}

		public void Resize(uint width, uint height)
		{
			windowSize = new Vector2f(width, height);
		}

		public View GetView()
		{
			View view = new View ();

			view.Center = rawPosition;
			view.Rotation = rotation;
			view.Size = Size;

			return view;
		}

		public void Goto(Vector2f position)
		{
			targetPosition = position;
			rawPosition = position;
		}

		public void Target(Vector2f position)
		{
			targetPosition = position;
		}

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
