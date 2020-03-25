using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using HamstarHelpers.Helpers.Debug;
using TheMadRanger.Helpers.Misc;
using TheMadRanger.NetProtocols;


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

			if( myplayer.GunHandling.IsReloading ) {
				if( !myplayer.GunHandling.ReloadingRounds ) {
					return false;
				}
				myplayer.GunHandling.StopReloading( player );
				this.ElapsedTimeSinceLastShotAttempt = 0;
			}

			bool wantsReload;
			if( !this.AttemptGunShotBegin(player, out wantsReload) ) {
				if( wantsReload ) {
					myplayer.GunHandling.BeginReload( player );

					if( Main.netMode == 1 && player.whoAmI == Main.myPlayer ) {
						GunAnimationProtocol.Broadcast( GunAnimationType.Reload );
					}
				}
				return false;
			}

			float shakeAddedRads = myplayer.AimMode.GetAimStateShakeAddedRadians( false );

			Vector2 randSpeed = new Vector2( speedX, speedY )
				.RotatedBy( shakeAddedRads );
			speedX = randSpeed.X;
			speedY = randSpeed.Y;

			damage = myplayer.AimMode.GetAimStateShakeDamage( damage );

			myplayer.GunHandling.BeginRecoil( MathHelper.ToDegrees(shakeAddedRads) * -player.direction );

			return true;
		}

		////

		private bool AttemptGunShotBegin( Player player, out bool wantsReload ) {
			wantsReload = false;

			if( this.ElapsedTimeSinceLastShotAttempt >= 60 ) {
				if( !this.Cylinder.Any( c => c >= 1 ) ) {
					wantsReload = true;
					return false;
				}
			}

			bool hasShot = false;

			if( this.CylinderShootOnce() ) {
				SoundHelpers.PlaySound( "RevolverFire", player.Center, 0.2f );
				hasShot = true;
			} else {
				SoundHelpers.PlaySound( "RevolverDryFire", player.Center, 0.2f );
				hasShot = false;
			}

			this.ElapsedTimeSinceLastShotAttempt = 0;

			return hasShot;
		}
	}
}
