using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GETIMFISP
{
	public struct FGameTime
	{
		public Time deltaTime;
		public Time elapsedTime;
		public Clock gameClock;

		public void Tick()
		{
			deltaTime = gameClock.Restart ();
			elapsedTime += deltaTime;
		}
	}
}
