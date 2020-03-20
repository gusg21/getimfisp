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
		
		public float RotationDegrees { get { return Graphics.Rotation; } set { Graphics.Rotation = value; } }
		public float RotationRadians { get { return Graphics.Rotation / 57.295779513f; } set { Graphics.Rotation = value * 57.295779513f; } }

		// The render order
		private int depth = 0;
		public int Depth { get { return depth; } set { depth = value; Manager.SortByDepth (); } }

		// Built-in motion
		public bool doBuiltInMotion = true;
		public Vector2f Velocity = new Vector2f();
		public Vector2f Acceleration = new Vector2f ();

		// The graphics of the sprite.
		public FSprite Graphics = FSprite.NULL_SPRITE;
		// Use the built-in renderer?
		public bool UseBuiltinRenderer { protected set; get; } = true;
		// Visible?
		public bool Visible { get { return Graphics.Visible; } set { Graphics.Visible = value; } }

		// The 2d rectangle that represents the collider for the actor.
		public FloatRect BBox = new FloatRect ();
		public bool CalcBBoxOffGraphics = true;

		// The tiled object this object is based off
		public TmxObject SrcObject;

		// Click Event
		public event EventHandler<MouseButtonEventArgs> Clicked;
		private void ClickCheck(object sender, MouseButtonEventArgs e)
		{
			Vector2f converted = Game.window.MapPixelToCoords (new Vector2i (e.X, e.Y));
			if (BBox.Contains(converted))
			{
				Clicked?.Invoke (this, e);
			}
		}

		// Mouse Over events
		public bool MouseOver = false;

		public event EventHandler<MouseMoveEventArgs> MouseEntered;
		private void MouseEnteredCheck(object sender, MouseMoveEventArgs e)
		{
			Vector2f converted = Game.window.MapPixelToCoords (new Vector2i (e.X, e.Y));
			if (BBox.Contains (converted))
			{
				MouseOver = true;
				MouseEntered?.Invoke (this, e);
			}
		}

		public event EventHandler<MouseMoveEventArgs> MouseLeft;
		private void MouseLeftCheck(object sender, MouseMoveEventArgs e)
		{
			Vector2f converted = Game.window.MapPixelToCoords (new Vector2i (e.X, e.Y));
			if (BBox.Contains (converted))
			{
				MouseOver = false;
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
			if (Graphics.Texture != null)
			{
				if (CalcBBoxOffGraphics)
				{
					BBox = new FloatRect (Graphics.Position, Graphics.Texture.Size.To2f().Mul(Graphics.Scale));
				}
				else
				{
					BBox.Left = Graphics.Position.X;
					BBox.Top = Graphics.Position.Y;
				}
			}
			else
			{
				BBox = new FloatRect ();
			}
		}

		/// <summary>
		/// Handle updating of position and velocity
		/// </summary>
		/// <param name="delta">time since last frame</param>
		public virtual void DoMotion(FGameTime delta)
		{
			Velocity += Acceleration * delta.AsSeconds ();
			Graphics.Position += Velocity * delta.AsSeconds ();
		}

		/// <summary>
		/// Fix the Name prop. if unset
		/// </summary>
		void FixName()
		{
			if (Name == "")
				Name = $"Object ({SrcObject.ObjectType}) @ {Graphics.Position.X}, {Graphics.Position.Y}";
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
			Graphics.Update (delta);
		}

		/// <summary>
		/// Draw the actor on the surface
		/// </summary>
		/// <param name="target">the surface to draw to</param>
		/// <param name="states">the renderer state</param>
		public virtual void Draw(RenderTarget target, RenderStates states)
		{
			if (UseBuiltinRenderer && Visible)
			{
				target.Draw (Graphics, states);
			}
		}
	}
}