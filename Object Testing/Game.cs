using GETIMFISP;
using Glide;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledSharp;

namespace SFMLGame
{
	class MyGame
	{
		public static FGame game;

		static void Main(string [] args)
		{
			// Make a new game from a Tiled file
			game = new FGame ("Data/main.tmx");
			// Register Scripts
			game.AddActorType<Player> ();
			// Set window info
			game.windowSettings = FWindowSettings.SIMPLE_WINDOWED;
			// Background Color
			game.backgroundColor = Color.Black;
			// Start the game
			game.Run ();

			// Keeps the console window open after you close the game.
			Console.ReadLine ();
		}
	}

	/// <summary>
	/// Testing class.
	/// </summary>
	public class Player : FActor
	{
		public override void OnGraphicsReady()
		{
			base.OnGraphicsReady ();

			// Load an animation of ABC
			graphics.AddAnimation ("Animation1", new Texture [] { new Texture("Data/images/object1.png"), new Texture ("Data/images/object2.png"), new Texture ("Data/images/object3.png"), });
			graphics.PlayAnimation ("Animation1");

			Game.tweener.Tween (this, new { X = 20 }, 1);
		}

		public override void Draw(RenderTarget target, RenderStates states)
		{
			base.Draw (target, states);
		}
	}
}
