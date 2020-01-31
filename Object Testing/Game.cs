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
	class Game
	{
		static void Main(string [] args)
		{
			// Make a new game from a Tiled file
			FGame game = new FGame ("Data/main.tmx");
			// Register Player Script
			game.AddActorType<Player> ();
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
		public override void OnCreated(TmxObject obj)
		{
			Console.WriteLine ($"Player position: X {obj.X} Y {obj.Y}");

			graphics = new Sprite (new Texture($"Data/images/object{obj.Properties["number"]}.png"));
			graphics.Position = new Vector2f ((float) obj.X, (float) obj.Y);

			velocity = new Vector2f (20, 20);
		}
	}
}
