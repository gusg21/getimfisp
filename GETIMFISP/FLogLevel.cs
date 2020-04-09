using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GETIMFISP
{
	/// <summary>
	/// The Level of printing to the debug.
	/// </summary>
	public enum FLogLevel
	{
		/// <summary>
		/// Just any info you might use to debug the program, but doesn't need to be seen by end users.
		/// </summary>
		INFO,
		/// <summary>
		/// Anything that could cause a problem.
		/// </summary>
		WARNING,
		/// <summary>
		/// Anything that is a problem.
		/// </summary>
		ERROR
	}
}
