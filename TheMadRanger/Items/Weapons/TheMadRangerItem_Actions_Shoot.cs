using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.Audio;
using TheMadRanger.NetProtocols;


namespace TheMadRanger.Items.Weapons {
	public partial class TheMadRangerItem : ModItem {
		/// <summary>
		/// Top level action for attempting to run all of the behaviors of firing a gun (short of producing the actual
		/// bullet projectile). May instead fail or begin gun reloading, if needed.
		/// </summary>
		/// <param name="player"></param>
		/// <param name="speedX"></param>
		/// <param name="speedY"></param>
		/// <param name="damage"></param>
		/// <param name="knockBack"></param>
		/// <returns></returns>
		public bool AttemptGunShot(
					Player player,
					ref float speedX,
					ref float speedY,
					ref int damage,
					ref float knockBack ) {
			var myplayer = player.GetModPlayer<TMRPlayer>();

			myplayer.HasAttemptedShotSinceEquip = true;

			if( !myplayer.GunHandling.CanAttemptToShootGun(player) ) {
				return false;
			}

			if( myplayer.GunHandling.IsReloading ) {
				if( !myplayer.GunHandling.ReloadingRounds ) {
					return false;
				}
				myplayer.GunHandling.StopReloading( player, this );
				this.ElapsedTimeSinceLastShotAttempt = 0;
			}

			bool wantsReload;
			if( !this.AttemptGunShotBegin(player, out wantsReload) ) {
				if( wantsReload ) {
					if( myplayer.GunHandling.BeginReload( player, this ) ) {
						if( Main.netMode == NetmodeID.MultiplayerClient && player.whoAmI == Main.myPlayer ) {
							GunAnimationProtocol.Broadcast( GunAnimationType.Reload );
						}
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
				SoundHelpers.PlaySound( TMRMod.Instance, "RevolverFire", player.Center, 0.2f );
				hasShot = true;
			} else {
				SoundHelpers.PlaySound( TMRMod.Instance, "RevolverDryFire", player.Center, 0.2f );
				hasShot = false;
			}

			this.ElapsedTimeSinceLastShotAttempt = 0;

			return hasShot;
		}
	}
}
