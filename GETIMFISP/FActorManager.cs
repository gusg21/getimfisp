using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System.Collections.Generic;

namespace GETIMFISP
{
	/// <summary>
	/// A class to distribute Update() and Draw() calls amongst children FActors, and to make sure they're drawn in the right order
	/// </summary>
	public class FActorManager : FActor
	{
		// TODO: Multiple Actor Managers?

		/// <summary>
		/// The Game this is a child of
		/// 
		/// Can't be Game because that refers to whatever this is parented to
		/// </summary>
		public FGame MyGame;

		Dictionary<int, FActor> actors; // internal actor holder (id -> actor)
		List<FActor> sortedActors; // The actors in order
		List<int> removalQueue; // the actors to remove
		int nextId; // the next id to be given out

		/// <summary>
		/// Create a top level FActorManager, passing the FGame it's a child of.
		/// </summary>
		/// <param name="game"></param>
		public FActorManager(FGame game)
		{
			MyGame = game;
			actors = new Dictionary<int, FActor> ();
			sortedActors = new List<FActor> ();
			removalQueue = new List<int> ();

#if DEBUG
			Add (FDebugConsole.Instance);
			FDebugConsole.Instance.Manager = this; // Will happen multiple times, but the console doesn't care which manager it has
#endif

			SortByDepth ();
		}

		/// <summary>
		/// Add a new actor to the manager
		/// </summary>
		/// <param name="actor">the actor to add</param>
		public void Add(FActor actor)
		{
			int id = nextId++;
			actors.Add (id, actor);
			actor.ID = id;
			actor.Manager = this;

			SortByDepth ();
		}

		/// <summary>
		/// Actually remove an actor from the manager. Not public because that would break loops.
		/// </summary>
		/// <param name="id">The id of the actor to be removed</param>
		/// <returns>Success?</returns>
		bool _Remove(int id)
		{
			if (actors.ContainsKey(id)) // Is that an actual ID we have?
			{
				actors [id].OnRemoved ();
				actors [id].Manager = null;
				actors.Remove (id);
				return true;
			}

			return false;
		}

		/// <summary>
		/// Remove all ids in the removal queue.
		/// </summary>
		void RemoveQueued()
		{
			foreach (int id in removalQueue)
			{
				_Remove (id);
			}

			removalQueue.Clear ();
		}

		/// <summary>
		/// Queue an actor to be removed by its id
		/// </summary>
		/// <param name="id">the object to be removed's id</param>
		public void Remove(int id)
		{
			removalQueue.Add (id);
		}

		/// <summary>
		/// Queue an actor to be removed by the object
		/// </summary>
		/// <param name="actor">The actor object to be removed</param>
		/// <returns>Success?</returns>
		public void Remove(FActor actor)
		{
			Remove (actor.ID);
		}

		/// <summary>
		/// Remove all actors of a type
		/// </summary>
		/// <typeparam name="T">The FActor child type</typeparam>
		public void RemoveAll<T>() where T : FActor
		{
			List<FActor> tempActors = new List<FActor> (actors.Values); // Make a temporary copy off the list because we can't modify the list while looping through it
			foreach (FActor actor in tempActors)
			{
				if (actor is T) // is the actor the right type?
				{
					Remove (actor.ID);
				}
			}
		}

		/// <summary>
		/// Get an actor by their id
		/// </summary>
		/// <param name="id">the Id of the actor</param>
		/// <returns></returns>
		public FActor Get(int id)
		{
			return actors [id];
		}

		/// <summary>
		/// Return all actors of a type
		/// </summary>
		/// <typeparam name="T">the actor type</typeparam>
		/// <returns></returns>
		public List<T> GetActorsByType<T>() where T : FActor
		{
			List<T> actorsToReturn = new List<T> ();

			foreach (FActor actor in actors.Values)
			{
				if (actor is T) // is the actor the right type?
				{
					actorsToReturn.Add ((T) actor);
				}
			}

			return actorsToReturn;
		}

		/// <summary>
		/// Get one actor of a type. Not recommended unless you're sure there's only one actor.
		/// </summary>
		/// <typeparam name="T">The type to look for</typeparam>
		/// <returns></returns>
		public T GetActorByType<T>() where T : FActor
		{
			foreach (FActor actor in actors.Values)
			{
				if (actor is T) // is the actor the right type?
				{
					return (T) actor;
				}
			}

			return null;
		}

		/// <summary>
		/// Return all actors currently
		/// </summary>
		/// <returns></returns>
		public FActor[] GetActors()
		{
			return sortedActors.ToArray ();
		}
		
		/// <summary>
		/// Sort the actors so they render in the right order
		/// </summary>
		public void SortByDepth()
		{
			sortedActors = new List<FActor> (actors.Values);
			sortedActors.Sort ((FActor a, FActor b) => { return a.Depth.CompareTo (b.Depth); });
		}

		/// <summary>
		/// Get rid of all non-persistent actors when changing scenes.
		/// </summary>
		public void ClearNonPersistent()
		{
			foreach (FActor actor in sortedActors)
			{
				if (!actor.Persistent)
				{
					Remove (actor);
				}
			}
		}

		/// <summary>
		/// Tell all children actors they can set up events and initialize.
		/// </summary>
		public override void OnGraphicsReady()
		{
			foreach (FActor actor in sortedActors)
			{
				FDebug.WriteLine ($"{actor.Name}");
				actor.OnGraphicsReady ();
			}
		}

		/// <summary>
		/// Update all children actors
		/// </summary>
		/// <param name="delta">the time since the last frame</param>
		public override void Update(FGameTime delta)
		{
			base.Update (delta);

			foreach (FActor actor in sortedActors)
			{
				actor.Update (delta);
			}

			// we need to wait to actually remove actors because if we remove them while iterating it crashes
			if (removalQueue.Count > 0) // are there actors to be removed?
			{
				RemoveQueued (); // remove them
				SortByDepth (); // re-sort
			}
		}

		/// <summary>
		/// Draw all children actors
		/// </summary>
		/// <param name="target">the target surface to draw to</param>
		/// <param name="states">the current renderer state</param>
		public override void Draw(RenderTarget target, RenderStates states)
		{
			base.Draw (target, states);

			foreach (FActor actor in sortedActors)
			{
				actor.Draw (target, states);
			}
		}

		/// <summary>
		/// Draw the GUI all children actors
		/// </summary>
		/// <param name="target">the target surface to draw to</param>
		/// <param name="states">the current renderer state</param>
		public override void DrawGUI(RenderTarget target, RenderStates states)
		{
			base.DrawGUI (target, states);

			foreach (FActor actor in sortedActors)
			{
				actor.DrawGUI (target, states);
			}
		}
	}
}