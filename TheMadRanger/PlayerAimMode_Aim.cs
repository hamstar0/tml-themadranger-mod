using System;
using Microsoft.Xna.Framework;
using Terraria;
using HamstarHelpers.Helpers.Debug;


namespace TheMadRanger {
	partial class PlayerAimMode {
		private void UpdateEquippedAimStateValue( Player plr ) {
			if( this.IsLocked ) {
				return;
			}

			bool isPlrMoving = this.UpdateEquippedAimStateValueForPlayerMovement( plr );
			bool isMouseMoving = this.UpdateEquippedAimStateValueForMouseMovement();

			if( !isPlrMoving && !isMouseMoving ) {
				this.UpdateEquippedAimStateValueForPlayerIdle();
			}
		}


		////////////////

		/// <summary></summary>
		/// <returns>`true` on player movement.</returns>
		private bool UpdateEquippedAimStateValueForPlayerMovement( Player plr ) {
			if( plr.velocity.LengthSquared() <= 1f ) {
				return false;
			}

			// Player is moving
			if( TMRConfig.Instance.DebugModeInfo ) {
				DebugHelpers.Print( "aim_move", "aim%: "
					+ ( this.AimPercent * 100f ).ToString( "N0" )
					+ " (" + this.AimElapsed.ToString( "N1" ) + "), "
					+ TMRConfig.Instance.AimModeOnPlayerMoveBuildupAmount );
			}

			this.AimElapsed = Math.Max( this.AimElapsed + TMRConfig.Instance.AimModeOnPlayerMoveBuildupAmount, 0f );
			return true;
		}


		/// <summary></summary>
		/// <returns>`true` on mouse movement.</returns>
		private bool UpdateEquippedAimStateValueForMouseMovement() {
			var mousePos = new Vector2( Main.mouseX, Main.mouseY );
			float mouseThreshSqr = TMRConfig.Instance.AimModeMouseMoveThreshold;
			mouseThreshSqr *= mouseThreshSqr;

			// Mouse is not moving?
			if( (this.LastAimMousePosition - mousePos).LengthSquared() <= mouseThreshSqr ) {
				return false;
			}

			if( TMRConfig.Instance.DebugModeInfo ) {
				DebugHelpers.Print( "aim_mouse", "aim%: "
					+ ( this.AimPercent * 100f ).ToString( "N0" )
					+ " (" + this.AimElapsed.ToString( "N1" ) + "), "
					+ TMRConfig.Instance.AimModeOnMouseMoveBuildupAmount );
			}

			this.AimElapsed = Math.Max( this.AimElapsed + TMRConfig.Instance.AimModeOnMouseMoveBuildupAmount, 0f );

			this.LastAimMousePosition = mousePos;

			return true;
		}


		private void UpdateEquippedAimStateValueForPlayerIdle() {
			int activationThreshold = TMRConfig.Instance.AimModeActivationTickDuration + 2;    // Added buffer for slight aim tweaks

			if( this.AimElapsed < activationThreshold ) {
				if( TMRConfig.Instance.DebugModeInfo ) {
					DebugHelpers.Print( "aim_idle", "aim%: "
						+ ( this.AimPercent * 100f ).ToString( "N0" )
						+ " (" + this.AimElapsed.ToString( "N1" ) + "), "
						+ TMRConfig.Instance.AimModeOnIdleBuildupAmount );
				}

				this.AimElapsed += TMRConfig.Instance.AimModeOnIdleBuildupAmount;
			}
		}
	}
}
