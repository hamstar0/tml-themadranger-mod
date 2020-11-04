using System;
using Terraria;
using HamstarHelpers.Helpers.Debug;


namespace TheMadRanger.Logic {
	partial class PlayerLogic {
		public static void UpdatePlayerStateForAimMode( TMRPlayer myplayer ) {
			PlayerAimMode aimMode = myplayer.AimMode;
			GunHandling gunHandling = myplayer.GunHandling;

			if( aimMode.IsModeLocked || (aimMode.IsAttemptingModeLock && !gunHandling.IsAnimating) ) {
				var config = TMRConfig.Instance;
				float aimLockMoveSpeed = config.Get<float>( nameof(config.AimModeLockMoveSpeedScale) );

				myplayer.player.maxRunSpeed *= aimLockMoveSpeed;
				myplayer.player.accRunSpeed = myplayer.player.maxRunSpeed;
				myplayer.player.moveSpeed *= aimLockMoveSpeed;
			}
		}


		////////////////

		public static int GetBodyFrameForItemAimAsIfForHeldGun( Player plr ) {
			int frameY;
			float rotDir = plr.itemRotation * (float)plr.direction;

			float minRot = -0.75f + 0.10472f;   //+6deg
			float maxRot = 0.6f - 0.174533f;    //-10deg

			if( rotDir < minRot ) {
				if( plr.gravDir == -1f ) {
					frameY = plr.bodyFrame.Height * 4;
				} else {
					frameY = plr.bodyFrame.Height * 2;
				}
			} else if( rotDir > maxRot ) {
				if( plr.gravDir == -1f ) {
					frameY = plr.bodyFrame.Height * 2;
				} else {
					frameY = plr.bodyFrame.Height * 4;
				}
			} else {
				frameY = plr.bodyFrame.Height * 3;
			}

			return frameY;
		}
	}
}
