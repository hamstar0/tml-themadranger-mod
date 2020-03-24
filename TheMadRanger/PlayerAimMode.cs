using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Utilities;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.TModLoader;


namespace TheMadRanger {
	class PlayerAimMode {
		public static float ComputeAimShakeMaxConeRadians() {
			UnifiedRandom rand = TmlHelpers.SafelyGetRand();
			float radRange = MathHelper.ToRadians( TMRConfig.Instance.UnaimedConeDegreesRange );

			return (rand.NextFloat() * radRange) - (radRange * 0.5f);
		}



		////////////////

		public bool IsModeActive => this.AimElapsed >= TMRConfig.Instance.AimModeActivationThreshold;

		public bool IsModeBeingActivated => this.AimElapsed > 0 && this.AimElapsed >= this.PrevAimElapsed;

		public bool IsQuickDraw => this.QuickDrawDuration > 0;

		public float AimPercent => (float)this.AimElapsed / (float)TMRConfig.Instance.AimModeActivationThreshold;


		////////////////

		private float PrevAimElapsed = 0f;
		private float AimElapsed = 0f;

		private int QuickDrawDuration = 0;

		private Vector2 LastAimMousePosition = default( Vector2 );



		////////////////

		public void CheckAimState( Player plr ) {
			this.PrevAimElapsed = this.AimElapsed;

			if( this.QuickDrawDuration > 1 ) {
				this.QuickDrawDuration--;
				this.AimElapsed = TMRConfig.Instance.AimModeActivationThreshold + 2f;
			} else if( this.QuickDrawDuration == 1 ) {
				this.QuickDrawDuration = 0;
				this.AimElapsed = 0;
			}
		}

		public void CheckEquippedAimState( Player plr, Item prevHeldItem ) {
			var myplayer = plr.GetModPlayer<TMRPlayer>();

			// On fresh re-equip
			if( prevHeldItem != plr.HeldItem ) {
				if( !myplayer.GunAnim.IsAnimating ) {
					this.ApplyQuickDrawMode( plr );
				}
			}

			// Animations cancel aim mode
			if( myplayer.GunAnim.IsAnimating ) {
				this.AimElapsed = 0f;
				return;
			}

			// Player is moving
			if( plr.velocity.LengthSquared() > 1f ) {
				this.AimElapsed = Math.Max( this.AimElapsed - TMRConfig.Instance.AimModeDepleteRateWhilePlayerMoving, 0f );
				return;
			}

			var mousePos = new Vector2( Main.mouseX, Main.mouseY );

			// Mouse is moving
			if( (this.LastAimMousePosition - mousePos).LengthSquared() > 1f ) {
				this.AimElapsed = Math.Max( this.AimElapsed - TMRConfig.Instance.AimModeDepleteRateWhileMouseMoving, 0f );
			}
			// Otherwise, apply normal idling buildup
			else {
				int activationThreshold = TMRConfig.Instance.AimModeActivationThreshold + 2;	// Added buffer for slight aim tweaks
				if( this.AimElapsed < activationThreshold ) {
					this.AimElapsed += TMRConfig.Instance.AimModeBuildupRateWhileIdle;
				}
			}

			this.LastAimMousePosition = mousePos;
		}

		public void CheckUnequippedAimState() {
			this.AimElapsed = 0f;
			this.PrevAimElapsed = 0f;
		}


		////////////////

		public void ApplyQuickDrawMode( Player plr ) {
			this.QuickDrawDuration = TMRConfig.Instance.QuickDrawTickDuration;

			/*Vector2 pos = GunAnimation.GetGunTipPosition(plr) - new Vector2(-2);

			for( int i = 0; i < 2; i++ ) {
				int dustIdx = Dust.NewDust( pos, 4, 4, 6, 0f, 0f, 0, Color.White, 1f );
				Dust dust = Main.dust[dustIdx];
				dust.noGravity = true;
				dust.shader = GameShaders.Armor.GetSecondaryShader( 3, plr );
			}*/
		}

		public void ApplySuccessfulHit( Player plr ) {
			int max = (int)TMRConfig.Instance.AimModeActivationThreshold + (int)TMRConfig.Instance.AimModeBufferAddedThreshold;

			// Switch to full aim mode
			if( this.IsQuickDraw ) {
				this.QuickDrawDuration = 0;
				this.AimElapsed = TMRConfig.Instance.AimModeActivationThreshold + 2;
			}
			// Otherwise, increase buildup to aim mode
			else {
				this.AimElapsed += TMRConfig.Instance.AimModeOnHitBuildupAmount;
				if( this.AimElapsed > max ) {
					this.AimElapsed = max;
				}
			}
		}

		public void ApplyUnsuccessfulHit( Player plr ) {
			if( !this.IsModeActive ) {
				this.AimElapsed -= TMRConfig.Instance.AimModeOnMissLossAmount;
				if( this.AimElapsed < 0f ) {
					this.AimElapsed = 0f;
				}
			}
		}


		////////////////

		public void InitializeBulletProjectile( Projectile projectile ) {
			if( this.AimPercent >= 1f ) {
				projectile.maxPenetrate = 2;
			}
		}


		////////////////

		public float GetAimStateShakeAddedRadians( bool isIdling ) {
			if( this.IsModeActive ) {
				return 0f;
			}

			float rads = PlayerAimMode.ComputeAimShakeMaxConeRadians();
			if( isIdling ) {
				rads *= 0.03f;
			}

			return rads;
		}


		public int GetAimStateShakeDamage( int damage ) {
			if( this.IsModeActive ) {
				return damage;
			}

			UnifiedRandom rand = TmlHelpers.SafelyGetRand();
			float maxAimDmg = TMRConfig.Instance.MaximumAimedGunDamage;
			float minUnaimDmg = TMRConfig.Instance.MinimumUnaimedGunDamage;
			float maxUnaimDmg = TMRConfig.Instance.MaximumUnaimedGunDamage;
			float dmgPercent = (float)damage / maxAimDmg;

			float baseDmg = maxUnaimDmg * dmgPercent;
			float dmgMinPercent = minUnaimDmg / maxUnaimDmg;
			float dmgRand = dmgMinPercent + (rand.NextFloat() * (1f - dmgMinPercent) );
			
			int newDmg = (int)(dmgRand * (baseDmg - 1f)) + 1;
			return newDmg;
		}
	}
}
