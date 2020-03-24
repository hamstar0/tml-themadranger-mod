using System;
using Terraria.ID;
using Terraria.ModLoader;
using TheMadRanger.Items;


namespace TheMadRanger.Recipes {
	class SpeedloaderRecipe : ModRecipe {
		public SpeedloaderRecipe( SpeedloaderItem myitem ) : base( TMRMod.Instance ) {
			this.AddIngredient( ItemID.IllegalGunParts, 1 );
			this.AddIngredient( ItemID.Actuator, 6 );
			this.AddTile( TileID.WorkBenches );
			this.SetResult( myitem );
		}

		
		public override bool RecipeAvailable() {
			return TMRConfig.Instance.RecipeAvailableForSpeedloader;
		}
	}
}
