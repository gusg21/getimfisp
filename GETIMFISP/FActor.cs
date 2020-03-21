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
		/// <summary>
		/// The unique ID of the actor to its FActorManager (see Manager)
		/// </summary>
		public int ID { get; set; }
		/// <summary>
		/// The name (usually from Tiled) of the actor
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// The FActorManager this is a child of
		/// </summary>
		public FActorManager Manager;
		/// <summary>
		/// The Game that the Manager is a child of (same as Manager.Game)
		/// </summary>
		public FGame Game { get { return Manager.Game; } }

		// The render order
		private int depth = 0;
		/// <summary>
		/// The order in which to render objects. Negative = behind
		/// </summary>
		public int Depth { get { return depth; } set { depth = value; Manager.SortByDepth (); } }

		/// <summary>
		/// Use the built-in motion system?
		/// </summary>
		public bool doBuiltInMotion = true;
		/// <summary>
		/// Built-in velocity. Only used if useBuiltInMotion = true
		/// </summary>
		public Vector2f Velocity = new Vector2f();
		/// <summary>
		/// Built-in acceleration. Only used if useBuiltInMotion = true
		/// </summary>
		public Vector2f Acceleration = new Vector2f ();

		/// <summary>
		/// The graphics of the sprite.
		/// </summary>
		public FSprite Graphics = new FSprite();
		/// <summary>
		/// Use the built-in renderer?
		/// </summary>
		public bool UseBuiltinRenderer { protected set; get; } = true;
		/// <summary>
		/// Is the sprite visible?
		/// </summary>
		public bool Visible { get { return Graphics.Visible; } set { Graphics.Visible = value; } }

		/// <summary>
		/// The collision (bounding) box of the actor
		/// </summary>
		public FloatRect BBox = new FloatRect ();
		/// <summary>
		/// Use the graphics as basis for the bbox.
		/// NOTE: position is still set automatically, but you can edit width + height.
		/// </summary>
		public bool CalcBBoxOffGraphics = true;

		/// <summary>
		/// The Tiled object this was created from.
		/// </summary>
		public TmxObject SrcObject;

		/// <summary>
		/// Called when this object is clicked
		/// </summary>
		public event EventHandler<MouseButtonEventArgs> Clicked;
		private void ClickCheck(object sender, MouseButtonEventArgs e)
		{
			Vector2f converted = Game.Window.MapPixelToCoords (new Vector2i (e.X, e.Y));
			if (BBox.Contains(converted))
			{
				Clicked?.Invoke (this, e);
			}
		}

		/// <summary>
		/// Is the mouse currently over the bbox?
		/// </summary>
		public bool MouseOver = false;

		/// <summary>
		/// Called when the mouse enters the bbox
		/// </summary>
		public event EventHandler<MouseMoveEventArgs> MouseEntered;
		private void MouseEnteredCheck(object sender, MouseMoveEventArgs e)
		{
			Vector2f converted = Game.Window.MapPixelToCoords (new Vector2i (e.X, e.Y));
			if (BBox.Contains (converted))
			{
				MouseOver = true;
				MouseEntered?.Invoke (this, e);
			}
		}

		/// <summary>
		/// Called when the mouse leaves the bbox
		/// </summary>
		public event EventHandler<MouseMoveEventArgs> MouseLeft;
		private void MouseLeftCheck(object sender, MouseMoveEventArgs e)
		{
			Vector2f converted = Game.Window.MapPixelToCoords (new Vector2i (e.X, e.Y));
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

			Game.Window.MouseButtonReleased += ClickCheck;
			Game.Window.MouseMoved += MouseEnteredCheck;

			FixName ();
		}

		/// <summary>
		/// Update the bbox. If no graphics the box will be zeroed, and if CalcBBoxOffGraphics is false the box will only contain position and the use has to define the size. Accounts for Graphics.Scale.
		/// </summary>
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