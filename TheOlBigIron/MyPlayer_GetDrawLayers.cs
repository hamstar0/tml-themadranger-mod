using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using HamstarHelpers.Helpers.Debug;


namespace TheOlBigIron {
	partial class TOBIPlayer : ModPlayer {
		private bool GetPlayerCustomArmLayers(
					Player plr,
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

			itemLayer = ( plrDrawInfo ) => { this.AddCustomPlayerItemLayers( plrDrawInfo, plrLight ); };
			armLayer = ( plrDrawInfo ) => { this.AddCustomPlayerArmLayers( plrDrawInfo ); };
			handLayer = ( plrDrawInfo ) => { this.AddCustomPlayerHandLayers( plrDrawInfo, plrLight ); };
			return true;
		}
	}
}
