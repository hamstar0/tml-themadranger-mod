/*using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;


namespace TheMadRanger.Tiles {
	class RevolverAmmoBoxTile : ModTile {
		public override void SetDefaults() {
			Main.tileSolidTop[ this.Type ] = true;
			Main.tileFrameImportant[ this.Type ] = true;
			Main.tileNoAttach[ this.Type ] = true;
			Main.tileTable[ this.Type ] = true;
			Main.tileLavaDeath[ this.Type ] = false;
			TileObjectData.newTile.CopyFrom( TileObjectData.Style2x2 );
			TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16 };
			TileObjectData.addTile( this.Type );

			ModTranslation name = this.CreateMapEntryName();
			name.SetDefault( "Revolver Ammo Box" );

			this.AddMapEntry( new Color(64, 128, 32), name );
			this.dustType = 1;
			this.disableSmartCursor = true;
			this.adjTiles = new int[] { this.Type };
		}


		public override void NumDust( int i, int j, bool fail, ref int num ) {
			num = fail ? 1 : 3;
		}

		public override void KillMultiTile( int i, int j, int frameX, int frameY ) {
			Item.NewItem( i * 16, j * 16, 32, 16, mod.ItemType("RevolverAmmoBoxItem") );
		}
	}
}*/
