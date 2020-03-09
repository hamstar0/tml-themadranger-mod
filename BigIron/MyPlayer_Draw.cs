using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using HamstarHelpers.Helpers.Debug;


namespace BigIron {
	partial class BigIronPlayer : ModPlayer {
		private bool ModifyDrawLayersForGun( List<PlayerLayer> layers ) {
			PlayerLayer plrLayer;
			Action<PlayerDrawInfo> itemLayer, armLayer, handLayer;

			if( !this.GetPlayerCustomArmLayers(this.player, out armLayer, out itemLayer, out handLayer) ) {
				return false;
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

			return true;
		}

		private void ModifyDrawLayerForTorsoWithGun( List<PlayerLayer> layers ) {
			int bodyLayerIdx = layers.FindIndex( ( lyr ) => lyr == PlayerLayer.Body );
			int skinLayerIdx = layers.FindIndex( ( lyr ) => lyr == PlayerLayer.Skin );
			if( bodyLayerIdx == -1 || skinLayerIdx == -1 ) { return; }

			Rectangle newFrame, oldFrame;
			newFrame = oldFrame = this.player.bodyFrame;
			newFrame.Y = BigIronPlayer.AimGunForBodyFrameY( this.player );

			Action<PlayerDrawInfo> preLayerAction = ( plrDrawInfo ) => {
				this.player.bodyFrame = newFrame;
			};
			Action<PlayerDrawInfo> postLayerAction = ( plrDrawInfo ) => {
				this.player.bodyFrame = oldFrame;
			};

			PlayerLayer preBodyLayer = new PlayerLayer( "TheOlBigIron", "Pre Torso Reframe", preLayerAction );
			PlayerLayer postBodyLayer = new PlayerLayer( "TheOlBigIron", "Post Torso Reframe", postLayerAction );
			PlayerLayer preSkinLayer = new PlayerLayer( "TheOlBigIron", "Pre Torso Skin Reframe", preLayerAction );
			PlayerLayer postSkinLayer = new PlayerLayer( "TheOlBigIron", "Post Torso Skin Reframe", postLayerAction );

			layers.Insert( bodyLayerIdx + 1, postBodyLayer );
			layers.Insert( bodyLayerIdx, preBodyLayer );
			layers.Insert( skinLayerIdx + 1, postSkinLayer );
			layers.Insert( skinLayerIdx, preSkinLayer );
		}
	}
}
