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

		// RENDERING
		// The window the game renders to.
		public RenderWindow window;
		// The information about the window
		public FWindowInfo windowInfo = FWindowInfo.SIMPLE_WINDOWED;
		// Convenience setters/getters for WindowInfo
		public string WindowTitle { get { return windowInfo.title; } set { windowInfo.title = value; } }
		public VideoMode WindowMode { get { return windowInfo.windowMode; } set { windowInfo.windowMode = value; } }
		public Styles WindowStyle { get { return windowInfo.style; } set { windowInfo.style = value; } }
		// Background color
		public Color backgroundColor = new Color (215, 123, 186);

		public FGame(string startingMap)
		{
			map = new TmxMap (startingMap);
			ActorTypes = new Dictionary<string, Type> ();
			ActorManager = new FActorManager ();
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
			// Loop through object groups in map
			foreach (TmxObjectGroup objGroup in map.ObjectGroups)
			{
				Console.WriteLine ("GROUP: " + objGroup.Name);
				// Loop through objects in the group
				foreach (TmxObject obj in objGroup.Objects)
				{
					Console.WriteLine ("  OBJECT: Name: [" + obj.Name + "] Type: [" + obj.Type + "]");
					if (!ActorTypes.ContainsKey(obj.Type)) // Is the type registered?
					{
						Console.WriteLine ($"    E: Unknown type: {obj.Type}");
						continue;
					}

					FActor actor;
					try // Is the type vaild/creatable/castable?
					{
						actor = (FActor) Activator.CreateInstance (ActorTypes [obj.Type]);
					}
					catch (Exception e)
					{
						Console.WriteLine ($"    E: Couldn't convert {obj.Type} to FActor:\n    {e.Message}");
						continue;
					}

					// Add it to the manager
					ActorManager.Add (actor);

					actor.OnCreated (obj);

					Console.WriteLine ("    Loaded successfully!");
				}
			}
		}

		/// <summary>
		/// Starts the game. Will block your code.
		/// </summary>
		public void Run()
		{
			LoadObjects ();

			window = new RenderWindow (WindowMode, WindowTitle, WindowStyle);

			RenderStates states = RenderStates.Default;

			Clock clock = new Clock ();
			Time delta = clock.Restart();

			while (window.IsOpen)
			{
				window.DispatchEvents ();

				ActorManager.Update (delta);

				window.Clear (backgroundColor);
				ActorManager.Draw (window, states);

				window.Display ();
			}
		}
	}
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         