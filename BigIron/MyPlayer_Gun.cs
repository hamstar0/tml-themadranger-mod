using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using HamstarHelpers.Helpers.Debug;
using BigIron.Items.Weapons;


namespace BigIron {
	partial class BigIronPlayer : ModPlayer {
		public static bool IsHoldingGun( Player player ) {
			return !player.HeldItem.IsAir && player.HeldItem.type == ModContent.ItemType<BigIronItem>();
		}

		public static int AimGunForBodyFrameY( Player plr ) {
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

		private (bool IsAimWithinArc, int AimDir) AimGun() {
			Player plr = this.player;
			Texture2D itemTex = Main.itemTexture[plr.HeldItem.type];

			Vector2 plrCenter = plr.RotatedRelativePoint( plr.MountedCenter, true );

			float aimX = (float)Main.mouseX + Main.screenPosition.X - plrCenter.X;
			float aimY = (float)Main.mouseY + Main.screenPosition.Y - plrCenter.Y;
			if( plr.gravDir == -1f ) {
				aimY = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY - plrCenter.Y;
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

			if( this.GunAnim.Recoil > 0 ) {
				if( this.GunAnim.Recoil <= 15 ) {
					plr.itemRotation = MathHelper.ToDegrees(plr.itemRotation) - (float)(plr.direction * this.GunAnim.Recoil);
					plr.itemRotation = MathHelper.ToRadians( plr.itemRotation );
					//plr.itemRotation += -plr.direction * MathHelper.ToRadians( this.GunAnim.Recoil );
				}
			} else {
				//1deg = 0.017453rad
				//15deg = 0.261799rad
				//-51deg = -0.890118rad

				//candidate 1: 36deg
				//candidate 2: 43deg
				//candidate 3: 51deg
				//candidate 4: -13deg
				//candidate 5: -22deg
				//candidate 6: -51deg
				//int rots = (int)( plr.itemRotation / -0.890118f );
				//plr.itemRotation = (float)rots * -0.890118f;
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
