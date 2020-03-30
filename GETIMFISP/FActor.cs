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
		public string Name;
		/// <summary>
		/// Whether the object will stay when the scene changes. An example would be a manager that controls player data across levels.
		/// </summary>
		public bool Persistent = false;
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
		/// Fix the Name property if anonymous.
		/// </summary>
		void FixName()
		{
			if (Name == "")
				Name = $"Anonymous Object ({SrcObject.ObjectType})";
		}


		/// <summary>
		/// Run when the tiled map loader has finished setting up the `graphics` variable and the `srcObject` variable
		/// 
		/// Only useful in scripts that come from the Tile source type.
		/// </summary>
		public virtual void OnGraphicsReady()
		{
			// Adjust the bounding box to the actor's graphics size
			UpdateBBox ();

			// Register hover events
			Game.Window.MouseButtonReleased += ClickCheck;
			Game.Window.MouseMoved += MouseEnteredCheck;

			// If the name is anonymous, replace it with something at least a little more helpful
			FixName ();
		}

		/// <summary>
		/// Run when this object is removed from its manager. Use this to clean up events or your actor will crash if it needs to access the Game property (this is null when removed)
		/// </summary>
		public virtual void OnRemoved()
		{
			// Remove hover events
			Game.Window.MouseButtonReleased -= ClickCheck;
			Game.Window.MouseMoved -= MouseEnteredCheck;
		}

		/// <summary>
		/// Update the actor
		/// </summary>
		/// <param name="delta">time since last frame</param>
		public virtual void Update(FGameTime delta)
		{
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