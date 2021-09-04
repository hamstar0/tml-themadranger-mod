/*using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheMadRanger.Tiles;


namespace TheMadRanger.Items {
	class RevolverAmmoBoxItem : ModItem {
		public const int Width = 30;
		public const int Height = 30;


		////////////////

		public override void SetStaticDefaults() {
			this.DisplayName.SetDefault( "Revolver Ammo Box" );
			this.Tooltip.SetDefault( "Provides a seemingly limit-less source of specialty revolver ammo." );
		}

		public override void SetDefaults() {
			this.item.width = 12;
			this.item.height = 12;
			this.item.maxStack = 99;
			this.item.useTurn = true;
			this.item.autoReuse = true;
			this.item.useAnimation = 15;
			this.item.useTime = 10;
			this.item.useStyle = 1;
			this.item.consumable = true;
			this.item.value = Item.buyPrice( 0, 5, 0, 0 );
			this.item.createTile = ModContent.TileType<RevolverAmmoBoxTile>();
		}


		public override void AddRecipes() {
			var recipe = new RevolverAmmoBoxRecipe( this );
			recipe.AddRecipe();
		}
	}



	class RevolverAmmoBoxRecipe : ModRecipe {
		public RevolverAmmoBoxRecipe( RevolverAmmoBoxItem mymixer ) : base( TMRMod.Instance ) {
			this.AddTile( TileID.TinkerersWorkbench );

			//this.AddIngredient( ItemID.DyeVat, 1 );
			//this.AddIngredient( ItemID.Extractinator, 1 );

			this.SetResult( mymixer );
		}


		public override bool RecipeAvailable() {
			return TMRModConfig.RevolverAmmoBoxRecipeEnabled;
		}
	}
}*/
