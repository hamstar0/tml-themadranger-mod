﻿using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModLibsCore.Libraries.Debug;
using ModLibsGeneral.Libraries.Audio;
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
		/// <returns>`true` if a bullet is to be produced from the gun.</returns>
		public bool UseGun(
					Player player,
					ref float speedX,
					ref float speedY,
					ref int damage,
					ref float knockBack ) {
			var myplayer = player.GetModPlayer<TMRPlayer>();

			if( !myplayer.GunHandling.CanAttemptToShootGun(player) ) {
				return false;
			}

			//

			myplayer.HasAttemptedShotSinceEquip = true;

			//

			if( myplayer.GunHandling.IsReloading ) {
				if( !myplayer.GunHandling.ReloadingRounds ) {
					return false;
				}

				//

				myplayer.GunHandling.StopReloading( player, this );

				//

				this.ElapsedTimeSinceLastShotAttempt = 0;
			}

			//

			bool wantsReload;
			bool canShoot = this.DecideGunUse( out wantsReload );
			bool hasShot = false;

			this.GunUseFx( player, canShoot );

			//

			if( !canShoot ) {
				if( wantsReload ) {
					bool isReloading = myplayer.GunHandling.BeginReload_If( player, this, false );

					if( isReloading ) {
						if( Main.netMode == NetmodeID.MultiplayerClient && player.whoAmI == Main.myPlayer ) {
							GunAnimationPacket.BroadcastFromLocalPlayer( GunAnimationType.Reload );
						}
					}
				} else {
					this.RotateCylinder( 1 );
				}
			} else {
				hasShot = this.CylinderAttemptShoot();

				//

				if( hasShot ) {
					this.PreShoot( myplayer );

					//

					this.ModifyShootAim( myplayer, ref damage, ref speedX, ref speedY );
				}
			}

			//

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
			float offsetRads = myplayer.AimMode.GetAimStateShakeRadiansOffset( false );

			var randSpeed = new Vector2( speedX, speedY )
				.RotatedBy( offsetRads );
			speedX = randSpeed.X;
			speedY = randSpeed.Y;

			damage = myplayer.AimMode.GetAimStateShakeDamage( damage );

			//

			float addedRotDegrees = MathHelper.ToDegrees( offsetRads ) * -myplayer.player.direction;

			myplayer.GunHandling.BeginRecoil( addedRotDegrees );

			//

			if( Main.netMode == NetmodeID.MultiplayerClient && myplayer.player.whoAmI == Main.myPlayer ) {
				GunAnimationPacket.BroadcastFromLocalPlayer( GunAnimationType.Recoil, addedRotDegrees );
			}
		}



		////////////////

		public void GunUseFx( Player player, bool isFiring ) {
			if( isFiring ) {
				SoundLibraries.PlaySound( TMRMod.Instance, "RevolverFire", player.Center, 0.2f );
			} else {
				SoundLibraries.PlaySound( TMRMod.Instance, "RevolverDryFire", player.Center, 0.2f );
			}
		}
	}
}
