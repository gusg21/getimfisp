using GETIMFISP;
using Glide;
using IronWren;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.IO;

namespace SFMLGame
{
	class MyGame
	{
		public static FGame game;

		static void Main(string [] args)
		{
			// Make a new game with a simple window mode setting
			game = new FGame (FWindowSettings.SIMPLE_WINDOWED);
			// Register Scripts
			game.AddActorType<Player> ();
			// Load the tiled map
			game.ChangeScene ("Data/main.tmx");
			// Start the game
			game.Run ();

			FDebug.WriteLogFile ();

			// Keeps the console window open after you close the game.
			Console.ReadLine ();
		}
	}

	/// <summary>
	/// Testing class.
	/// </summary>
	public class Player : FActor
	{
		Tween tween;

		public override void OnGraphicsReady()
		{
			base.OnGraphicsReady ();

			// Load an animation of ABC
			Graphics.AddAnimation ("Animation1", new Texture [] { new Texture("Data/images/object1.png"), new Texture ("Data/images/object2.png"), new Texture ("Data/images/object3.png"), });
			Graphics.SwitchAnimation ("Animation1");
			Graphics.Scale = new Vector2f (5, 5);
			Graphics.CenterOrigin ();

			// Register some test events
			Clicked += Player_Clicked;
			Game.Window.KeyPressed += Window_KeyPressed;
			
			// Move the camera to focus on this object
			Game.Camera.Target (Graphics.Position);

			DebugBBox = true;
		}

		public override void OnRemoved()
		{
			base.OnRemoved ();

			Game.Window.KeyPressed -= Window_KeyPressed;
		}

		private void Window_KeyPressed(object sender, KeyEventArgs e)
		{
			if (e.Code == Keyboard.Key.R)
			{
				FDebug.WriteLine ("Test");
			}
		}

		private void Player_Clicked(object sender, MouseButtonEventArgs e)
		{
			if (tween != null && tween.Completion != 0 && tween.Completion != 1)
				return;

			tween = Game.Tweener.Tween (Graphics, new { X = 20, Rotation = Graphics.Rotation + 45 }, 1);
			tween.Ease (Ease.QuadInOut);
			tween.Completed += Tween_Completed;
		}

		private void Tween_Completed(object sender, EventArgs e)
		{
			Graphics.X = (float) SrcObject.X;

			Graphics.NextFrame ();
		}

		public override void Draw(RenderTarget target, RenderStates states)
		{
			base.Draw (target, states);

			//Console.WriteLine ("Player Draw");
		}
	}
}
