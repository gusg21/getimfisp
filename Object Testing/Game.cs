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
			// Make a new game from a Tiled file
			game = new FGame ("Data/main.tmx");
			// Register Scripts
			game.AddActorType<Player> ();
			// Set window info
			game.windowSettings = FWindowSettings.SIMPLE_WINDOWED;
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
		Tween tween;
		WrenVM wren;
		string script;

		public override void OnGraphicsReady()
		{
			base.OnGraphicsReady ();

			// Load an animation of ABC
			graphics.AddAnimation ("Animation1", new Texture [] { new Texture("Data/images/object1.png"), new Texture ("Data/images/object2.png"), new Texture ("Data/images/object3.png"), });
			graphics.SwitchAnimation ("Animation1");
			graphics.Scale = new Vector2f (20, 1);

			Clicked += Player_Clicked;
			Game.window.KeyPressed += Window_KeyPressed;
			
			Game.camera.Target (Position);

			script = File.ReadAllText ("Data/" + srcObject.Properties ["clicked"]);

			WrenConfig config = new WrenConfig ();
			config.Write += (vm, text) => Console.Write (text);
			config.Error += (type, module, line, message) => Console.WriteLine ($"Error [{type}] in module [{module}] at line {line}:{Environment.NewLine}{message}");

			wren = new WrenVM (config);
		}

		private void Window_KeyPressed(object sender, KeyEventArgs e)
		{
			if (e.Code == Keyboard.Key.F)
			{
				Console.WriteLine ("Toggling fullscreen...");
				Game.ToggleFullscreen ();
			}
		}

		private void Player_Clicked(object sender, MouseButtonEventArgs e)
		{
			WrenInterpretResult result = wren.Interpret (script);
			WrenFunctionHandle testHandle = wren.MakeCallHandle ("test()");
			wren.Call (testHandle);
			testHandle.Release ();

			if (tween != null && tween.Completion != 0 && tween.Completion != 1)
				return;

			tween = Game.tweener.Tween (this, new { X = 20, RotationDegrees = RotationDegrees + 45 }, 1);
			tween.Ease (Ease.QuadInOut);
			tween.Completed += Tween_Completed;
		}

		private void Tween_Completed(object sender, EventArgs e)
		{
			X = (float) srcObject.X;

			graphics.NextFrame ();
		}

		public override void Draw(RenderTarget target, RenderStates states)
		{
			base.Draw (target, states);

			//Console.WriteLine ("Player Draw");
		}
	}
}
