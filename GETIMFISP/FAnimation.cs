using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GETIMFISP
{
	/// <summary>
	/// Represents a single animation
	/// </summary>
	public class FAnimation
	{
		float currentPlaytime = 0f;
		public List<Texture> frames;
		public int NumOfFrames { get { return frames.Count; } }
		public int CurrentFrame { get { return (int) Math.Floor (currentPlaytime % NumOfFrames); } }
		public Texture CurrentTexture { get { return frames[CurrentFrame]; } }

		/// <summary>
		/// Create this animation from a List<> of textures.
		/// </summary>
		/// <param name="frames"></param>
		public FAnimation(List<Texture> frames)
		{
			this.frames = frames;
		}

		/// <summary>
		/// Create this animation from an array of Textures.
		/// </summary>
		/// <param name="frames"></param>
		public FAnimation(Texture[] frames)
		{
			this.frames = new List<Texture> (frames);
		}

		/// <summary>
		/// Create an empty animation
		/// </summary>
		public FAnimation()
		{
			frames = new List<Texture> ();
		}

		/// <summary>
		/// Update the texture and keep track of time
		/// </summary>
		/// <param name="dt"></param>
		public void Update(float dt)
		{
			currentPlaytime += dt;
			Console.WriteLine ($"Current Playtime: {currentPlaytime}");
		}
	}
}
