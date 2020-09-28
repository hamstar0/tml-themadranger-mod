using System;
using Terraria;
using HamstarHelpers.Helpers.Debug;


namespace TheMadRanger {
	partial class PlayerAimMode {
		public bool AttemptQuickDrawMode( Player plr ) {
			var myplayer = plr.GetModPlayer<TMRPlayer>();
			if( !myplayer.GunHandling.IsQuickDrawReady ) {
				return false;
			}

			this.QuickDrawDuration = TMRConfig.Instance.Get<int>( nameof(TMRConfig.QuickDrawTickDuration) );

			/*Vector2 pos = GunAnimation.GetGunTipPosition(plr) - new Vector2(-2);

			for( int i = 0; i < 2; i++ ) {
				int dustIdx = Dust.NewDust( pos, 4, 4, 6, 0f, 0f, 0, Color.White, 1f );
				Dust dust = Main.dust[dustIdx];
				dust.noGravity = true;
				dust.shader = GameShaders.Armor.GetSecondaryShader( 3, plr );
			}*/

			return true;
		}


		////

		public void ApplySuccessfulHit( Player plr ) {
			var config = TMRConfig.Instance;
			int aimDuration = config.Get<int>( nameof(TMRConfig.AimModeActivationTickDuration) );
			int aimDurationAdd = config.Get<int>( nameof(TMRConfig.AimModeActivationTickDurationAddedBuffer) );
			int max = aimDuration + aimDurationAdd;

			// Switch to full aim mode
			if( this.IsQuickDrawActive ) {
				this.QuickDrawDuration = 0;
				this.AimElapsed = max;
			}
			// Otherwise, increase buildup to aim mode
			else {
				this.AimElapsed += config.Get<float>( nameof(TMRConfig.AimModeOnHitBuildupAmount) );
				if( this.AimElapsed > max ) {
					this.AimElapsed = max;
				}

				if( TMRConfig.Instance.DebugModeInfo ) {
					DebugHelpers.Print( "aim_hit", "aim%: "
						+ ( this.AimPercent * 100f ).ToString( "N0" )
						+ " (" + this.AimElapsed.ToString( "N1" ) + "), "
						+ max );
				}
			}
		}

		public void ApplyUnsuccessfulHit( Player plr ) {
			if( this.IsModeActive ) {
				return;
			}

			var config = TMRConfig.Instance;
			float aimMissBuildup = config.Get<float>( nameof(TMRConfig.AimModeOnMissBuildupAmount) );

			this.AimElapsed += aimMissBuildup;
			if( this.AimElapsed < 0f ) {
				this.AimElapsed = 0f;
			}

			if( config.DebugModeInfo ) {
				DebugHelpers.Print( "aim_miss", "aim%: "
					+ ( this.AimPercent * 100f ).ToString( "N0" )
					+ " (" + this.AimElapsed.ToString( "N1" ) + "), "
					+ aimMissBuildup );
			}
		}


		////////////////

		public void InitializeBulletProjectile( Projectile projectile ) {
			if( this.AimPercent >= 1f ) {
				projectile.maxPenetrate = 2;
			}
		}
	}
}
