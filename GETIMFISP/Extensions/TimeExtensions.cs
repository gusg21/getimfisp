using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GETIMFISP.Extensions
{
	public static class TimeExtensions
	{
		public static float ToFPS(this Time time)
		{
			return 1 / time.AsSeconds ();
		}
	}
}
