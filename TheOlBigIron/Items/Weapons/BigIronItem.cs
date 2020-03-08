using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace TheOlBigIron.Items.Weapons {
	public class BigIronItem : ModItem {
		public static int Width = 24;
		public static int Height = 16;



		////////////////

		public override void SetStaticDefaults() {
			this.DisplayName.SetDefault( "Ol' Big Iron" );
			this.Tooltip.SetDefault( "An antique gun from a far away land.\nRequires a specific, manufactured ammo" );
		}

		public override void SetDefaults() {
			this.item.width = BigIronItem.Width;
			this.item.height = BigIronItem.Height;
			this.item.scale = 0.5f;

			this.item.ranged = true;
			this.item.useStyle = 5;
			this.item.useTime = 6;
			this.item.useAnimation = 6;
			this.item.autoReuse = false;
			this.item.noMelee = true; //so the item's animation doesn't do damage
			this.item.shoot = ProjectileID.WoodenArrowFriendly;	//10; //idk why but all the guns in the vanilla source have this
			this.item.shootSpeed = 16f;
//			this.item.noUseGraphic = true;
			//this.item.useAmmo = AmmoID.Bullet;

			this.item.UseSound = SoundID.Item11;

			this.item.damage = 20;
			this.item.knockBack = 4;

			this.item.rare = 2;
			this.item.value = Item.buyPrice( 0, 5, 0, 0 );
		}
	}
}