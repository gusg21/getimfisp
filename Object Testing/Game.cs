using GETIMFISP;
using Glide;
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
		Tween tween;

		public override void OnGraphicsReady()
		{
			base.OnGraphicsReady ();

			// Load an animation of ABC
			graphics.AddAnimation ("Animation1", new Texture [] { new Texture("Data/images/object1.png"), new Texture ("Data/images/object2.png"), new Texture ("Data/images/object3.png"), });
			graphics.SwitchAnimation ("Animation1");

			Clicked += Player_Clicked;

			Console.WriteLine (MidiDeviceManager.Default.OutputDevices.ToArray ()[1].Name);
			IMidiOutputDevice dev = MidiDeviceManager.Default.OutputDevices.ToArray () [1].CreateDevice ();
			Console.WriteLine (dev.Send (new RtMidi.Core.Messages.NoteOnMessage (Channel.Channel1, Key.Key59, 64)));
		}

		private void Player_Clicked(object sender, MouseButtonEventArgs e)
		{
			tween = Game.tweener.Tween (this, new { X = 20 }, 1);
			tween.Ease (Ease.QuadInOut);
			tween.Completed += Tween_Completed;
		}

		private void Tween_Completed(object sender, EventArgs e)
		{
			X = (float) srcObject.X;

			graphics.NextFrame ();
		}
	}
}
