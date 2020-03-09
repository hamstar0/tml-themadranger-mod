using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.DataStructures;
using HamstarHelpers.Helpers.Debug;


namespace BigIron {
	partial class BigIronPlayer : ModPlayer {
		private bool GetPlayerCustomArmLayers(
					Player plr,
					int newBodyFrameY,
					out Action<PlayerDrawInfo> armLayer,
					out Action<PlayerDrawInfo> itemLayer,
					out Action<PlayerDrawInfo> handLayer ) {
			armLayer = itemLayer = handLayer = null;

			if( plr.frozen || plr.dead ) { return false; }
			if( plr.HeldItem.IsAir ) { return false; }

			int lightTileX = (int)( ( plr.position.X + ( (float)plr.width * 0.5f ) ) / 16f );
			int lightTileY = (int)( ( plr.position.Y + ( (float)plr.height * 0.5f ) ) / 16f );
			Color plrLight = Lighting.GetColor( lightTileX, lightTileY );
			ItemSlot.GetItemLight( ref plrLight, plr.HeldItem, false );

			Rectangle newFrame, oldFrame;
			newFrame = oldFrame = plr.bodyFrame;
			newFrame.Y = newBodyFrameY;

			itemLayer = ( plrDrawInfo ) => {
				foreach( DrawData drawData in this.AddCustomPlayerItemLayers(plrDrawInfo, plrLight) ) {
					Main.playerDrawData.Add( drawData );
				}
			};
			armLayer = ( plrDrawInfo ) => {
				foreach( DrawData drawData in this.AddCustomPlayerArmLayers(plrDrawInfo, newFrame) ) {
					Main.playerDrawData.Add( drawData );
				}
			};
			handLayer = ( plrDrawInfo ) => {
				foreach( DrawData drawData in this.AddCustomPlayerHandLayers(plrDrawInfo, plrLight, newFrame) ) {
					Main.playerDrawData.Add( drawData );
				}
			};
			return true;
		}
	}
}
