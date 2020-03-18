using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using HamstarHelpers.Helpers.Debug;


namespace TheMadRanger.Items.Weapons {
	public partial class TheMadRangerItem : ModItem {
		public bool AttemptGunShot(
					Player player,
					ref float speedX,
					ref float speedY,
					ref int damage,
					ref float knockBack ) {
			var myplayer = player.GetModPlayer<TMRPlayer>();
			if( !myplayer.CanAttemptToShootGun() ) {
				return false;
			}

			bool wantsReload;
			if( !this.AttemptShot(player, out wantsReload) ) {
				if( wantsReload ) {
					myplayer.GunAnim.BeginReload( player );
				}
				return false;
			}

			float shakeAddedRads = myplayer.AimMode.GetAimStateShakeAddedRadians( false );

			Vector2 randSpeed = new Vector2( speedX, speedY )
				.RotatedBy( shakeAddedRads );
			speedX = randSpeed.X;
			speedY = randSpeed.Y;

			damage = myplayer.AimMode.GetAimStateShakeDamage( damage );

			myplayer.GunAnim.BeginRecoil( MathHelper.ToDegrees(shakeAddedRads) * -player.direction );

			return true;
		}
	}
}
