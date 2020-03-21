using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledSharp;

namespace GETIMFISP
{
	/// <summary>
	/// The set of tilesets organized so you can look up a GID and find a tileset with that GID
	/// </summary>
	public class FTilesetManager
	{
		/// <summary>
		/// The internal list of the tilesets
		/// </summary>
		public List<FTileset> Tilesets;

		/// <summary>
		/// Create from a list of tilesets
		/// </summary>
		/// <param name="tilesets"></param>
		public FTilesetManager(TmxList<TmxTileset> tilesets)
		{
			Tilesets = new List<FTileset> ();
			foreach (TmxTileset tileset in tilesets)
			{
				Tilesets.Add (new FTileset (tileset));
			}

			Tilesets.Sort((a, b) => { return a.Gid.CompareTo (b.Gid); });
			Tilesets.Reverse ();
		}

		/// <summary>
		/// Find the tileset that contains a GID
		/// </summary>
		/// <param name="gid"></param>
		/// <returns></returns>
		public FTileset GetTilesetWithGid(int gid)
		{
			FTileset last = Tilesets.First();
			foreach (FTileset tileset in Tilesets)
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
