using GETIMFISP.Extensions;
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
		/// <summary>
		/// Currently playing animation name
		/// </summary>
		public string CurrentAnimationName = null;
		/// <summary>
		/// The current FAnimation object
		/// </summary>
		public FAnimation CurrentAnimation { get { return animations [CurrentAnimationName]; } }
		/// <summary>
		/// Is the current animation playing?
		/// </summary>
		public bool IsAnimationPlaying { get { return CurrentAnimationName != null; } }

		/// <summary>
		/// Is this sprite visible?
		/// </summary>
		public bool Visible = true;

		/// <summary>
		/// Called when an animation is played
		/// </summary>
		public event EventHandler AnimationPlayed;

		/// <summary>
		/// Create a still sprite
		/// </summary>
		/// <param name="texture"></param>
		public FSprite(Texture texture)
		{
			FromTex (texture);
			Texture = CurrentAnimation.CurrentTexture;
		}
		
		/// <summary>
		/// Create a still sprite
		/// </summary>
		/// <param name="filepath"></param>
		public FSprite(string filepath)
		{
			FromTex (new Texture (filepath));
			Texture = CurrentAnimation.CurrentTexture;
		}

		/// <summary>
		/// Create a sprite with animation
		/// </summary>
		/// <param name="animations"></param>
		public FSprite(Dictionary<string, FAnimation> animations)
		{
			this.animations = animations;
			CurrentAnimationName = animations.First ().Key;
			Texture = CurrentAnimation.CurrentTexture;
		}

		/// <summary>
		/// Create a sprite with animation
		/// </summary>
		public FSprite()
		{
			animations = new Dictionary<string, FAnimation> ();
			FromTex (new Texture ("Internal Data/null.png"));
		}
		
		void FromTex(Texture tex)
		{
			CurrentAnimationName = "Still";
			animations = new Dictionary<string, FAnimation> ();
			AddAnimation ("Still", new FAnimation (new Texture [] { tex }));
			PlayAnimation ("Still");
		}

		/// <summary>
		/// Change the TextureRect to be the size of the animation
		/// </summary>
		public void CalcTextureRect()
		{
			TextureRect = new IntRect (new Vector2i(), CurrentAnimation.CurrentTexture.Size.To2i());
		}

		/// <summary>
		/// Set the origin of the sprite to be the center of the texture
		/// </summary>
		public void CenterOrigin()
		{
			Origin = (Texture.Size.To2f () / 2f);
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
			SwitchAnimation (animationName);

			CurrentAnimation.Playing = true;

			AnimationPlayed?.Invoke (this, new EventArgs ());
		}

		/// <summary>
		/// Changes the current animation without playing it
		/// </summary>
		/// <param name="animationName">The animation to switch to</param>
		public void SwitchAnimation(string animationName)
		{
			CurrentAnimationName = animationName;
			Texture = CurrentAnimation.CurrentTexture;

			CalcTextureRect ();
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
		/// Restarts the Animation currently playing.
		/// </summary>
		public void RestartAnimation()
		{
			CurrentAnimation.Restart ();
		}

		/// <summary>
		/// Go to the next frame of animation
		/// </summary>
		public void NextFrame()
		{
			CurrentAnimation.NextFrame ();
		}

		/// <summary>
		/// Go to the previous frame of animation
		/// </summary>
		public void PreviousFrame()
		{
			CurrentAnimation.PreviousFrame ();
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

			Color = new Color (Color.R, Color.G, Color.B, Visible ? Color.A : (byte) 0);
			Texture = CurrentAnimation.CurrentTexture;
		}
	}
}
