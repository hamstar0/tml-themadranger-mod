using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using ModLibsNet.Services.Network;
using TheMadRanger.Logic;


namespace TheMadRanger {
	partial class TMRPlayer : ModPlayer {
		public override void FrameEffects() {
			if( this.player.whoAmI != Main.myPlayer ) {
				return;
			}

			if( PlayerLogic.IsUsingHeldGun(this.player) ) {
				int dir = (Main.MouseWorld.X > this.player.Center.X).ToDirectionInt();
				this.player.ChangeDir( dir );
			}
		}

		public override void DrawEffects( PlayerDrawInfo drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright ) {
			if( PlayerLogic.IsUsingHeldGun( this.player ) ) {
				if( !this.GunHandling.IsAnimating ) {
					drawInfo.drawPlayer.bodyFrame.Y = PlayerLogic.GetBodyFrameForItemAimAsIfForHeldGun( this.player );
				}
			}
			if( this.GunHandling.IsAnimating ) {
				drawInfo.drawPlayer.bodyFrame.Y = this.player.bodyFrame.Height * 3;
			}
		}

		public override void ModifyDrawInfo( ref PlayerDrawInfo drawInfo ) {
			if( PlayerLogic.IsUsingHeldGun( this.player ) ) {
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
			if( PlayerLogic.IsUsingHeldGun(this.player) ) {
				(bool isAimWithinArc, int aimDir) aim = PlayerLogic.ApplyGunAimFromCursorData( this );

				//

				if( !this.GunHandling.IsAnimating ) {
					if( aim.aimDir == this.player.direction || this.GunHandling.RecoilDuration == 0 ) {
						this.ModifyDrawLayersForGun_If( layers, true );

						//

						this.player.headPosition.Y += 1;
					}
				}
			}

			this.GunHandling.ModifyDrawLayersForGunAnim( this.player, layers );
		}


		////////////////
		
		private bool ModifyDrawLayersForGun_If( List<PlayerLayer> layers, bool aimGun ) {
			PlayerLayer plrLayer;
			Action<PlayerDrawInfo> armLayer, itemLayer, handLayer;

			int newBodyFrameY;
			if( aimGun ) {
				newBodyFrameY = PlayerLogic.GetBodyFrameForItemAimAsIfForHeldGun( this.player );
			} else {
				newBodyFrameY = this.player.bodyFrame.Height * 3;
			}

			//

			bool hasCustomItemHoldingDraws = PlayerDraw.GetPlayerLayersForItemHolding_If(
				this.player,
				newBodyFrameY,
				out armLayer,
				out itemLayer,
				out handLayer
			);
			if( !hasCustomItemHoldingDraws ) {
				return false;
			}

			//

			int armLayerIdx = layers.FindIndex( lyr => lyr == PlayerLayer.Arms );
			if( armLayerIdx != -1 ) {
				plrLayer = new PlayerLayer( "TheMadRanger", "Item Holding Arm", armLayer );
				layers.Insert( armLayerIdx+1, plrLayer );
			}

			int itemLayerIdx = layers.FindIndex( lyr => lyr == PlayerLayer.HeldItem );
			if( itemLayerIdx != -1 ) {
				plrLayer = new PlayerLayer( "TheMadRanger", "Held Item", itemLayer );
				layers.Insert( itemLayerIdx + 1, plrLayer );
			}

			int handLayerIdx = layers.FindIndex( lyr => lyr == PlayerLayer.HandOnAcc );
			if( handLayerIdx != -1 ) {
				plrLayer = new PlayerLayer( "TheMadRanger", "Item Holding Hand", handLayer );
				layers.Insert( handLayerIdx+1, plrLayer );
			}

			//

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
