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

			bool isPreLocked = this.IsPreLocked;
			bool isPlrMoving = false;
			bool isMouseMoving = false;

			if( !isPreLocked ) {
				isPlrMoving = this.UpdateEquippedAimStateValueForPlayerMovement( plr );
				isMouseMoving = this.UpdateEquippedAimStateValueForMouseMovement();
			}

			if( isPreLocked || (!isPlrMoving && !isMouseMoving) ) {
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

			float aimMoveBuildup = TMRConfig.Instance.Get<float>( nameof(TMRConfig.AimModeOnPlayerMoveBuildupAmount) );

			// Player is moving
			if( TMRConfig.Instance.DebugModeInfo ) {
				DebugHelpers.Print( "aim_move", "aim%: "
					+ ( this.AimPercent * 100f ).ToString( "N0" )
					+ " (" + this.AimElapsed.ToString( "N1" ) + "), "
					+ aimMoveBuildup );
			}

			this.AimElapsed = Math.Max( this.AimElapsed + aimMoveBuildup, 0f );
			return true;
		}


		/// <summary></summary>
		/// <returns>`true` on mouse movement.</returns>
		private bool UpdateEquippedAimStateValueForMouseMovement() {
			var config = TMRConfig.Instance;
			var mousePos = new Vector2( Main.mouseX, Main.mouseY );
			float mouseThreshSqr = config.Get<float>( nameof(TMRConfig.AimModeMouseMoveThreshold) );
			mouseThreshSqr *= mouseThreshSqr;

			// Mouse is not moving?
			if( (this.LastAimMousePosition - mousePos).LengthSquared() <= mouseThreshSqr ) {
				return false;
			}

			float aimBuildupAmt = config.Get<float>( nameof(TMRConfig.AimModeOnMouseMoveBuildupAmount) );

			if( config.DebugModeInfo ) {
				DebugHelpers.Print( "aim_mouse", "aim%: "
					+ ( this.AimPercent * 100f ).ToString( "N0" )
					+ " (" + this.AimElapsed.ToString( "N1" ) + "), "
					+ aimBuildupAmt );
			}

			this.AimElapsed = Math.Max( this.AimElapsed + aimBuildupAmt, 0f );

			this.LastAimMousePosition = mousePos;

			return true;
		}


		private void UpdateEquippedAimStateValueForPlayerIdle() {
			var config = TMRConfig.Instance;
			int activationThreshold = config.Get<int>( nameof(TMRConfig.AimModeActivationTickDuration) ) + 2;   // Added buffer for slight aim tweaks
			float aimIdleBuildup = config.Get<float>( nameof( TMRConfig.AimModeOnIdleBuildupAmount ) );

			if( this.AimElapsed < activationThreshold ) {
				if( config.DebugModeInfo ) {
					DebugHelpers.Print( "aim_idle", "aim%: "
						+ ( this.AimPercent * 100f ).ToString( "N0" )
						+ " (" + this.AimElapsed.ToString( "N1" ) + "), "
						+ aimIdleBuildup );
				}

				this.AimElapsed += aimIdleBuildup;
			}
		}
	}
}
