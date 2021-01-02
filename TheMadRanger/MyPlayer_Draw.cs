using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using HamstarHelpers.Services.Network;
using TheMadRanger.Logic;


namespace TheMadRanger {
	partial class TMRPlayer : ModPlayer {
		public override void DrawEffects( PlayerDrawInfo drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright ) {
			if( PlayerLogic.IsHoldingGun( this.player ) ) {
				if( !this.GunHandling.IsAnimating ) {
					drawInfo.drawPlayer.bodyFrame.Y = PlayerLogic.GetBodyFrameForItemAimAsIfForHeldGun( this.player );
				}
			}
			if( this.GunHandling.IsAnimating ) {
				drawInfo.drawPlayer.bodyFrame.Y = this.player.bodyFrame.Height * 3;
			}
		}

		public override void ModifyDrawInfo( ref PlayerDrawInfo drawInfo ) {
			if( PlayerLogic.IsHoldingGun( this.player ) ) {
				if( !this.GunHandling.IsAnimating ) {
					drawInfo.drawPlayer.bodyFrame.Y = PlayerLogic.GetBodyFrameForItemAimAsIfForHeldGun( this.player );
				}
			}
			if( this.GunHandling.IsAnimating ) {
				drawInfo.drawPlayer.bodyFrame.Y = this.player.bodyFrame.Height * 3;
			}
		}


		////////////////

		public override void ModifyDrawLayers( List<PlayerLayer> layers ) {
			if( PlayerLogic.IsHoldingGun( this.player ) ) {
				(bool isAimWithinArc, int aimDir) aim;

				if( this.player.whoAmI == Main.myPlayer ) {
					aim = PlayerLogic.ApplyGunAim( this, Main.mouseX, Main.mouseY );
				} else {
					(int x, int y) cursor;
					if( Client.LastKnownCursorPositions.ContainsKey(this.player.whoAmI) ) {
						cursor = Client.LastKnownCursorPositions[ this.player.whoAmI ];
					} else {
						cursor = ((int)this.player.MountedCenter.X, (int)this.player.MountedCenter.Y);
						cursor.x += this.player.direction * 256;
					}

					aim = PlayerLogic.ApplyGunAim( this, cursor.x, cursor.y );
				}

				if( !this.GunHandling.IsAnimating ) {
					if( aim.aimDir == this.player.direction || this.GunHandling.RecoilDuration == 0 ) {
						this.ModifyDrawLayersForGun( layers, true );
						this.player.headPosition.Y += 1;
					}
				}
			}

			this.GunHandling.ModifyDrawLayers( this.player, layers );
		}


		////////////////
		
		private bool ModifyDrawLayersForGun( List<PlayerLayer> layers, bool aimGun ) {
			PlayerLayer plrLayer;
			Action<PlayerDrawInfo> itemLayer, armLayer, handLayer;

			int newBodyFrameY;
			if( aimGun ) {
				newBodyFrameY = PlayerLogic.GetBodyFrameForItemAimAsIfForHeldGun( this.player );
			} else {
				newBodyFrameY = this.player.bodyFrame.Height * 3;
			}

			//

			if( !PlayerDraw.GetPlayerLayersForItemHolding(this.player, newBodyFrameY, out armLayer, out itemLayer, out handLayer) ) {
				return false;
			}

			//

			int itemLayerIdx = layers.FindIndex( lyr => lyr == PlayerLayer.HeldItem );
			if( itemLayerIdx != -1 ) {
				plrLayer = new PlayerLayer( "TheMadRanger", "Held Item", itemLayer );
				layers.Insert( itemLayerIdx + 1, plrLayer );
			}
			int armLayerIdx = layers.FindIndex( lyr => lyr == PlayerLayer.Arms );
			if( armLayerIdx != -1 ) {
				plrLayer = new PlayerLayer( "TheMadRanger", "Item Holding Arm", armLayer );
				layers.Insert( armLayerIdx+1, plrLayer );
			}
			int handLayerIdx = layers.FindIndex( lyr => lyr == PlayerLayer.HandOnAcc );
			if( handLayerIdx != -1 ) {
				plrLayer = new PlayerLayer( "TheMadRanger", "Item Holding Hand", handLayer );
				layers.Insert( handLayerIdx+1, plrLayer );
			}

			PlayerLayer.HeldItem.visible = false;
			PlayerLayer.Arms.visible = false;
			PlayerLayer.HandOnAcc.visible = false;
			PlayerLayer.HandOffAcc.visible = false;
			this.player.handon = 0;
			this.player.handoff = 0;

			return true;
		}
	}
}
