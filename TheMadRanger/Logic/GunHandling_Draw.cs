using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using HamstarHelpers.Helpers.Debug;
using TheMadRanger.Items.Weapons;


namespace TheMadRanger.Logic {
	partial class GunHandling {
		private void InitDrawLayers() {
			this.GunDrawLayer = new PlayerLayer( "TheMadRanger", "Custom Gun Animation", ( plrDrawInfo ) => {
				Main.playerDrawData.Add( this.GetGunDrawData( plrDrawInfo ) );

				DrawData? drawData = this.GetReloadDrawData( plrDrawInfo );
				if( drawData.HasValue ) {
					Main.playerDrawData.Add( drawData.Value );
				}
			} );
		}


		////////////////

		public void ModifyDrawLayers( Player plr, List<PlayerLayer> layers ) {
			if( this.IsAnimating ) {
				int heldItemIdx = layers.FindIndex( lyr => lyr == PlayerLayer.HeldItem );
				if( heldItemIdx != -1 ) {
					layers[ heldItemIdx ].visible = false;
					layers.Insert( heldItemIdx + 1, this.GunDrawLayer );
				}
			}
		}


		////////////////

		public DrawData GetGunDrawData( PlayerDrawInfo plrDrawInfo ) {
			Player plr = plrDrawInfo.drawPlayer;
			Texture2D itemTex = Main.itemTexture[ ModContent.ItemType<TheMadRangerItem>() ];

			Vector2 offset = this.GetAddedPositionOffset( plr );
			Vector2 origin = new Vector2( itemTex.Width/2, itemTex.Height/2 );

			//double progress = 1d - ( (double)this.HolsterDuration / (double)this.HolsterDurationMax );
			//
			//var aim = new Vector2( (float)Math.Cos(this.LastKnownItemRotation), (float)Math.Sin(this.LastKnownItemRotation) );
			//aim = Vector2.Normalize( aim );
			//aim *= itemTex.Width * 0.25f;

			//float curve = (float)Math.Sin( progress * Math.PI );

			Vector2 pos = plr.MountedCenter
				+ new Vector2(plr.direction * 8, 0)
				+ new Vector2(plr.direction * offset.X, offset.Y)
				- Main.screenPosition;
			float rot = this.GetAddedRotationRadians( plr );

			DrawData getDrawData( Texture2D tex, Color color ) {
				return new DrawData(
					texture: tex,
					position: pos,
					sourceRect: null,
					color: color,
					rotation: rot,
					origin: origin,
					scale: TheMadRangerItem.Scale,
					effect: plrDrawInfo.spriteEffects,
					inactiveLayerDepth: 0
				);
			}

			//

			int lightTileX = (int)( ( plr.position.X + ( (float)plr.width * 0.5f ) ) / 16f );
			int lightTileY = (int)( ( plr.position.Y + ( (float)plr.height * 0.5f ) ) / 16f );
			Color plrLight = Lighting.GetColor( lightTileX, lightTileY );
			//ItemSlot.GetItemLight( ref plrLight, plr.HeldItem, false );
			//Color itemLight = TMRPlayer.GetItemLightColor( plr, plrLight );

			return getDrawData( itemTex, plrLight );
		}


		private DrawData? GetReloadDrawData( PlayerDrawInfo plrDrawInfo ) {
			if( !this.IsReloading || !this.ReloadingRounds ) {
				return null;
			}
			if( this.ReloadDuration > 15 ) {
				return null;
			}

			Player plr = plrDrawInfo.drawPlayer;
			Texture2D handTex;
			Rectangle frame = plr.bodyFrame;
			frame.Y = 0;

			if( plr.handon <= 0 ) {
				handTex = Main.playerTextures[ plr.skinVariant, 9 ];
			} else {
				handTex = Main.accHandsOnTexture[ (int)plr.handon ];
			}

			Vector2 pos = plr.MountedCenter
				+ new Vector2( plr.direction * 18, -16 )
				- new Vector2( frame.Width / 2, frame.Height / 2 )
				- Main.screenPosition;
			pos.Y += 5 - (this.ReloadDuration / 3);

//Utils.DrawRectangle( Main.spriteBatch, pos+Main.screenPosition, pos+frame.BottomRight()+Main.screenPosition, Color.Blue, Color.Red, 1f );
			return new DrawData(
				texture: handTex,
				position: pos,
				sourceRect: frame,
				color: plrDrawInfo.bodyColor,
				rotation: 0f,
				origin: default(Vector2),
				scale: 1f,
				effect: plrDrawInfo.spriteEffects,
				inactiveLayerDepth: 0
			);
		}
	}
}
