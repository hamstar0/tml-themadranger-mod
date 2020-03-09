using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.DataStructures;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.DotNET.Reflection;


namespace BigIron {
	partial class BigIronPlayer : ModPlayer {
		public static Color GetItemLightColor( Player plr, Color plrLight ) {
			Color itemLight = plrLight;
			float stealthPercent = plr.stealth < 0.03f
				? plr.stealth
				: 0.03f;

			if( plr.shroomiteStealth && plr.HeldItem.ranged ) {
				float stealthGlowPercent = ( 1f + ( stealthPercent * 10f ) ) / 11f;

				itemLight = new Color(
					(byte)( (float)itemLight.R * stealthPercent ),
					(byte)( (float)itemLight.G * stealthPercent ),
					(byte)( (float)itemLight.B * stealthGlowPercent ),
					(byte)( (float)itemLight.A * stealthPercent )
				);
			}

			if( plr.setVortex && plr.HeldItem.ranged ) {
				itemLight = itemLight.MultiplyRGBA(
					new Color(
						Vector4.Lerp(
							Vector4.One,
							new Vector4( 0f, 0.12f, 0.16f, 0f ),
							1f - stealthPercent
						)
					)
				);
			}
			ItemSlot.GetItemLight( ref itemLight, plr.HeldItem, false );

			return itemLight;
		}



		////////////////

		private void AddCustomPlayerItemLayers( PlayerDrawInfo plrDrawInfo, Color plrLight, float shadow = 0f ) {
			DrawData drawInfo;

			Player plr = plrDrawInfo.drawPlayer;
			Color itemLight = BigIronPlayer.GetItemLightColor( plr, plrLight );

			Vector2 itemScrPos;
			ReflectionHelpers.RunMethod(
				Main.instance,
				"DrawPlayerItemPos",
				new object[] { plr.gravDir, plr.HeldItem.type },
				out itemScrPos
			);

			Texture2D itemTex = Main.itemTexture[plr.HeldItem.type];
			var itemTexOffset = new Vector2( itemTex.Width / 2, itemScrPos.Y );

			Vector2 itemWldPos = plr.itemLocation;//plr.position + (plr.itemLocation - plr.position);
			Vector2 origin = new Vector2(
				(float)( -itemScrPos.X ),
				(float)( itemTex.Height / 2 )
			);

			if( plr.direction == -1 ) {
				origin = new Vector2(
					(float)( itemTex.Width + itemScrPos.X ),
					(float)( itemTex.Height / 2 )
				);
			}

			//

			Vector2 pos = itemWldPos - Main.screenPosition + itemTexOffset;

			DrawData getDrawData( Texture2D tex, Color color ) {
				return new DrawData(
					tex,
					pos,
					new Rectangle( 0, 0, itemTex.Width, itemTex.Height ),
					color,
					plr.itemRotation,
					origin,
					plr.HeldItem.scale,
					plrDrawInfo.spriteEffects,
					0
				);
			}

			//

			drawInfo = getDrawData( itemTex, plr.HeldItem.GetAlpha(itemLight) );
			//drawInfo.Draw( Main.spriteBatch );
			Main.playerDrawData.Add( drawInfo );

			if( plr.HeldItem.color != default(Color) ) {
				drawInfo = getDrawData( itemTex, plr.HeldItem.GetColor( itemLight ) );
				//drawInfo.Draw( Main.spriteBatch );
				Main.playerDrawData.Add( drawInfo );
			}

			if( plr.HeldItem.glowMask != -1 ) {
				drawInfo = getDrawData(
					Main.glowMaskTexture[(int)plr.HeldItem.glowMask],
					new Color( 250, 250, 250, plr.HeldItem.alpha )
				);
				//drawInfo.Draw( Main.spriteBatch );
				Main.playerDrawData.Add( drawInfo );
			}
		}
	}
}
