using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledSharp;

namespace GETIMFISP.Extensions
{
	static class TmxTilesetExtenstions
	{
		public static Texture GetTileTex(this TmxTileset tileset, int x, int y)
		{
			return new Texture (new Image (tileset.Image.Source), new IntRect (x * tileset.TileWidth, y * tileset.TileHeight, tileset.TileWidth, tileset.TileHeight));
		}

		public static Texture GetTileTex(this TmxTileset tileset, int gid)
		{
			FDebug.WriteLine (gid);
			int localId = gid - tileset.FirstGid;
			int x = localId % (int) tileset.Columns;
			int y = (int) (localId / (float) tileset.Columns);
			return tileset.GetTileTex (x, y);
		}
	}
}
