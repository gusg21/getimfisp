using SFML.Graphics;
using System.Collections.Generic;
using TiledSharp;

namespace GETIMFISP
{
	public class FTilemap : FActor
	{
		TmxMap map;
		TmxLayer layer;
		FTilesetManager tilesets;
		Color color;

		public FTilemap(TmxMap map, TmxLayer layer, FTilesetManager tilesets, int depth)
		{
			this.map = map;
			this.layer = layer;
			this.tilesets = tilesets;

			Visible = layer.Visible;
			Name = "Tilemap";
			color = new Color (255, 255, 255, (byte) (layer.Opacity * 255));
		}

		public override void OnGraphicsReady()
		{
			base.OnGraphicsReady ();

			UseBuiltinRenderer = false;

			Depth = -1;
		}

		public override void Update(FGameTime delta)
		{
			base.Update (delta);
		}

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
