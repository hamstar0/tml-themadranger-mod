using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using HamstarHelpers.Helpers.Debug;
using TheMadRanger.Items.Weapons;


namespace TheMadRanger {
	partial class TMRPlayer : ModPlayer {
		public bool CanShootGun() {
			return !this.GunAnim.IsAnimating;
		}


		public bool ShootGun(
					Item item,
					ref float speedX,
					ref float speedY,
					ref int damage,
					ref float knockBack ) {
			if( this.GunAnim.IsAnimating ) {
				return false;
			}

			bool wantsReload;
			var myitem = (TheMadRangerItem)item.modItem;

			if( !myitem.AttemptShot(this.player, out wantsReload) ) {
				if( wantsReload ) {
					this.GunAnim.BeginReload( this.player );
				}
				return true;
			}

			float shakeAddedRads = this.AimMode.GetAimStateShakeAddedRadians( false );

			Vector2 randSpeed = new Vector2( speedX, speedY )
				.RotatedBy( shakeAddedRads );
			speedX = randSpeed.X;
			speedY = randSpeed.Y;

			damage = this.AimMode.GetAimStateShakeDamage( damage );

			this.GunAnim.BeginRecoil( MathHelper.ToDegrees(shakeAddedRads) * -this.player.direction );

			return true;
		}
	}
}
