using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledSharp;

namespace GETIMFISP.Extensions
{
	static class TmxMapExtenstions
	{
		public static TmxTileset GetTilesetWithGid(this TmxMap map, int gid)
		{
			//TODO: Might be broken; if map.Tilesets isn't sorted
			foreach (TmxTileset set in map.Tilesets)
			{
				if (gid > set.FirstGid)
					return set;
			}

			return map.Tilesets [0];
		}
	}
}
