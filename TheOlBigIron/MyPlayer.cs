using System;
using System.Collections.Generic;
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



		////////////////

		private int Recoil = 0;
		private bool IsFacingWrongWay = false;


		////////////////

		public override bool CloneNewInstances => false;



		////////////////

		public override void PreUpdate() {
			if( this.Recoil > 0 ) {
				this.Recoil--;
			}
		}


		////////////////

		public override bool Shoot( Item item, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack ) {
			if( TOBIPlayer.IsDrawingGun(this.player) ) {
				this.Recoil = 17;
			}
			return base.Shoot( item, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack );
		}


		////////////////
		
		public override void ModifyDrawLayers( List<PlayerLayer> layers ) {
			if( TOBIPlayer.IsDrawingGun(this.player) ) {
				this.AimGun();

				if( !this.IsFacingWrongWay || this.Recoil == 0 ) {
					this.AimGunForBodyFrame();
					this.ModifyDrawLayersForGun( layers );
				}
			}
		}

		////

		private void ModifyDrawLayersForGun( List<PlayerLayer> layers ) {
			PlayerLayer plrLayer;
			Action<PlayerDrawInfo> itemLayer, armLayer, handLayer;

			if( !this.GetPlayerCustomArmLayers(Main.LocalPlayer, out armLayer, out itemLayer, out handLayer) ) {
				return;
			}

			int itemLayerIdx = layers.FindIndex( ( lyr ) => lyr == PlayerLayer.HeldItem );
			int armLayerIdx = layers.FindIndex( (lyr) => lyr == PlayerLayer.Arms );
			int handLayerIdx = layers.FindIndex( (lyr) => lyr == PlayerLayer.HandOnAcc );

			if( itemLayerIdx != -1 ) {
				plrLayer = new PlayerLayer( "TheOlBigIron", "Held Item", /*PlayerLayer.HeldItem,*/ itemLayer );
				layers.Insert( itemLayerIdx + 1, plrLayer );
			}
			if( armLayerIdx != -1 ) {
				plrLayer = new PlayerLayer( "TheOlBigIron", "Item Holding Arm", /*PlayerLayer.Arms,*/ armLayer );
				layers.Insert( armLayerIdx+1, plrLayer );
			}
			if( handLayerIdx != -1 ) {
				plrLayer = new PlayerLayer( "TheOlBigIron", "Item Holding Hand", /*PlayerLayer.HandOnAcc,*/ handLayer );
				layers.Insert( handLayerIdx+1, plrLayer );
			}

			PlayerLayer.HeldItem.visible = false;
			PlayerLayer.Arms.visible = false;
			PlayerLayer.HandOnAcc.visible = false;
			//PlayerLayer.HandOffAcc.visible = false;
		}


		////////////////

		private void AimGun() {
			Player plr = this.player;
			Texture2D itemTex = Main.itemTexture[ plr.HeldItem.type ];

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
				+ ((float)plr.width * 0.5f)
				- (itemTex.Width * 0.5f)
				- plr.direction * 2;
			plr.itemLocation.Y = plr.MountedCenter.Y
				- ((float)itemTex.Height * 0.5f);

			ItemLoader.UseStyle( plr.HeldItem, plr );

			//NetMessage.SendData( MessageID.PlayerControls, -1, -1, null, plr.whoAmI, 0f, 0f, 0f, 0, 0, 0 );
			//NetMessage.SendData( MessageID.ItemAnimation, -1, -1, null, plr.whoAmI, 0f, 0f, 0f, 0, 0, 0 );
		}

		private void AimGunForBodyFrame() {
			Player plr = this.player;
			float rotDir = plr.itemRotation * (float)plr.direction;

			if( rotDir < -0.75f ) {
				if( plr.gravDir == -1f ) {
					plr.bodyFrame.Y = plr.bodyFrame.Height * 4;
				} else {
					plr.bodyFrame.Y = plr.bodyFrame.Height * 2;
				}
			} else if( rotDir > 0.6f ) {
				if( plr.gravDir == -1f ) {
					plr.bodyFrame.Y = plr.bodyFrame.Height * 2;
				} else {
					plr.bodyFrame.Y = plr.bodyFrame.Height * 4;
				}
			} else {
				plr.bodyFrame.Y = plr.bodyFrame.Height * 3;
			}
		}
	}
}
