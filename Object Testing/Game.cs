using GETIMFISP;
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
			game.AddActorType<Controller> ();
			// Set window info
			game.windowInfo = FWindowInfo.SIMPLE_WINDOWED;
			// Background Color
			game.backgroundColor = Color.Black;
			game.Run (); 

			Console.ReadLine ();
		}
	}

	public class Player : FActor
	{
		public override void OnGraphicsReady()
		{
			graphics = new Sprite (new Texture($"Data/images/object{srcObject.Properties["number"]}.png"));

			velocity = new Vector2f (20, 20);
		}
	}

	public class Controller : FActor
	{
		public override void Update(Time delta)
		{
			base.Update (delta);

			if (Game.Elapsed.AsSeconds() > 2)
			{
				Manager.RemoveAll<Player> ();
			}
		}
	}
}
