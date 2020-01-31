using SFML.Graphics;
using SFML.System;
using System.Collections.Generic;

namespace GETIMFISP
{
	/// <summary>
	/// A class to distribute Update() and Draw() calls amongst children FActors
	/// </summary>
	public class FActorManager
	{
		Dictionary<int, FActor> actors; // internal actor holder (id -> actor)
		int nextId; // the next id to be given out

		public FActorManager()
		{
			actors = new Dictionary<int, FActor> ();
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
		}

		public void Update(Time delta)
		{
			foreach (FActor actor in actors.Values)
			{
				actor.Update (delta);
			}
		}

		public void Draw(RenderTarget target, RenderStates states)
		{
			foreach (FActor actor in actors.Values)
			{
				actor.Draw (target, states);
			}
		}
	}
}