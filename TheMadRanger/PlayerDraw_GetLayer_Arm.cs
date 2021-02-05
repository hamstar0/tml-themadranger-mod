using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.Players;


namespace TheMadRanger {
	partial class PlayerDraw {
		public static IEnumerable<DrawData> GetPlayerLayerForArms( PlayerDrawInfo plrDrawInfo, Rectangle plrBodyFrame ) {
			Player plr = plrDrawInfo.drawPlayer;
			DrawData drawInfo;

			//

			DrawData getDrawData( Texture2D tex, Vector2 mypos, Color color, Rectangle frame ) {
				return new DrawData(
					tex,
					mypos,
					frame,
					color,
					plr.bodyRotation,
					plrDrawInfo.bodyOrigin,
					1f,
					plrDrawInfo.spriteEffects,
					0
				);
			}

			//

			var pos = plr.position - Main.screenPosition;
			pos.X = (int)( pos.X - (plrBodyFrame.Width / 2) + (plr.width / 2) );
			pos.Y = (int)( pos.Y + plr.height - plrBodyFrame.Height + 4 );
			pos.Y += plrDrawInfo.drawPlayer.gfxOffY;
			pos.Y *= plrDrawInfo.drawPlayer.gravDir;
			pos += plr.bodyPosition;
			pos += new Vector2( plrBodyFrame.Width / 2, plrBodyFrame.Height / 2 );

			if( plr.body > 0 && (!plr.invis || (plr.body != 21 && plr.body != 22)) ) {
				int bodyFrameXOffset = 0;
				if( !PlayerAppearanceHelpers.IsPlayerBackSlotFilled(plr) && plr.back > 0 && !plr.mount.Active && plr.front >= 1 ) {
					int bodyFrameIdx = plrBodyFrame.Y / 56;
					if( bodyFrameIdx < 1 || bodyFrameIdx > 5 ) {
						bodyFrameXOffset = 10;
					} else if( plr.front == 2 || plr.front == 4 ) {
						bodyFrameXOffset = 8;
					}
				}

				Rectangle offsetBodyFrame = plrBodyFrame;
				offsetBodyFrame.X += bodyFrameXOffset;
				offsetBodyFrame.Width -= bodyFrameXOffset;

				if( plr.direction == -1 ) {
					bodyFrameXOffset = 0;
				}

				if( plrDrawInfo.drawHands && !plr.invis ) {
					if( plrDrawInfo.drawArms ) {
						drawInfo = getDrawData( Main.playerTextures[plr.skinVariant, 7], pos, plrDrawInfo.bodyColor, plrBodyFrame );
						//drawInfo.Draw( Main.spriteBatch );
						yield return drawInfo;
					}

					drawInfo = getDrawData( Main.playerTextures[plr.skinVariant, 9], pos, plrDrawInfo.bodyColor, plrBodyFrame );
					//drawInfo.Draw( Main.spriteBatch );
					yield return drawInfo;
				}

				pos.X += bodyFrameXOffset;

				drawInfo = getDrawData( Main.armorArmTexture[plr.body], pos, plrDrawInfo.middleArmorColor, offsetBodyFrame );
				drawInfo.shader = plrDrawInfo.bodyArmorShader;
				//drawInfo.Draw( Main.spriteBatch );
				yield return drawInfo;

				if( plrDrawInfo.armGlowMask != -1 ) {
					drawInfo = getDrawData( Main.glowMaskTexture[plrDrawInfo.armGlowMask], pos, plrDrawInfo.legGlowMaskColor, offsetBodyFrame );
					drawInfo.shader = plrDrawInfo.bodyArmorShader;
					//drawInfo.Draw( Main.spriteBatch );
					yield return drawInfo;
				}
			} else if( !plr.invis ) {
				drawInfo = getDrawData( Main.playerTextures[plr.skinVariant, 7], pos, plrDrawInfo.bodyColor, plrBodyFrame );
				//drawInfo.Draw( Main.spriteBatch );
				yield return drawInfo;

				drawInfo = getDrawData( Main.playerTextures[plr.skinVariant, 8], pos, plrDrawInfo.underShirtColor, plrBodyFrame );
				//drawInfo.Draw( Main.spriteBatch );
				yield return drawInfo;

				drawInfo = getDrawData( Main.playerTextures[plr.skinVariant, 13], pos, plrDrawInfo.shirtColor, plrBodyFrame );
				//drawInfo.Draw( Main.spriteBatch );
				yield return drawInfo;
			}
		}
	}
}
