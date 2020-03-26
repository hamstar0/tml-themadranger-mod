using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Utilities;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.TModLoader;


namespace TheMadRanger {
	partial class PlayerAimMode {
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
				if( !myplayer.GunHandling.IsAnimating ) {
					this.ApplyQuickDrawMode( plr );
				}
			}

			// Animations cancel aim mode
			if( myplayer.GunHandling.IsAnimating ) {
				this.AimElapsed = 0f;
				return;
			}

			// Player is moving
			if( plr.velocity.LengthSquared() > 1f ) {
				if( TMRConfig.Instance.DebugModeInfo ) {
					float aimPercent = this.AimElapsed / TMRConfig.Instance.AimModeActivationThreshold;
					DebugHelpers.Print( "aim_down_move", "aim%: "
						+ ( aimPercent * 100f ).ToString( "N0" )
						+ " (" + this.AimElapsed.ToString( "N1" ) + "), "
						+ "-" + TMRConfig.Instance.AimModeDepleteRateWhilePlayerMoving );
				}
				this.AimElapsed = Math.Max( this.AimElapsed - TMRConfig.Instance.AimModeDepleteRateWhilePlayerMoving, 0f );
				return;
			}

			var mousePos = new Vector2( Main.mouseX, Main.mouseY );

			// Mouse is moving
			if( (this.LastAimMousePosition - mousePos).LengthSquared() > 1f ) {
				if( TMRConfig.Instance.DebugModeInfo ) {
					float aimPercent = this.AimElapsed / TMRConfig.Instance.AimModeActivationThreshold;
					DebugHelpers.Print( "aim_down_mouse", "aim%: "
						+ ( aimPercent * 100f ).ToString( "N0" )
						+ " (" + this.AimElapsed.ToString( "N1" ) + "), "
						+ "-" + TMRConfig.Instance.AimModeDepleteRateWhileMouseMoving );
				}
				this.AimElapsed = Math.Max( this.AimElapsed - TMRConfig.Instance.AimModeDepleteRateWhileMouseMoving, 0f );
			}
			// Otherwise, apply normal idling buildup
			else {
				int activationThreshold = TMRConfig.Instance.AimModeActivationThreshold + 2;	// Added buffer for slight aim tweaks
				if( this.AimElapsed < activationThreshold ) {
					if( TMRConfig.Instance.DebugModeInfo ) {
						float aimPercent = this.AimElapsed / TMRConfig.Instance.AimModeActivationThreshold;
						DebugHelpers.Print( "aim_up_idle", "aim%: "
							+ ( aimPercent * 100f ).ToString( "N0" )
							+ " (" + this.AimElapsed.ToString( "N1" ) + "), "
							+ TMRConfig.Instance.AimModeBuildupRateWhileIdle );
					}
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
