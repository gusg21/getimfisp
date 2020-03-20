using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledSharp;

namespace GETIMFISP
{
	public class FTilesetManager
	{
		public List<FTileset> tilesets;

		public FTilesetManager(TmxList<TmxTileset> tilesets)
		{
			this.tilesets = new List<FTileset> ();
			foreach (TmxTileset tileset in tilesets)
			{
				this.tilesets.Add (new FTileset (tileset));
			}

			this.tilesets.Sort((a, b) => { return a.Gid.CompareTo (b.Gid); });
			this.tilesets.Reverse ();
		}

		public FTileset GetTilesetWithGid(int gid)
		{
			FTileset last = tilesets.First();
			foreach (FTileset tileset in tilesets)
			{
				if (gid > tileset.Gid)
				{
					break;
				}
				last = tileset;
			}
			return last;
		}
	}
}
