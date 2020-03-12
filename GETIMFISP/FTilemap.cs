using SFML.Graphics;
using TiledSharp;

namespace GETIMFISP
{
	public class FTilemap : FActor
	{
		TmxLayer layer;
		FTileset tileset;

		public FTilemap(TmxLayer layer, FTileset tileset, int depth)
		{
			this.layer = layer;
			this.tileset = tileset;

			Visible = layer.Visible;
			Name = "Tilemap";
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
					tileset.DrawTile (tile.Gid - 1, tile.X * tileset.TileWidth, tile.Y * tileset.TileHeight, target, states);
				}
			}
		}
	}
}
