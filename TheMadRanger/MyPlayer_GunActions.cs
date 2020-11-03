using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using HamstarHelpers.Helpers.Debug;
using TheMadRanger.Items;
using TheMadRanger.Items.Weapons;


namespace TheMadRanger {
	partial class TMRPlayer : ModPlayer {
		public static bool IsHoldingGun( Player player ) {
			Item heldItem = player.HeldItem;
			return heldItem != null && !heldItem.IsAir && heldItem.type == ModContent.ItemType<TheMadRangerItem>();
		}

		////////////////

		public static int GetBodyFrameForItemAimAsIfForHeldGun( Player plr ) {
			int frameY;
			float rotDir = plr.itemRotation * (float)plr.direction;

			float minRot = -0.75f + 0.10472f;	//+6deg
			float maxRot = 0.6f - 0.174533f;	//-10deg

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



		////////////////

		public bool CanAttemptToShootGun() {
			return !this.GunHandling.IsHolstering
				&& (!this.GunHandling.IsReloading || this.GunHandling.ReloadingRounds)
				&& !SpeedloaderItem.IsReloading( this.player.whoAmI );
		}

		////////////////

		private (bool IsAimWithinArc, int AimDir) ApplyGunAim( int screenX, int screenY ) {
			Player plr = this.player;
			Texture2D itemTex = Main.itemTexture[plr.HeldItem.type];

			Vector2 plrCenter = plr.RotatedRelativePoint( plr.MountedCenter, true );

			float aimX = (float)screenX + Main.screenPosition.X - plrCenter.X;
			float aimY = (float)screenY + Main.screenPosition.Y - plrCenter.Y;
			if( plr.gravDir == -1f ) {
				aimY = Main.screenPosition.Y + (float)Main.screenHeight - (float)screenY - plrCenter.Y;
			}

			plr.itemRotation = (float)Math.Atan2(
				(double)( aimY * (float)plr.direction ),
				(double)( aimX * (float)plr.direction )
			) - plr.fullRotation;

			bool isAimWithinArc = true;

			if( plr.direction > 0 ) {
				float rot1 = 1.2f;
				float rot2 = -1.5f;

				if( plr.itemRotation > rot1 || plr.itemRotation < rot2 ) {
					float upTest = Math.Abs( plr.itemRotation - rot2 );
					float downTest = Math.Abs( plr.itemRotation - rot1 );

					if( upTest < downTest ) {
						plr.itemRotation = rot2;
					} else {
						plr.itemRotation = rot1;
					}
					isAimWithinArc = false;
				}
			} else {
				float rot1 = -1.2f;
				float rot2 = 1.5f;

				if( plr.itemRotation < rot1 || plr.itemRotation > rot2 ) {
					float upTest = Math.Abs( plr.itemRotation - rot2 );
					float downTest = Math.Abs( plr.itemRotation - rot1 );

					if( upTest < downTest ) {
						plr.itemRotation = rot2;
					} else {
						plr.itemRotation = rot1;
					}
					isAimWithinArc = false;
				}
			}

			float addedRotDeg = this.GunHandling.GetAddedRotationDegrees( plr );
			if( addedRotDeg != 0f ) {
				plr.itemRotation = MathHelper.ToDegrees( plr.itemRotation ) - (float)(plr.direction * addedRotDeg);
				plr.itemRotation = MathHelper.ToRadians( plr.itemRotation );
			} else {
				plr.itemRotation += this.AimMode.GetAimStateShakeAddedRadians( true );
			}

			plr.itemLocation.X = plr.position.X
				+ ( (float)plr.width * 0.5f )
				- ( itemTex.Width * 0.5f )
				- plr.direction * 2;
			plr.itemLocation.Y = plr.MountedCenter.Y
				- ( (float)itemTex.Height * 0.5f );

			ItemLoader.UseStyle( plr.HeldItem, plr );

			//NetMessage.SendData( MessageID.PlayerControls, -1, -1, null, plr.whoAmI, 0f, 0f, 0f, 0, 0, 0 );
			//NetMessage.SendData( MessageID.ItemAnimation, -1, -1, null, plr.whoAmI, 0f, 0f, 0f, 0, 0, 0 );

			return (isAimWithinArc, Math.Sign(aimX));
		}
	}
}
