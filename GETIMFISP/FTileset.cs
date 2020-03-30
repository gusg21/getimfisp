using SFML.Graphics;
using SFML.System;
using System;
using System.Diagnostics;
using TiledSharp;

namespace GETIMFISP
{
	/// <summary>
	/// The tileset 
	/// </summary>
	public class FTileset
	{
		TmxTileset tileset;
		Texture tilesetTex;
		Sprite tilesetSprite;

		/// <summary>
		/// The first GID of the tileset
		/// </summary>
		public int Gid { get { return tileset.FirstGid; } }
		/// <summary>
		/// The width of one tile in the tileset
		/// </summary>
		public int TileWidth { get { return tileset.TileWidth; } }
		/// <summary>
		/// The height of one tile in the tileset
		/// </summary>
		public int TileHeight { get { return tileset.TileHeight; } }

		/// <summary>
		/// Create from a tiled object
		/// </summary>
		/// <param name="tileset"></param>
		public FTileset(TmxTileset tileset)
		{
			Debug.Assert (tileset.Spacing == 0);

			this.tileset = tileset;
			tilesetTex = new Texture (tileset.Image.Source);
			tilesetSprite = new Sprite (tilesetTex);
		}

		/// <summary>
		/// Get the rectangle that represents a tile on this tileset
		/// </summary>
		/// <param name="localTileId"></param>
		/// <returns></returns>
		public IntRect GetTile(int localTileId)
		{
			int tileWidth = tileset.TileWidth;
			int tileHeight = tileset.TileHeight;
			int tilesWide = (int) tileset.Columns;

			return new IntRect (localTileId % tilesWide * tileWidth, (int) Math.Floor ((double) localTileId / tilesWide) * tileHeight, tileWidth, tileHeight);
		}

		/// <summary>
		/// Put a tile on screen
		/// </summary>
		/// <param name="localTileId">the tile index in THIS tileset (not the GID!)</param>
		/// <param name="x">x position to draw at</param>
		/// <param name="y">y position to draw at</param>
		/// <param name="color">the "tint" (used for alpha/opacity)</param>
		/// <param name="target">what to render to</param>
		/// <param name="states">renderer settings</param>
		public void DrawTile(int localTileId, int x, int y, Color color, RenderTarget target, RenderStates states)
		{
			tilesetSprite.Position = new Vector2f (x, y);
			tilesetSprite.TextureRect = GetTile (localTileId);
			tilesetSprite.Color = color;
			target.Draw (tilesetSprite, states);
		}
	}
}