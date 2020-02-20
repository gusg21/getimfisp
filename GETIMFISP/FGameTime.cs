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
		public Time deltaTime;
		/// <summary>
		/// The total time the game has been running
		/// </summary>
		public Time totalElapsedTime;
		/// <summary>
		/// The internal clock to keep track of time with
		/// </summary>
		public Clock gameClock;

		public FGameTime()
		{
			deltaTime = new Time ();
			totalElapsedTime = new Time ();
			gameClock = new Clock ();
		}

		/// <summary>
		/// Proxy for deltaTime.AsSeconds()
		/// </summary>
		/// <returns></returns>
		public float AsSeconds()
		{
			return deltaTime.AsSeconds ();
		}

		/// <summary>
		/// Call this every frame
		/// </summary>
		public void Tick()
		{
			deltaTime = gameClock.Restart ();
			totalElapsedTime += deltaTime;
		}
	}
}
