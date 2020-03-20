using SFML.Graphics;
using SFML.System;
using System;
using System.Diagnostics;
using TiledSharp;

namespace GETIMFISP
{
	public class FTileset
	{
		TmxTileset tileset;
		Texture tilesetTex;
		Sprite tilesetSprite;

		public int Gid { get { return tileset.FirstGid; } }
		public int TileWidth { get { return tileset.TileWidth; } }
		public int TileHeight { get { return tileset.TileHeight; } }

		public FTileset(TmxTileset tileset)
		{
			Debug.Assert (tileset.Spacing == 0);

			this.tileset = tileset;
			tilesetTex = new Texture (tileset.Image.Source);
			tilesetSprite = new Sprite (tilesetTex);
		}

		public IntRect GetTile(int localTileId)
		{
			int tileWidth = tileset.TileWidth;
			int tileHeight = tileset.TileHeight;
			int tilesWide = (int) tileset.Columns;

			return new IntRect (localTileId % tilesWide * tileWidth, (int) Math.Floor ((double) localTileId / tilesWide) * tileHeight, tileWidth, tileHeight);
		}

		public void DrawTile(int localTileId, int x, int y, Color color, RenderTarget target, RenderStates states)
		{
			tilesetSprite.Position = new Vector2f (x, y);
			tilesetSprite.TextureRect = GetTile (localTileId);
			tilesetSprite.Color = color;
			target.Draw (tilesetSprite, states);
		}
	}
}