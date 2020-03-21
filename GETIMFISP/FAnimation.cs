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

		/// <summary>
		/// The individual Textures that make up the animation
		/// </summary>
		public List<Texture> frames;
		/// <summary>
		/// The number of frames in the animation
		/// </summary>
		public int NumOfFrames { get { return frames.Count; } }
		/// <summary>
		/// The amount of frames to show per second
		/// </summary>
		public int fps = 2;
		/// <summary>
		/// The current frame NUMBER
		/// </summary>
		public int CurrentFrame { get { return (int) Math.Floor ((currentPlaytime * fps) % NumOfFrames); } }
		/// <summary>
		/// The current frame texture
		/// </summary>
		public Texture CurrentTexture { get { return frames[CurrentFrame]; } }
		/// <summary>
		/// Is the animation playing?
		/// </summary>
		public bool Playing = false;

		/// <summary>
		/// Called when the animation frame change
		/// </summary>
		public event EventHandler FrameChanged;
		/// <summary>
		/// Called when the animation loops around
		/// </summary>
		public event EventHandler Looped;

		/// <summary>
		/// Create this animation from a List of textures.
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
			int lastFrame = CurrentFrame;
			if (Playing)
				currentPlaytime += dt;
			if (lastFrame != CurrentFrame)
			{
				FrameChanged?.Invoke (this, new EventArgs ());
				if (CurrentFrame == 0)
					Looped?.Invoke (this, new EventArgs ());
			}
		}

		/// <summary>
		/// Go to the next frame of animation
		/// </summary>
		public void NextFrame()
		{
			currentPlaytime += 1.0f / fps;
		}

		/// <summary>
		/// Go to the previous frame of animation
		/// </summary>
		public void PreviousFrame()
		{
			currentPlaytime -= 1.0f / fps;
		}

		/// <summary>
		/// Reset the playtime of the animation
		/// </summary>
		public void Restart()
		{
			currentPlaytime = 0f;
		}
	}
}
