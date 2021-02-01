using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;


namespace TheMadRanger.Items.Weapons {
	public partial class TheMadRangerItem : ModItem {
		public override bool CanUseItem( Player player ) {
			return player.GetModPlayer<TMRPlayer>()
				.GunHandling
				.CanAttemptToShootGun( player );
		}

		public override bool Shoot(
					Player player,
					ref Vector2 position,
					ref float speedX,
					ref float speedY,
					ref int type,
					ref int damage,
					ref float knockBack ) {
			bool canShoot = this.UseGun( player, ref speedX, ref speedY, ref damage, ref knockBack );
			return canShoot;
		}
	}
}