using System;
using Terraria.ID;
using Terraria.ModLoader;
using TheMadRanger.Items.Accessories;


namespace TheMadRanger.Recipes {
	class BandolierRecipe : ModRecipe {
		public BandolierRecipe( BandolierItem myitem ) : base( TMRMod.Instance ) {
			this.AddIngredient( ItemID.Leather, 10 );
			this.AddIngredient( ItemID.Silk, 10 );
			this.AddTile( TileID.WorkBenches );
			this.SetResult( myitem );
		}

		
		public override bool RecipeAvailable() {
			return TMRConfig.Instance.Get<bool>( nameof(TMRConfig.RecipeAvailableForBandolier) );
		}
	}
}
