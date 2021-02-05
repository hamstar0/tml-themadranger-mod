using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;
using HamstarHelpers.Helpers.Debug;


namespace TheMadRanger {
	partial class PlayerDraw {
		public static IEnumerable<DrawData> GetPlayerLayerForHand(
					PlayerDrawInfo plrDrawInfo,
					Color plrLight,
					Rectangle plrBodyFrame,
					float shadow=0f ) {
			DrawData drawData;

			Player plr = plrDrawInfo.drawPlayer;
			int itemType = plr.HeldItem.type;
			Texture2D itemTex = Main.itemTexture[itemType];

			Vector2 itemPos = plr.itemLocation;//plr.position + (plr.itemLocation - plr.position);
			itemPos.Y += plrDrawInfo.drawPlayer.gfxOffY;
			itemPos.Y *= plr.gravDir;

			// Hand:
			if( plr.handon > 0 ) {
				Vector2 pos = (plr.position - Main.screenPosition);//.Floor();
				pos += plr.bodyPosition;
				pos += new Vector2( plrBodyFrame.Width, plrBodyFrame.Height) * 0.5f;
				pos.Y += (plrBodyFrame.Height + 4) + plr.height;
				pos.Y += plr.gfxOffY;//plrDrawInfo.drawPlayer.gfxOffY;
				pos.Y *= plr.gravDir;

				drawData = new DrawData(
					texture: Main.accHandsOnTexture[(int)plr.handon],
					position: pos,
					sourceRect: plrBodyFrame,
					color: plrDrawInfo.middleArmorColor,
					rotation: plr.bodyRotation,
					origin: plrDrawInfo.bodyOrigin,
					scale: 1f,
					effect: plrDrawInfo.spriteEffects,
					inactiveLayerDepth: 0
				);
				drawData.shader = plrDrawInfo.handOnShader;
				//drawInfo.Draw( Main.spriteBatch );
				yield return drawData;
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
				Vector2 itemOrigin;

				if( plr.gravDir == -1f ) {
					itemOrigin = new Vector2(
						((float)itemTex.Width * 0.5f) - ((float)itemTex.Width * 0.5f * (float)plr.direction),
						0f
					);
				} else {
					itemOrigin = new Vector2(
						((float)itemTex.Width * 0.5f) - ((float)itemTex.Width * 0.5f * (float)plr.direction),
						itemTex.Height
					);
				}

				drawData = new DrawData(
					texture: itemTex,
					position: (itemPos - Main.screenPosition),
					sourceRect: new Rectangle( 0, 0, itemTex.Width, itemTex.Height ),
					color: plr.HeldItem.GetAlpha( plrLight ),
					rotation: plr.itemRotation,
					origin: itemOrigin,
					scale: plr.HeldItem.scale,
					effect: plrDrawInfo.spriteEffects,
					inactiveLayerDepth: 0
				);
				//drawInfo.Draw( Main.spriteBatch );
				yield return drawData;
			}
		}
	}
}
