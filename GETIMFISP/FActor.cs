using System;
using GETIMFISP.Extensions;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using TiledSharp;

namespace GETIMFISP
{
	/// <summary>
	///	An actor in game. Has update + draw functionality.
	/// </summary>
	public class FActor : Drawable
	{
		// The id of the actor
		public int ID { get; set; }
		// The name of the actor
		public string Name { get; set; }
		// The manager of the actor
		public FActorManager Manager;
		// The game of the manager (for convenience)
		public FGame Game { get { return Manager.Game; } }

		// Current position (convenience for graphics.Position)
		public Vector2f Position { get { return graphics.Position; } set { graphics.Position = value; } }
		// Individual components of position
		public float X { get { return graphics.Position.X; } set { graphics.Position = new Vector2f (value, graphics.Position.Y); } }
		public float Y { get { return graphics.Position.Y; } set { graphics.Position = new Vector2f (graphics.Position.X, value); } }
		public float RotationDegrees { get { return graphics.Rotation; } set { graphics.Rotation = value; } }
		public float RotationRadians { get { return graphics.Rotation / 57.295779513f; } set { graphics.Rotation = value * 57.295779513f; } }

		// The render order
		private int depth = 0;
		public int Depth { get { return depth; } set { depth = value; Manager.SortByDepth (); } }

		// Built-in motion
		public bool doBuiltInMotion = true;
		public Vector2f velocity = new Vector2f();
		public Vector2f acceleration = new Vector2f ();

		// The graphics of the sprite.
		public FSprite graphics = FSprite.NULL_SPRITE;
		// Use the built-in renderer?
		public bool UseBuiltinRenderer { protected set; get; } = true;
		// Visible?
		public bool Visible { get; protected set; } = true;

		// The 2d rectangle that represents the collider for the actor.
		public FloatRect bbox = new FloatRect ();

		// The tiled object this object is based off
		public TmxObject srcObject;

		// Click Event
		public event EventHandler<MouseButtonEventArgs> Clicked;
		private void ClickCheck(object sender, MouseButtonEventArgs e)
		{
			Vector2f converted = Game.window.MapPixelToCoords (new Vector2i (e.X, e.Y));
			if (bbox.Contains(converted))
			{
				Clicked?.Invoke (this, e);
			}
		}

		// Mouse Over events
		public bool mouseOver = false;

		public event EventHandler<MouseMoveEventArgs> MouseEntered;
		private void MouseEnteredCheck(object sender, MouseMoveEventArgs e)
		{
			Vector2f converted = Game.window.MapPixelToCoords (new Vector2i (e.X, e.Y));
			if (bbox.Contains (converted))
			{
				mouseOver = true;
				MouseEntered?.Invoke (this, e);
			}
		}

		public event EventHandler<MouseMoveEventArgs> MouseLeft;
		private void MouseLeftCheck(object sender, MouseMoveEventArgs e)
		{
			Vector2f converted = Game.window.MapPixelToCoords (new Vector2i (e.X, e.Y));
			if (bbox.Contains (converted))
			{
				mouseOver = false;
				MouseLeft?.Invoke (this, e);
			}
		}

		/// <summary>
		/// Run when the tiled map loader has finished setting up the `graphics` variable and the `srcObject` variable
		/// 
		/// Only useful in scripts that come from the Tile source type.
		/// </summary>
		public virtual void OnGraphicsReady()
		{
			UpdateBBox ();

			Game.window.MouseButtonReleased += ClickCheck;
			Game.window.MouseMoved += MouseEnteredCheck;

			FixName ();
		}

		public void UpdateBBox()
		{
			if (graphics.Texture != null)
			{
				bbox = new FloatRect (Position, graphics.Texture.Size.To2f().Mul(graphics.Scale));
			}
			else
			{
				bbox = new FloatRect ();
			}
		}

		/// <summary>
		/// Handle updating of position and velocity
		/// </summary>
		/// <param name="delta">time since last frame</param>
		public virtual void DoMotion(FGameTime delta)
		{
			velocity += acceleration * delta.AsSeconds ();
			Position += velocity * delta.AsSeconds ();
		}

		/// <summary>
		/// Fix the Name prop. if unset
		/// </summary>
		void FixName()
		{
			if (Name == "")
				Name = $"Object ({srcObject.ObjectType}) @ {X}, {Y}";
		}

		/// <summary>
		/// Update the actor
		/// </summary>
		/// <param name="delta">time since last frame</param>
		public virtual void Update(FGameTime delta)
		{
			FixName ();

			if (doBuiltInMotion)
				DoMotion (delta);

			UpdateBBox ();

			// Update graphics
			graphics.Update (delta);
		}

		/// <summary>
		/// Draw the actor on the surface
		/// </summary>
		/// <param name="target">the surface to draw to</param>
		/// <param name="states">the renderer state</param>
		public virtual void Draw(RenderTarget target, RenderStates states)
		{
			if (UseBuiltinRenderer && Visible)
				target.Draw (graphics, states);
		}
	}
}