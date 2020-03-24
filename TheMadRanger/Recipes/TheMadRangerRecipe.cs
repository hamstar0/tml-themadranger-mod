using System;
using Terraria.ID;
using Terraria.ModLoader;
using TheMadRanger.Items.Weapons;


namespace TheMadRanger.Recipes {
	class TheMadRangerRecipe : ModRecipe {
		public TheMadRangerRecipe( TheMadRangerItem myitem ) : base( TMRMod.Instance ) {
			this.AddIngredient( ItemID.IllegalGunParts, 1 );
			this.AddIngredient( ItemID.SilverBar, 15 );
			this.AddIngredient( ItemID.SuspiciousLookingEye, 1 );
			this.AddTile( TileID.WorkBenches );
			this.SetResult( myitem );
		}


		public override bool RecipeAvailable() {
			return TMRConfig.Instance.RecipeAvailableForTheMadRanger;
		}
	}
}
