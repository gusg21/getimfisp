using GETIMFISP;
using Glide;
using ImGuiNET;
using RtMidi.Core;
using RtMidi.Core.Devices;
using RtMidi.Core.Devices.Infos;
using RtMidi.Core.Enums;
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

		public override void OnGraphicsReady()
		{
			base.OnGraphicsReady ();

			// Load an animation of ABC
			graphics.AddAnimation ("Animation1", new Texture [] { new Texture("Data/images/object1.png"), new Texture ("Data/images/object2.png"), new Texture ("Data/images/object3.png"), });
			graphics.SwitchAnimation ("Animation1");

			Clicked += Player_Clicked;
			Game.window.KeyPressed += Window_KeyPressed;

			graphics.Scale = new Vector2f(20, 1);

			Game.camera.Target (Position);
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
