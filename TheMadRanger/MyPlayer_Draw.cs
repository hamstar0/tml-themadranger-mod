using System;
using System.Collections.Generic;
using Terraria.ModLoader;


namespace TheMadRanger {
	partial class TMRPlayer : ModPlayer {
		public override void DrawEffects( PlayerDrawInfo drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright ) {
			if( TMRPlayer.IsHoldingGun( this.player ) ) {
				drawInfo.drawPlayer.bodyFrame.Y = TMRPlayer.AimGunForBodyFrameY( this.player );
				drawInfo.drawPlayer.hairFrame.Y = drawInfo.drawPlayer.bodyFrame.Y;
			}
		}

		public override void ModifyDrawInfo( ref PlayerDrawInfo drawInfo ) {
			if( TMRPlayer.IsHoldingGun( this.player ) ) {
				drawInfo.drawPlayer.bodyFrame.Y = TMRPlayer.AimGunForBodyFrameY( this.player );
				drawInfo.drawPlayer.hairFrame.Y = drawInfo.drawPlayer.bodyFrame.Y;
			}
		}


		////////////////

		public override void ModifyDrawLayers( List<PlayerLayer> layers ) {
			if( TMRPlayer.IsHoldingGun( this.player ) ) {
				(bool isAimWithinArc, int aimDir) aim = this.ApplyGunAim();

				if( !this.GunAnim.IsAnimating ) {
					if( aim.aimDir == this.player.direction || this.GunAnim.RecoilDuration == 0 ) {
						this.ModifyDrawLayersForGun( layers, true );
						this.player.headPosition.Y += 1;
					}
				}
			}

			this.GunAnim.ModifyDrawLayers( this.player, layers );
		}


		////////////////
		
		private bool ModifyDrawLayersForGun( List<PlayerLayer> layers, bool aimGun ) {
			PlayerLayer plrLayer;
			Action<PlayerDrawInfo> itemLayer, armLayer, handLayer;

			int newBodyFrameY;
			if( aimGun ) {
				newBodyFrameY = TMRPlayer.AimGunForBodyFrameY( this.player );
			} else {
				newBodyFrameY = this.player.bodyFrame.Height * 3;
			}

			if( !PlayerDraw.GetPlayerLayersForItemHolding( this.player, newBodyFrameY, out armLayer, out itemLayer, out handLayer ) ) {
				return false;
			}

			int itemLayerIdx = layers.FindIndex( lyr => lyr == PlayerLayer.HeldItem );
			int armLayerIdx = layers.FindIndex( lyr => lyr == PlayerLayer.Arms );
			int handLayerIdx = layers.FindIndex( lyr => lyr == PlayerLayer.HandOnAcc );

			if( itemLayerIdx != -1 ) {
				plrLayer = new PlayerLayer( "TheMadRanger", "Held Item", /*PlayerLayer.HeldItem,*/ itemLayer );
				layers.Insert( itemLayerIdx + 1, plrLayer );
			}
			if( armLayerIdx != -1 ) {
				plrLayer = new PlayerLayer( "TheMadRanger", "Item Holding Arm", /*PlayerLayer.Arms,*/ armLayer );
				layers.Insert( armLayerIdx+1, plrLayer );
			}
			if( handLayerIdx != -1 ) {
				plrLayer = new PlayerLayer( "TheMadRanger", "Item Holding Hand", /*PlayerLayer.HandOnAcc,*/ handLayer );
				layers.Insert( handLayerIdx+1, plrLayer );
			}

			PlayerLayer.HeldItem.visible = false;
			PlayerLayer.Arms.visible = false;
			PlayerLayer.HandOnAcc.visible = false;
			//PlayerLayer.HandOffAcc.visible = false;

			return true;
		}
	}
}
