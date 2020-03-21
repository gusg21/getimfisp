using SFML.Graphics;
using System.Collections.Generic;
using TiledSharp;

namespace GETIMFISP
{
	/// <summary>
	/// Represents a layer of tiles arranged in a grid
	/// </summary>
	public class FTilemap : FActor
	{
		TmxMap map;
		TmxLayer layer;
		FTilesetManager tilesets;
		int depth;
		Color color;

		/// <summary>
		/// Create a tilemap
		/// </summary>
		/// <param name="map">The source tiled map</param>
		/// <param name="layer">The source tiled tile layer</param>
		/// <param name="tilesets">The set of tilesets to source the tiles from</param>
		/// <param name="depth">The depth of the tilemap</param>
		public FTilemap(TmxMap map, TmxLayer layer, FTilesetManager tilesets, int depth = -1)
		{
			this.map = map;
			this.layer = layer;
			this.tilesets = tilesets;
			this.depth = depth;

			Visible = layer.Visible;
			Name = "Tilemap";
			color = new Color (255, 255, 255, (byte) (layer.Opacity * 255));
		}

		/// <summary>
		/// Called when graphics are set up
		/// </summary>
		public override void OnGraphicsReady()
		{
			base.OnGraphicsReady ();

			UseBuiltinRenderer = false;

			Depth = depth;
		}

		/// <summary>
		/// Update the tilemap
		/// </summary>
		/// <param name="delta"></param>
		public override void Update(FGameTime delta)
		{
			base.Update (delta);
		}

		/// <summary>
		/// Draw the tilemap
		/// </summary>
		/// <param name="target"></param>
		/// <param name="states"></param>
		public override void Draw(RenderTarget target, RenderStates states)
		{
			base.Draw (target, states);
			
			if (!Visible)
				return;
			
			foreach (TmxLayerTile tile in layer.Tiles)
			{
				if (tile.Gid > 0)
				{
					FTileset tileset = tilesets.GetTilesetWithGid (tile.Gid);
					tileset.DrawTile (tile.Gid - tileset.Gid, tile.X * map.TileWidth, tile.Y * map.TileHeight, color, target, states);
				}
			}
		}
	}
}
