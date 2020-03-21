using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GETIMFISP
{
	/// <summary>
	/// A container for all things game time related
	/// </summary>
	public class FGameTime
	{
		/// <summary>
		/// The time since last frame
		/// </summary>
		public Time Delta;
		/// <summary>
		/// The total time the game has been running
		/// </summary>
		public Time Elapsed;
		/// <summary>
		/// The internal clock to keep track of time with
		/// </summary>
		public Clock GameClock;

		/// <summary>
		/// Create a GameTime with default constructors
		/// </summary>
		public FGameTime()
		{
			Delta = new Time ();
			Elapsed = new Time ();
			GameClock = new Clock ();
		}

		/// <summary>
		/// Proxy for deltaTime.AsSeconds()
		/// </summary>
		/// <returns></returns>
		public float AsSeconds()
		{
			return Delta.AsSeconds ();
		}

		/// <summary>
		/// Call this every frame
		/// </summary>
		public void Tick()
		{
			Delta = GameClock.Restart ();
			Elapsed += Delta;
		}
	}
}
