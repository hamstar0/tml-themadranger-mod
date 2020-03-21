using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using HamstarHelpers.Helpers.Debug;


namespace TheMadRanger {
	partial class TMRPlayer : ModPlayer {
		public override void ModifyDrawLayers( List<PlayerLayer> layers ) {
			if( TMRPlayer.IsHoldingGun( this.player ) ) {
				(bool isAimWithinArc, int aimDir) aim = this.ApplyGunAim();

				if( !this.GunAnim.IsAnimating ) {
					if( aim.aimDir == this.player.direction || this.GunAnim.RecoilDuration == 0 ) {
						if( this.ModifyDrawLayersForGun( layers, true ) ) {
							this.ModifyDrawLayerForHeadAndTorsoWithGun( layers, true );
						}

						this.player.headPosition.Y += 1;
					}
				}
			}

			this.GunAnim.ModifyDrawLayers( this.player, layers );
		}


		/*public override void ModifyDrawHeadLayers( List<PlayerHeadLayer> layers ) {
			if( !TMRPlayer.IsHoldingGun(this.player) ) {
				return;
			}

			Rectangle newHeadFrame,  oldHeadFrame;
			Rectangle newBodyFrame,  oldBodyFrame;
			newHeadFrame = oldHeadFrame = this.player.headFrame;
			newBodyFrame = oldBodyFrame = this.player.bodyFrame;
			newHeadFrame.Y = 0;
			newBodyFrame.Y = 0;

			Action<PlayerHeadDrawInfo> preLayerAction = ( plrHeadDrawInfo ) => {
				this.player.headPosition = Vector2.Zero;
//				this.player.headFrame = newHeadFrame;
//				this.player.hairFrame = newHeadFrame;
				this.player.bodyFrame = newBodyFrame;
			};
			Action<PlayerHeadDrawInfo> postLayerAction = ( plrHeadDrawInfo ) => {
//				this.player.headFrame = oldHeadFrame;
//				this.player.hairFrame = oldHeadFrame;
				this.player.bodyFrame = oldBodyFrame;
			};

			var preHeadLayer = new PlayerHeadLayer( "TheMadRanger", "Pre Head Reframe", preLayerAction );
			var postHeadLayer = new PlayerHeadLayer( "TheMadRanger", "Post Head Reframe", postLayerAction );

			layers.Insert( 0, preHeadLayer );
			layers.Add( postHeadLayer );
		}*/


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

		private void ModifyDrawLayerForHeadAndTorsoWithGun( List<PlayerLayer> layers, bool aimGun ) {
			int headLayerIdx = layers.FindIndex( lyr => lyr == PlayerLayer.Head );
			int faceLayerIdx = layers.FindIndex( lyr => lyr == PlayerLayer.Face );
			int hairLayerIdx = layers.FindIndex( lyr => lyr == PlayerLayer.Hair );
			int bodyLayerIdx = layers.FindIndex( lyr => lyr == PlayerLayer.Body );
			int skinLayerIdx = layers.FindIndex( lyr => lyr == PlayerLayer.Skin );

			Rectangle newBodyFrame, oldBodyFrame;
			newBodyFrame = oldBodyFrame = this.player.bodyFrame;
			
//			if( aimGun ) {
				newBodyFrame.Y = TMRPlayer.AimGunForBodyFrameY( this.player );
//			} else {
//				newBodyFrame.Y = this.player.bodyFrame.Height * 3;
//			}

			Action<PlayerDrawInfo> preLayerAction = ( plrDrawInfo ) => {
//				this.player.headPosition = Vector2.Zero;
//				this.player.hairFrame = newBodyFrame;
//				this.player.headFrame = newBodyFrame;
				this.player.bodyFrame = newBodyFrame;
//				this.player.hairFrame.Y = 0;
//				this.player.headFrame.Y = 0;
			};
			Action<PlayerDrawInfo> postLayerAction = ( plrDrawInfo ) => {
//				this.player.hairFrame = oldBodyFrame;
//				this.player.headFrame = oldBodyFrame;
				this.player.bodyFrame = oldBodyFrame;
			};


//			if( headLayerIdx != -1 ) {
//				var preLayer = new PlayerLayer( "TheMadRanger", "Pre Head Reframe", preLayerAction );
//				var postLayer = new PlayerLayer( "TheMadRanger", "Post Head Reframe", postLayerAction );
//				layers.Insert( headLayerIdx + 1, postLayer );
//				layers.Insert( headLayerIdx, preLayer );
//			}
			if( faceLayerIdx != -1 ) {
				var preLayer = new PlayerLayer( "TheMadRanger", "Pre Face Reframe", preLayerAction );
				var postLayer = new PlayerLayer( "TheMadRanger", "Post Face Reframe", postLayerAction );
				layers.Insert( faceLayerIdx + 1, postLayer );
				layers.Insert( faceLayerIdx, preLayer );
			}
//			if( hairLayerIdx != -1 ) {
//				var preLayer = new PlayerLayer( "TheMadRanger", "Pre Hair Reframe", preLayerAction );
//				var postLayer = new PlayerLayer( "TheMadRanger", "Post Hair Reframe", postLayerAction );
//				layers.Insert( hairLayerIdx + 1, postLayer );
//				layers.Insert( hairLayerIdx, preLayer );
//			}
			if( bodyLayerIdx != -1 ) {
				var preLayer = new PlayerLayer( "TheMadRanger", "Pre Torso Reframe", preLayerAction );
				var postLayer = new PlayerLayer( "TheMadRanger", "Post Torso Reframe", postLayerAction );
				layers.Insert( bodyLayerIdx + 1, postLayer );
				layers.Insert( bodyLayerIdx, preLayer );
			}
			if( skinLayerIdx != -1 ) {
				var preLayer = new PlayerLayer( "TheMadRanger", "Pre Torso Skin Reframe", preLayerAction );
				var postLayer = new PlayerLayer( "TheMadRanger", "Post Torso Skin Reframe", postLayerAction );
				layers.Insert( skinLayerIdx + 1, postLayer );
				layers.Insert( skinLayerIdx, preLayer );
			}
		}
	}
}
