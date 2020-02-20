using SFML.Graphics;
using SFML.System;
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

		// Built-in motion
		public bool doBuiltInMotion = true;
		public Vector2f velocity = new Vector2f();
		public Vector2f acceleration = new Vector2f ();

		// The graphics of the sprite.
		public FSprite graphics = new FSprite();

		// The tiled object this object is based off
		public TmxObject srcObject = null;

		/// <summary>
		/// Run when the tiled map loader has finished setting up the `graphics` variable and the `srcObject` variable
		/// 
		/// Only useful in scripts that come from the Tile source type.
		/// </summary>
		public virtual void OnGraphicsReady() { }

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
		/// Update the actor
		/// </summary>
		/// <param name="delta">time since last frame</param>
		public virtual void Update(FGameTime delta)
		{
			if (doBuiltInMotion)
				DoMotion (delta);

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
			target.Draw (graphics, states);
		}
	}
}