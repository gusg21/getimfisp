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
		// The current camera
		public FCamera camera;

		// RENDERING
		// The window the game renders to.
		public RenderWindow window;
		// The information about the window
		public FWindowSettings windowSettings = FWindowSettings.SIMPLE_WINDOWED;
		// Convenience setters/getters for WindowInfo
		public string WindowTitle { get { return windowSettings.title; } set { windowSettings.title = value; } }
		public VideoMode WindowMode { get { return windowSettings.windowMode; } set { windowSettings.windowMode = value; } }
		public Styles WindowStyle { get { return windowSettings.style; } set { windowSettings.style = value; } }
		public bool IsFullscreen { get; private set; }
		// Background color
		public Color backgroundColor = new Color (33, 33, 34);

		public FGame(string mapPath, FWindowSettings settings)
		{
			map = new TmxMap (mapPath);
			windowSettings = settings;
			Construct ();
		}

		public FGame(string mapPath)
		{
			map = new TmxMap (mapPath);
			Construct ();
		}

		public FGame()
		{
			Construct ();
		}

		void Construct()
		{
			ActorTypes = new Dictionary<string, Type> ();
			ActorManager = new FActorManager (this);
			tweener = new Tweener ();
			camera = new FCamera (new Vector2f());
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

		public void ToggleFullscreen()
		{
			WindowStyle ^= Styles.Fullscreen;
			IsFullscreen = !IsFullscreen;
			CreateWindow ();
		}

		void LoadActors()
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
							actor.Graphics = new FSprite(tileset.GetTileTex (obj.Tile.Gid));
							break;
						
						case TmxObjectType.Basic:
							actor.BBox.Width = (float) obj.Width;
							actor.BBox.Height = (float) obj.Height;
							actor.CalcBBoxOffGraphics = false;
							break;

						// implement more types...
					}

					// Universal edits
					actor.Name = obj.Name;
					actor.SrcObject = obj;
					actor.Graphics.Position = new Vector2f ((float) actor.SrcObject.X, (float) actor.SrcObject.Y);

					// Add it to the manager
					ActorManager.Add (actor);

					// everything's ready; the object can now set itself up
					actor.OnGraphicsReady ();

					// Depth is setup in OnGraphicsReady(), so we account for it here
					ActorManager.SortByDepth ();

					Console.WriteLine ("    Loaded successfully!");

					// keep track of the object count we actually loaded
					successCount++;
				}
			}

			Console.WriteLine ($"=== Loaded {successCount} object(s) successfully. ===");

			ActorManager.SortByDepth ();
		}

		void LoadTilemaps()
		{
			int depth = 0;
			foreach (TmxLayer layer in map.Layers)
			{
				Console.WriteLine ($"Adding Tilemap for layer: {layer.Name}");

				FTilemap mapActor = new FTilemap (map, layer, new FTilesetManager(map.Tilesets), -1 - depth);
				ActorManager.Add (mapActor);
				mapActor.OnGraphicsReady ();

				depth++;
			}
		}

		public void CreateCamera()
		{
			camera.Resize (window.Size.X, window.Size.Y);
		}

		public void CreateWindow()
		{
			if (window != null && window.IsOpen)
			{
				window.Close ();
			}

			VideoMode mode = (IsFullscreen ? VideoMode.DesktopMode : WindowMode);
			Console.WriteLine (mode);
			window = new RenderWindow (mode, WindowTitle, WindowStyle);

			CreateCamera ();
		}

		/// <summary>
		/// Starts the game. Will block your code.
		/// </summary>
		public void Run()
		{
			// Window (and camera) creation
			CreateWindow ();
			RenderStates states = RenderStates.Default;

			// Window close event
			window.Closed += (sender, e) => { Console.WriteLine ("Closing window..."); ((Window) sender).Close (); };
			window.Resized += (sender, e) => { camera.Resize (e.Width, e.Height); };

			if (map != null)
			{
				// Load the objects into the game
				LoadActors ();
				// Load the tilemap
				LoadTilemaps ();
			}

			gameTime = new FGameTime ();

			// Main game loop
			while (window.IsOpen)
			{
				// Global data updates
				ActorManager.Update (gameTime);
				tweener.Update (gameTime);
				camera.Update (gameTime);

				// Window events update
				window.DispatchEvents ();

				// Rendering
				window.Clear (backgroundColor);
				window.SetView (camera.GetView());
				ActorManager.Draw (window, states);
				
				// Update the screen
				window.Display ();

				// record the frame time
				gameTime.Tick ();
			}
		}

		public void Stop()
		{
			window.Close ();
		}
	}
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         