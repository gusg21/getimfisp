using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System.Collections.Generic;

namespace GETIMFISP
{
	/// <summary>
	/// A class to distribute Update() and Draw() calls amongst children FActors
	/// </summary>
	public class FActorManager
	{
		public FGame Game;

		Dictionary<int, FActor> actors; // internal actor holder (id -> actor)
		List<FActor> sortedActors;
		List<int> removalQueue; // the actors to remove
		int nextId; // the next id to be given out

		public FActorManager(FGame game)
		{
			Game = game;
			actors = new Dictionary<int, FActor> ();
			sortedActors = new List<FActor> ();
			removalQueue = new List<int> ();

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

		public void SortByDepth()
		{
			sortedActors = new List<FActor> (actors.Values);
			sortedActors.Sort ((FActor a, FActor b) => { return a.Depth.CompareTo (b.Depth); });
		}

		/// <summary>
		/// Update all children actors
		/// </summary>
		/// <param name="delta">the time since the last frame</param>
		public void Update(FGameTime delta)
		{
			foreach (FActor actor in actors.Values)
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
		public void Draw(RenderTarget target, RenderStates states)
		{
			foreach (FActor actor in actors.Values)
			{
				actor.Draw (target, states);
			}
		}
	}
}