using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using HamstarHelpers.Helpers.Debug;
using TheOlBigIron.Items.Weapons;


namespace TheOlBigIron {
	partial class TOBIPlayer : ModPlayer {
		public static bool IsDrawingGun( Player player ) {
			return !player.HeldItem.IsAir && player.HeldItem.type == ModContent.ItemType<BigIronItem>();
		}

		public static int AimGunForBodyFrame( Player plr ) {
			int frameY;
			float rotDir = plr.itemRotation * (float)plr.direction;

			if( rotDir < -0.75f ) {
				if( plr.gravDir == -1f ) {
					frameY = plr.bodyFrame.Height * 4;
				} else {
					frameY = plr.bodyFrame.Height * 2;
				}
			} else if( rotDir > 0.6f ) {
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

		private void AimGun() {
			Player plr = this.player;
			Texture2D itemTex = Main.itemTexture[plr.HeldItem.type];

			Vector2 plrCenter = plr.RotatedRelativePoint( plr.MountedCenter, true );

			float rotX = (float)Main.mouseX + Main.screenPosition.X - plrCenter.X;
			float rotY = (float)Main.mouseY + Main.screenPosition.Y - plrCenter.Y;
			if( plr.gravDir == -1f ) {
				rotY = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY - plrCenter.Y;
			}

			plr.itemRotation = (float)Math.Atan2(
				(double)( rotY * (float)plr.direction ),
				(double)( rotX * (float)plr.direction )
			) - plr.fullRotation;

			if( this.Recoil <= 16 ) {
				plr.itemRotation += -plr.direction * MathHelper.ToRadians( this.Recoil );
			}

			this.IsFacingWrongWay = false;

			if( plr.direction > 0 ) {
				if( plr.itemRotation > 0.9f || plr.itemRotation < -1.5f ) {
					float upTest = Math.Abs( plr.itemRotation - -1.5f );
					float downTest = Math.Abs( plr.itemRotation - 0.9f );

					if( upTest < downTest ) {
						plr.itemRotation = -1.5f;
					} else {
						plr.itemRotation = 0.9f;
					}
					this.IsFacingWrongWay = true;
				}
			} else {
				if( plr.itemRotation < -0.9f || plr.itemRotation > 1.5f ) {
					float upTest = Math.Abs( plr.itemRotation - 1.5f );
					float downTest = Math.Abs( plr.itemRotation - -0.9f );

					if( upTest < downTest ) {
						plr.itemRotation = 1.5f;
					} else {
						plr.itemRotation = -0.9f;
					}
					this.IsFacingWrongWay = true;
				}
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
		}
	}
}
