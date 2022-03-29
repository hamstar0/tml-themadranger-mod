using System;
using Microsoft.Xna.Framework;
using Terraria;
using ModLibsCore.Libraries.Debug;


namespace TheMadRanger.Logic {
	partial class PlayerAimMode {
		private bool UpdateEquippedAimStateValue_Local_If( Player player ) {
			if( player.whoAmI != Main.myPlayer ) {
				return false;
			}

			if( this.IsModeLocked_LocalOnly ) {
				return false;
			}

			//

			bool isAimingLocked = this.IsApplyingModeLock_LocalOnly;
			bool isPlrMoving = false;
			bool isMouseMoving = false;

			if( !isAimingLocked ) {
				isPlrMoving = this.UpdateEquippedAimStateValueForPlayerMovement_If( player );
				
				if( !this.UpdateEquippedAimStateValueForMouseMovement_Local_If(player, out isMouseMoving) ) {
					isMouseMoving = true;
				}
			}

			//

			if( isAimingLocked || (!isPlrMoving && !isMouseMoving) ) {
				this.UpdateEquippedAimStateValueForPlayerIdle( player );
			}

			return true;
		}


		////////////////

		/// <summary></summary>
		/// <returns>`true` on player movement.</returns>
		private bool UpdateEquippedAimStateValueForPlayerMovement_If( Player plr ) {
			if( plr.velocity.LengthSquared() <= 1f ) {
				return false;
			}

			//

			var config = TMRConfig.Instance;
			float aimMoveBuildup = config.Get<float>( nameof(config.AimModeOnPlayerMoveBuildupAmount) );

			//

			// Player is moving
			if( config.DebugModeGunInfo && plr.whoAmI == Main.myPlayer ) {
				DebugLibraries.Print( "aim_move", "aim%: "
					+ (this.AimPercent * 100f).ToString( "N0" )
					+ " (" + this.AimElapsed.ToString( "N1" ) + "), "
					+ aimMoveBuildup
				);
			}

			//

			this.AimElapsed = Math.Max( this.AimElapsed + aimMoveBuildup, 0f );

			return true;
		}


		/// <summary></summary>
		/// <param name="player"></param>
		/// <param name="isMouseMoving"></param>
		/// <returns>`true` if aim state was allowed to be computed.</returns>
		private bool UpdateEquippedAimStateValueForMouseMovement_Local_If(
					Player player,
					out bool isMouseMoving ) {
			if( player.whoAmI != Main.myPlayer ) {
				isMouseMoving = false;
				return false;
			}

			//

			var config = TMRConfig.Instance;
			var mousePos = new Vector2( Main.mouseX, Main.mouseY );
			float mouseThreshSqr = config.Get<float>( nameof(config.AimModeMouseMoveThreshold) );
			mouseThreshSqr *= mouseThreshSqr;

			// Mouse is not moving?
			if( (this.PrevAimMousePosition - mousePos).LengthSquared() <= mouseThreshSqr ) {
				isMouseMoving = false;
				return false;
			}

			float aimBuildupAmt = config.Get<float>( nameof(config.AimModeOnMouseMoveBuildupAmount) );

			//

			if( config.DebugModeGunInfo && player.whoAmI == Main.myPlayer ) {
				DebugLibraries.Print( "aim_mouse", "aim%: "
					+ (this.AimPercent * 100f).ToString( "N0" )
					+ " (" + this.AimElapsed.ToString( "N1" ) + "), "
					+ aimBuildupAmt
				);
			}

			//

			this.AimElapsed = Math.Max( this.AimElapsed + aimBuildupAmt, 0f );

			this.PrevAimMousePosition = mousePos;

			//

			isMouseMoving = true;
			return true;
		}


		private void UpdateEquippedAimStateValueForPlayerIdle( Player player ) {
			var config = TMRConfig.Instance;
			int activationThreshold = config.Get<int>( nameof(config.AimModeActivationTickDuration) ) + 2;   // Added buffer for slight aim tweaks
			float aimIdleBuildup = config.Get<float>( nameof(config.AimModeOnIdleBuildupAmount) );

			if( this.AimElapsed < activationThreshold ) {
				if( config.DebugModeGunInfo && player.whoAmI == Main.myPlayer ) {
					DebugLibraries.Print( "aim_idle", "aim%: "
						+ ( this.AimPercent * 100f ).ToString( "N0" )
						+ " (" + this.AimElapsed.ToString( "N1" ) + "), "
						+ aimIdleBuildup
					);
				}

				//

				this.AimElapsed += aimIdleBuildup;
			}
		}
	}
}
