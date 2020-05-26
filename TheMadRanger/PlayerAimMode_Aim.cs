using System;
using Microsoft.Xna.Framework;
using Terraria;
using HamstarHelpers.Helpers.Debug;


namespace TheMadRanger {
	partial class PlayerAimMode {
		private void UpdateEquippedAimStateValue( Player plr ) {
			if( !this.UpdateEquippedAimStateValueForPlayerMovement( plr ) ) {
				return;
			}

			if( !this.UpdateEquippedAimStateValueForMouseMovement() ) {
				return;
			}

			this.UpdateEquippedAimStateValueForPlayerIdle();
		}


		////////////////

		private bool UpdateEquippedAimStateValueForPlayerMovement( Player plr ) {
			if( plr.velocity.LengthSquared() <= 1f ) {
				return true;
			}

			// Player is moving
			if( TMRConfig.Instance.DebugModeInfo ) {
				float aimPercent = this.AimElapsed / TMRConfig.Instance.AimModeActivationThreshold;
				DebugHelpers.Print( "aim_down_move", "aim%: "
					+ ( aimPercent * 100f ).ToString( "N0" )
					+ " (" + this.AimElapsed.ToString( "N1" ) + "), "
					+ "-" + TMRConfig.Instance.AimModeDepleteRateWhilePlayerMoving );
			}

			this.AimElapsed = Math.Max( this.AimElapsed - TMRConfig.Instance.AimModeDepleteRateWhilePlayerMoving, 0f );
			return false;
		}


		private bool UpdateEquippedAimStateValueForMouseMovement() {
			var mousePos = new Vector2( Main.mouseX, Main.mouseY );

			// Mouse is not moving?
			if( (this.LastAimMousePosition - mousePos).LengthSquared() <= 1f ) {
				return false;
			}

			if( TMRConfig.Instance.DebugModeInfo ) {
				float aimPercent = this.AimElapsed / TMRConfig.Instance.AimModeActivationThreshold;
				DebugHelpers.Print( "aim_dec_mouse", "aim%: "
					+ ( aimPercent * 100f ).ToString( "N0" )
					+ " (" + this.AimElapsed.ToString( "N1" ) + "), "
					+ "-" + TMRConfig.Instance.AimModeDepleteRateWhileMouseMoving );
			}

			this.AimElapsed = Math.Max( this.AimElapsed - TMRConfig.Instance.AimModeDepleteRateWhileMouseMoving, 0f );

			this.LastAimMousePosition = mousePos;

			return true;
		}


		private void UpdateEquippedAimStateValueForPlayerIdle() {
			int activationThreshold = TMRConfig.Instance.AimModeActivationThreshold + 2;    // Added buffer for slight aim tweaks

			if( this.AimElapsed < activationThreshold ) {
				if( TMRConfig.Instance.DebugModeInfo ) {
					float aimPercent = this.AimElapsed / TMRConfig.Instance.AimModeActivationThreshold;
					DebugHelpers.Print( "aim_inc_idle", "aim%: "
						+ ( aimPercent * 100f ).ToString( "N0" )
						+ " (" + this.AimElapsed.ToString( "N1" ) + "), "
						+ TMRConfig.Instance.AimModeBuildupRateWhileIdle );
				}

				this.AimElapsed += TMRConfig.Instance.AimModeBuildupRateWhileIdle;
			}
		}
	}
}
