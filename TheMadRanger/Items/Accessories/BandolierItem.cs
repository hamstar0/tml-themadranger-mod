﻿using Terraria;
using Terraria.ModLoader;


namespace TheMadRanger.Items.Accessories {
	[AutoloadEquip( EquipType.Waist )]
	class BandolierItem : ModItem {
		public static int Width = 24;
		public static int Height = 16;

		

		////////////////
		
		public override void SetStaticDefaults() {
			this.DisplayName.SetDefault( "Bandolier" );
			this.Tooltip.SetDefault( "An ample supply of .357 ammo." );
		}

		public override void SetDefaults() {
			this.item.width = BandolierItem.Width;
			this.item.height = BandolierItem.Height;
			this.item.maxStack = 1;
			this.item.defense = 3;
			this.item.value = Item.buyPrice( 0, 5, 0, 0 );
			this.item.rare = 5;
			this.item.accessory = true;
		}
	}
}
