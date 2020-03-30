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
		Dictionary<string, Type> actorTypes;
		/// <summary>
		/// The actor manager
		/// </summary>
		public FActorManager ActorManager { get; private set; }
		/// <summary>
		/// The object that keeps track of time related things
		/// </summary>
		public FGameTime GameTime;
		/// <summary>
		/// Used to create a tween
		/// </summary>
		public Tweener Tweener;
		/// <summary>
		/// The current camera
		/// </summary>
		public FCamera Camera;

		// RENDERING
		/// <summary>
		/// The window the game draws to.
		/// </summary>
		public RenderWindow Window;
		/// <summary>
		/// Set up info for the window
		/// </summary>
		public FWindowSettings WindowSettings = FWindowSettings.SIMPLE_WINDOWED;
		/// <summary>
		/// The default background color of the window.
		/// </summary>
		public Color ClearColor = new Color (33, 33, 34);

		/// <summary>
		/// Create the Game from a Map and window settings
		/// </summary>
		/// <param name="mapPath">the path to the Tiled map</param>
		/// <param name="settings">the window settings to use</param>
		public FGame(FWindowSettings settings)
		{
			WindowSettings = settings;
			Construct ();
		}

		/// <summary>
		/// Create an empty game, a blank canvas if you will.
		/// </summary>
		public FGame()
		{
			Construct ();
		}

		void Construct()
		{
			actorTypes = new Dictionary<string, Type> ();
			ActorManager = new FActorManager (this);
			Tweener = new Tweener ();
			Camera = new FCamera ();

			// Window (and camera) creation
			Window = new RenderWindow (WindowSettings.WindowMode, WindowSettings.Title, WindowSettings.Style);

			// Window close event
			Window.Closed += (sender, e) => { Console.WriteLine ("Closing window..."); ((Window) sender).Close (); };
		}

		/// <summary>
		/// Register a new type of actor
		/// </summary>
		/// <typeparam name="T">The type to register</typeparam>
		public void AddActorType<T>() where T : FActor
		{
			Console.WriteLine ("Registered script: " + typeof (T).Name);
			actorTypes.Add (typeof (T).Name, typeof (T));
		}

		/// <summary>
		/// Register a new type of actor with a type name override
		/// </summary>
		/// <typeparam name="T">The type to register</typeparam>
		/// <param name="typeName">The name to use</param>
		public void AddActorType<T>(string typeName) where T : FActor
		{
			Console.WriteLine ($"Registered script with overridden name: {typeName}");
			actorTypes.Add (typeName, typeof (T));
		}

		/// <summary>
		/// Load a different Tiled map (level, menu, etc...)
		/// </summary>
		/// <param name="newMapPath"></param>
		public void ChangeScene(string newMapPath)
		{
			// Create a new Tiled Map object (tmx = tiled)
			map = new TmxMap (newMapPath);

			// Kill all actors which are not marked as persistent
			ActorManager.ClearNonPersistent ();

			// Load the objects into the game
			LoadActors ();

			// Load the tilemap
			LoadTilemaps ();
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

					if (!actorTypes.ContainsKey(obj.Type) && obj.Type != "") // Is the type registered?
					{
						Console.WriteLine ($"    E: Unknown type: {obj.Type}");
						continue;
					}

					// create an empty actor to load into
					FActor actor = new FActor();
					
					// if the object's type isn't empty try to load it
					if (obj.Type != "")
						actor = (FActor) Activator.CreateInstance (actorTypes [obj.Type]);
					
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
					actor.Visible = obj.Visible;
					actor.SrcObject = obj;
					actor.Graphics.Position = new Vector2f ((float) actor.SrcObject.X, (float) actor.SrcObject.Y);
					Console.WriteLine ($"{actor.Graphics.Position}");

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

				FTilemap mapActor = new FTilemap (map, layer, new FTilesetManager (map.Tilesets), -1 - depth);
				ActorManager.Add (mapActor);
				mapActor.OnGraphicsReady ();

				depth++;
			}
		}

		/// <summary>
		/// Starts the game. Will block your code.
		/// </summary>
		public void Run()
		{
			RenderStates states = RenderStates.Default;

			Camera.Initialize (this);
			
			GameTime = new FGameTime ();

			// Main game loop
			while (Window.IsOpen)
			{
				// Global data updates
				ActorManager.Update (GameTime);
				Tweener.Update (GameTime);
				Camera.Update (GameTime);

				// Window events update
				Window.DispatchEvents ();

				// Rendering
				Window.Clear (ClearColor);
				Window.SetView (Camera.GetView());
				ActorManager.Draw (Window, states);
				
				// Update the screen
				Window.Display ();

				// record the frame time
				GameTime.Tick ();
			}
		}

		/// <summary>
		/// Stop the game.
		/// </summary>
		public void Stop()
		{
			Window.Close ();
		}
	}
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         