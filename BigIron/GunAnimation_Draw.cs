using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using HamstarHelpers.Helpers.Debug;
using BigIron.Items.Weapons;


namespace BigIron {
	partial class GunAnimation {
		public void ModifyDrawLayers( Player plr, List<PlayerLayer> layers ) {
			if( this.IsHolstering ) {
				this.ModifyDrawLayersForHolstering( plr, layers );
			}
		}

		private void ModifyDrawLayersForHolstering( Player plr, List<PlayerLayer> layers ) {
			int heldItemIdx = layers.FindIndex( lyr => lyr == PlayerLayer.HeldItem );
			int bodyLayerIdx = layers.FindIndex( lyr => lyr == PlayerLayer.Body );
			int skinLayerIdx = layers.FindIndex( lyr => lyr == PlayerLayer.Skin );

			this.BodyFramePlayer = plr;
			this.BodyFrameShifted = this.BodyFrameUnshifted = plr.bodyFrame;
			this.BodyFrameUnshifted.Y = plr.bodyFrame.Height * 3;

			if( heldItemIdx != -1 ) {
				layers.Insert( heldItemIdx + 1, this.GunDrawLayer );
			}
			if( bodyLayerIdx != -1 && skinLayerIdx != -1 ) {
				layers.Insert( bodyLayerIdx + 1, this.BodyUnshiftLayer );
				layers.Insert( bodyLayerIdx, this.BodyShiftLayer );
				layers.Insert( skinLayerIdx + 1, this.SkinUnshiftLayer );
				layers.Insert( skinLayerIdx, this.SkinShiftLayer );

				PlayerLayer.Body.visible = false;
				PlayerLayer.Skin.visible = false;
			}
		}


		////////////////

		public DrawData DrawGun( PlayerDrawInfo plrDrawInfo ) {
			Player plr = plrDrawInfo.drawPlayer;
			Texture2D itemTex = Main.itemTexture[ ModContent.ItemType<BigIronItem>() ];
			Vector2 origin = new Vector2( itemTex.Width/2, itemTex.Height/2 );

			double progress = 1d - ( (double)this.HolsterDuration / (double)this.HolsterDurationMax );

			//var aim = new Vector2( (float)Math.Cos(this.LastKnownItemRotation), (float)Math.Sin(this.LastKnownItemRotation) );
			//aim = Vector2.Normalize( aim );
			//aim *= itemTex.Width * 0.25f;

			//float curve = (float)Math.Sin( progress * Math.PI );

			Vector2 pos = plr.MountedCenter + new Vector2(plr.direction * 8, 0) - Main.screenPosition;

			DrawData getDrawData( Texture2D tex, Color color ) {
				return new DrawData(
					texture: tex,
					position: pos,
					sourceRect: null,
					color: color,
					rotation: this.AddedRotationRadians,
					origin: origin,
					scale: BigIronItem.Scale,
					effect: plrDrawInfo.spriteEffects,
					inactiveLayerDepth: 0
				);
			}

			//

			int lightTileX = (int)( ( plr.position.X + ( (float)plr.width * 0.5f ) ) / 16f );
			int lightTileY = (int)( ( plr.position.Y + ( (float)plr.height * 0.5f ) ) / 16f );
			Color plrLight = Lighting.GetColor( lightTileX, lightTileY );
			//ItemSlot.GetItemLight( ref plrLight, plr.HeldItem, false );
			//Color itemLight = BigIronPlayer.GetItemLightColor( plr, plrLight );

			return getDrawData( itemTex, plrLight );
		}
	}
}
