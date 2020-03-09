using GETIMFISP.Extensions;
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

namespace GETIMFISP
{
	/// <summary>
	/// The central game class for GETIMFISP
	/// </summary>
	public class FGame
	{
		// DATA
		// The Tiled map object
		TmxMap map;
		// All the registered types of actors
		Dictionary<string, Type> ActorTypes;
		// The actor manager
		public FActorManager ActorManager { get; private set; }
		// The GameTime struct to keep track of all time-related things
		public FGameTime gameTime;
		// The Global object used for tweening
		public Tweener tweener;

		// RENDERING
		// The window the game renders to.
		public RenderWindow window;
		// The information about the window
		public FWindowSettings windowSettings = FWindowSettings.SIMPLE_WINDOWED;
		// Convenience setters/getters for WindowInfo
		public string WindowTitle { get { return windowSettings.title; } set { windowSettings.title = value; } }
		public VideoMode WindowMode { get { return windowSettings.windowMode; } set { windowSettings.windowMode = value; } }
		public Styles WindowStyle { get { return windowSettings.style; } set { windowSettings.style = value; } }
		// Background color
		public Color backgroundColor = new Color (215, 123, 186);

		public FGame(string startingMap)
		{
			map = new TmxMap (startingMap);
			ActorTypes = new Dictionary<string, Type> ();
			ActorManager = new FActorManager (this);
			tweener = new Tweener ();
		}

		/// <summary>
		/// Register a new type of actor
		/// </summary>
		/// <typeparam name="T">The type to register</typeparam>
		public void AddActorType<T>() where T : FActor
		{
			Console.WriteLine ("Registered script: " + typeof (T).Name);
			ActorTypes.Add (typeof (T).Name, typeof (T));
		}

		/// <summary>
		/// Register a new type of actor with a type name override
		/// </summary>
		/// <typeparam name="T">The type to register</typeparam>
		/// <param name="typeName">The name to use</param>
		public void AddActorType<T>(string typeName) where T : FActor
		{
			Console.WriteLine ($"Registered script with overridden name: {typeName}");
			ActorTypes.Add (typeName, typeof (T));
		}

		void LoadObjects()
		{
			int successCount = 0;

			// Loop through object groups in map
			foreach (TmxObjectGroup objGroup in map.ObjectGroups)
			{
				Console.WriteLine ("GROUP: " + objGroup.Name);
				// Loop through objects in the group
				foreach (TmxObject obj in objGroup.Objects)
				{
					Console.WriteLine ("  OBJECT: Name: [" + obj.Name + "] Type: [" + obj.Type + "] ObjType: [" + obj.ObjectType + "]");
					if (obj.Type == "")
					{
						Console.WriteLine ("    W: No type provided.");
					}

					if (!ActorTypes.ContainsKey(obj.Type) && obj.Type != "") // Is the type registered?
					{
						Console.WriteLine ($"    E: Unknown type: {obj.Type}");
						continue;
					}

					// create an empty actor to load into
					FActor actor = new FActor();
					
					// if the object's type isn't empty try to load it
					if (obj.Type != "")
						actor = (FActor) Activator.CreateInstance (ActorTypes [obj.Type]);

					// Custom functionality based on source tiled type
					switch (obj.ObjectType)
					{
						case TmxObjectType.Tile:
							// Loading tile image
							TmxTileset tileset = map.GetTilesetWithGid (obj.Tile.Gid);
							actor.graphics = new FSprite(tileset.GetTileTex (obj.Tile.Gid));
							break;
						case TmxObjectType.Basic:
							break;
					}

					// Universal edits
					actor.Position = new Vector2f ((float) obj.X, (float) obj.Y);
					actor.Name = obj.Name;
					actor.srcObject = obj;

					// Add it to the manager
					ActorManager.Add (actor);

					// everything's ready; the object can now set itself up
					actor.OnGraphicsReady ();

					Console.WriteLine ("    Loaded successfully!");
					successCount++;
				}
			}

			Console.WriteLine ($"=== Loaded {successCount} object(s) successfully. ===");
		}

		/// <summary>
		/// Starts the game. Will block your code.
		/// </summary>
		public void Run()
		{
			window = new RenderWindow (WindowMode, WindowTitle, WindowStyle);
			RenderStates states = RenderStates.Default;

			// Window close event
			window.Closed += (sender, e) => { ((Window) sender).Close (); };

			// Load the objects into the game
			LoadObjects ();

			gameTime = new FGameTime ();

			// Main game loop
			while (window.IsOpen)
			{
				// Global data updates
				ActorManager.Update (gameTime);
				tweener.Update (gameTime.AsSeconds ());

				// Window events update
				window.DispatchEvents ();

				// Rendering
				window.Clear (backgroundColor);
				ActorManager.Draw (window, states);
				
				// Update the screen
				window.Display ();

				// record the frame time
				gameTime.Tick ();
			}
		}
	}
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         