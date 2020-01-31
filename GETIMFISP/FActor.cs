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
		public int ID { get; set; }

		public Vector2f Position { get { return graphics.Position; } set { graphics.Position = value; } }
		public float X { get { return graphics.Position.X; } set { graphics.Position = new Vector2f (value, graphics.Position.Y); } }
		public float Y { get { return graphics.Position.Y; } set { graphics.Position = new Vector2f (graphics.Position.X, value); } }
		public Vector2f velocity = new Vector2f();

		public Sprite graphics = new Sprite();

		public virtual void OnCreated(TmxObject obj) { }
		public virtual void Update(Time delta)
		{
			Position += velocity * delta.AsSeconds();
		}
		public virtual void Draw(RenderTarget target, RenderStates states)
		{
			target.Draw (graphics, states);
		}
	}
}