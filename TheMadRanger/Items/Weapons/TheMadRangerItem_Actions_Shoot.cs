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
		/// Top level action for attempting to use a gun (short of producing the actual bullet projectile). May instead
		/// fail or begin automatic gun reloading, if needed.
		/// </summary>
		/// <param name="player"></param>
		/// <param name="speedX"></param>
		/// <param name="speedY"></param>
		/// <param name="damage"></param>
		/// <param name="knockBack"></param>
		/// <returns>`true` if a bullet can be produced from the gun.</returns>
		public bool UseGun( Player player, ref float speedX, ref float speedY, ref int damage, ref float knockBack ) {
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
			bool canShoot = this.DecideGunUse( out wantsReload );

			this.GunUseFx( player, canShoot );

			if( !canShoot ) {
				if( wantsReload ) {
					if( myplayer.GunHandling.BeginReload( player, this ) ) {
						if( Main.netMode == NetmodeID.MultiplayerClient && player.whoAmI == Main.myPlayer ) {
							GunAnimationProtocol.Broadcast( GunAnimationType.Reload );
						}
					}
				}
				return false;
			}

			bool hasShot = this.CylinderAttemptShoot();
			if( hasShot ) {
				this.PreShoot( myplayer );
				this.ModifyShootAim( myplayer, ref damage, ref speedX, ref speedY );
			}

			return hasShot;
		}

		////

		private bool DecideGunUse( out bool wantsReload ) {
			// Trigger auto-reload after short pause in firing attempts
			if( this.ElapsedTimeSinceLastShotAttempt >= 60 ) {
				if( !this.Cylinder.Any(b => b >= 1) ) {	// empty cylinder
					wantsReload = true;	// trigger auto-reload
					return false;
				}
			}
			this.ElapsedTimeSinceLastShotAttempt = 0;

			wantsReload = false;
			return this.CylinderCanShootNow();
		}

		private void PreShoot( TMRPlayer myplayer ) {
			// Refresh quick draw
			if( myplayer.AimMode.IsQuickDrawActive ) {
				myplayer.GunHandling.IsQuickDrawReady = true;
				myplayer.AimMode.AttemptQuickDrawMode( myplayer.player );
			}
		}

		private void ModifyShootAim( TMRPlayer myplayer, ref int damage, ref float speedX, ref float speedY ) {
			float shakeAddedRads = myplayer.AimMode.GetAimStateShakeAddedRadians( false );

			var randSpeed = new Vector2( speedX, speedY )
				.RotatedBy( shakeAddedRads );
			speedX = randSpeed.X;
			speedY = randSpeed.Y;

			damage = myplayer.AimMode.GetAimStateShakeDamage( damage );

			myplayer.GunHandling.BeginRecoil( MathHelper.ToDegrees(shakeAddedRads) * -myplayer.player.direction );
		}



		////////////////

		public void GunUseFx( Player player, bool isFiring ) {
			if( isFiring ) {
				SoundHelpers.PlaySound( TMRMod.Instance, "RevolverFire", player.Center, 0.2f );
			} else {
				SoundHelpers.PlaySound( TMRMod.Instance, "RevolverDryFire", player.Center, 0.2f );
			}
		}
	}
}
