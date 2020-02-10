using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GETIMFISP
{
	public class FAnimatedSprite : Sprite
	{
		public Dictionary<string, FAnimation> animations;
		public string currentAnimationName = null;
		public FAnimation CurrentAnimation { get { return animations [currentAnimationName]; } }
		public bool IsAnimation { get { return currentAnimationName != null; } }

		public FAnimatedSprite(Dictionary<string, FAnimation> animations)
		{
			this.animations = animations;
		}

		public FAnimatedSprite()
		{
			animations = new Dictionary<string, FAnimation> ();
		}

		public void Update(FGameTime delta)
		{
			float globalTime = delta.elapsedTime.AsSeconds();

			if (IsAnimation)
				Texture = CurrentAnimation.GetTexture ();
		}
	}
}
