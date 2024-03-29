﻿using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.DataStructures;
using ModLibsCore.Libraries.Debug;


namespace TheMadRanger {
	partial class PlayerDraw {
		public static bool GetPlayerLayersForItemHolding_If(
					Player plr,
					int newBodyFrameY,
					out Action<PlayerDrawInfo> armLayer,
					out Action<PlayerDrawInfo> itemLayer,
					out Action<PlayerDrawInfo> handLayer ) {
			armLayer = itemLayer = handLayer = null;

			if( plr.frozen || plr.dead ) {
				return false;
			}
			if( plr.HeldItem.IsAir ) {
				return false;
			}

			//

			int lightTileX = (int)( ( plr.position.X + ( (float)plr.width * 0.5f ) ) / 16f );
			int lightTileY = (int)( ( plr.position.Y + ( (float)plr.height * 0.5f ) ) / 16f );
			Color plrLight = Lighting.GetColor( lightTileX, lightTileY );
			ItemSlot.GetItemLight( ref plrLight, plr.HeldItem, false );

			Rectangle newFrame, oldFrame;
			newFrame = oldFrame = plr.bodyFrame;
			newFrame.Y = newBodyFrameY;
			
			armLayer = ( plrDrawInfo ) => {
				foreach( DrawData drawData in PlayerDraw.GetPlayerLayerForArms(plrDrawInfo, newFrame) ) {
					Main.playerDrawData.Add( drawData );
				}
			};
			itemLayer = ( plrDrawInfo ) => {
				foreach( DrawData drawData in PlayerDraw.GetPlayerLayerForHeldItem(plrDrawInfo, plrLight) ) {
					Main.playerDrawData.Add( drawData );
				}
			};
			handLayer = ( plrDrawInfo ) => {
				foreach( DrawData drawData in PlayerDraw.GetPlayerLayerDataForHand(plrDrawInfo, plrLight, newFrame) ) {
					Main.playerDrawData.Add( drawData );
				}
			};

			return true;
		}
	}
}
