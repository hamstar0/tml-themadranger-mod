﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;
using HamstarHelpers.Helpers.Debug;


namespace TheOlBigIron {
	partial class TOBIPlayer : ModPlayer {
		private void AddCustomPlayerHandLayers( PlayerDrawInfo plrDrawInfo, Color plrLight, float shadow=0f ) {
			DrawData drawInfo;

			Player plr = plrDrawInfo.drawPlayer;
			int itemType = plr.HeldItem.type;
			Texture2D itemTex = Main.itemTexture[itemType];
			Vector2 itemPos = plr.position + ( plr.itemLocation - plr.position );

			// Hand:
			if( plr.handon > 0 ) {
				Vector2 pos = (plr.position - Main.screenPosition).Floor();
				pos += plr.bodyPosition;
				pos += new Vector2(plr.bodyFrame.Width, plr.bodyFrame.Height) * 0.5f;
				pos.X += (plr.bodyFrame.Width / 2) - plr.bodyFrame.Width / 2;
				pos.Y += (plr.bodyFrame.Height + 4) + plr.height;

				drawInfo = new DrawData(
					Main.accHandsOnTexture[(int)plr.handon],
					pos,
					plr.bodyFrame,
					plrDrawInfo.middleArmorColor,
					plr.bodyRotation,
					plrDrawInfo.bodyOrigin,
					1f,
					plrDrawInfo.spriteEffects,
					0
				);
				drawInfo.shader = plrDrawInfo.handOnShader;
				//drawInfo.Draw( Main.spriteBatch );
				Main.playerDrawData.Add( drawInfo );
			}

			bool canDrawClawItem = !plr.HeldItem.IsAir
				&& Item.claw[itemType]
				&& shadow == 0f
				&& !plr.frozen
				&& !plr.dead
				&& !plr.HeldItem.noUseGraphic
				&& (!plr.wet || !plr.HeldItem.noWet)
				&& (plr.itemAnimation > 0 || (plr.HeldItem.holdStyle > 0 && !plr.pulley));

			if( canDrawClawItem ) {
				Vector2 origin;

				if( plr.gravDir == -1f ) {
					origin = new Vector2( ((float)itemTex.Width * 0.5f) - ((float)itemTex.Width * 0.5f * (float)plr.direction), 0f );
				} else {
					origin = new Vector2( ((float)itemTex.Width * 0.5f) - ((float)itemTex.Width * 0.5f * (float)plr.direction), itemTex.Height );
				}

				drawInfo = new DrawData(
					itemTex,
					(itemPos - Main.screenPosition).Floor(),
					new Rectangle( 0, 0, itemTex.Width, itemTex.Height ),
					plr.HeldItem.GetAlpha( plrLight ),
					plr.itemRotation,
					origin,
					plr.HeldItem.scale,
					plrDrawInfo.spriteEffects,
					0
				);
				//drawInfo.Draw( Main.spriteBatch );
				Main.playerDrawData.Add( drawInfo );
			}
		}
	}
}
