using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GETIMFISP
{
	/// <summary>
	/// Represents some graphics, with or without animation
	/// </summary>
	public class FSprite : Sprite
	{
		Dictionary<string, FAnimation> animations;
		public string currentAnimationName = null;
		public FAnimation CurrentAnimation { get { return animations [currentAnimationName]; } }
		public bool IsAnimationPlaying { get { return currentAnimationName != null; } }

		public event EventHandler AnimationPlayed;

		/// <summary>
		/// Create a still sprite
		/// </summary>
		/// <param name="texture"></param>
		public FSprite(Texture texture)
		{
			FromTex (texture);
		}
		
		/// <summary>
		/// Create a still sprite
		/// </summary>
		/// <param name="filepath"></param>
		public FSprite(string filepath)
		{
			FromTex (new Texture (filepath));
		}

		/// <summary>
		/// Create a sprite with animation
		/// </summary>
		/// <param name="animations"></param>
		public FSprite(Dictionary<string, FAnimation> animations)
		{
			this.animations = animations;
		}

		/// <summary>
		/// Create a sprite with animation
		/// </summary>
		public FSprite()
		{
			animations = new Dictionary<string, FAnimation> ();
		}
		
		void FromTex(Texture tex)
		{
			animations = new Dictionary<string, FAnimation> ();
			AddAnimation ("Still", new FAnimation (new Texture [] { tex }));
			PlayAnimation ("Still");
		}

		/// <summary>
		/// Register a new animation not added in the constructor.
		/// </summary>
		/// <param name="animationName">The animation name</param>
		/// <param name="animation">The FAnimation object</param>
		public void AddAnimation(string animationName, FAnimation animation)
		{
			animations.Add (animationName, animation);
		}

		/// <summary>
		/// Register a new animation not added in the constructor.
		/// </summary>
		/// <param name="animationName">The animation name</param>
		/// <param name="frames">The FAnimation object</param>
		public void AddAnimation(string animationName, Texture[] frames)
		{
			animations.Add (animationName, new FAnimation(frames));
		}
		
		/// <summary>
		/// Play a registered animation by name
		/// </summary>
		/// <param name="animationName">The animation name to play</param>
		public void PlayAnimation(string animationName)
		{
			currentAnimationName = animationName;

			AnimationPlayed?.Invoke (this, new EventArgs ());
		}

		/// <summary>
		/// Get an animation
		/// </summary>
		/// <param name="animationName"></param>
		/// <returns></returns>
		public FAnimation GetAnimation(string animationName)
		{
			return animations [animationName];
		}

		/// <summary>
		/// Update the sprite
		/// </summary>
		/// <param name="delta"></param>
		public void Update(FGameTime delta)
		{
			if (IsAnimationPlaying)
			{
				CurrentAnimation.Update (delta.AsSeconds ());
			}

			Texture = CurrentAnimation.CurrentTexture;
		}
	}
}
