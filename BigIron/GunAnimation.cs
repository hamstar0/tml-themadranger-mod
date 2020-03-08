using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.UI;
using Terraria.DataStructures;
using Terraria.ModLoader;
using HamstarHelpers.Helpers.Debug;
using BigIron.Items.Weapons;


namespace BigIron {
	partial class GunAnimation {
		public int Recoil { get; private set; } = 0;

		public int HolsterDuration { get; private set; } = 0;

		public int HolsterDurationMax { get; private set; } = 0;

		public float AddedRotationDegrees { get; private set; } = 0f;


		////

		public bool IsAnimating => this.HolsterDuration > 0;

		public float AddedRotationRadians => MathHelper.ToRadians( this.AddedRotationDegrees );

		////

		public PlayerLayer DrawLayer { get; }



		////////////////
		
		public GunAnimation() {
			this.DrawLayer = new PlayerLayer( "BigIron", "Custom Gun Animation", (plrDrawInfo) => {
				foreach( DrawData dat in this.DrawGun(plrDrawInfo) ) {
					Main.playerDrawData.Add( dat );
				}
			} );
		}

		////////////////

		public void Update() {
			if( this.HolsterDuration > 0 ) {
				this.HolsterDuration--;

				this.AddedRotationDegrees += 24f;
				if( this.AddedRotationDegrees > 360f ) {
					this.AddedRotationDegrees -= 360f;
				}
			} else {
				this.AddedRotationDegrees = 0f;
			}

			if( this.Recoil > 0 ) {
				this.Recoil--;
			}
		}

		////////////////

		public void BeginRecoil() {
			this.Recoil = 17;
		}

		public void BeginHolster() {
			this.HolsterDuration = 60;
			this.HolsterDurationMax = 60;
		}


		////////////////

		public IEnumerable<DrawData> DrawGun( PlayerDrawInfo plrDrawInfo ) {
			Player plr = plrDrawInfo.drawPlayer;
			Texture2D itemTex = Main.itemTexture[ModContent.ItemType<BigIronItem>()];
			Vector2 origin = new Vector2( itemTex.Width/2, itemTex.Height/2 );
			Vector2 pos = plr.Center;
			pos -= Main.screenPosition;

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
			ItemSlot.GetItemLight( ref plrLight, plr.HeldItem, false );
			Color itemLight = BigIronPlayer.GetItemLightColor( plr, plrLight );

			yield return getDrawData( itemTex, plr.HeldItem.GetAlpha(itemLight) );

			if( plr.HeldItem.color != default(Color) ) {
				yield return getDrawData( itemTex, plr.HeldItem.GetColor(itemLight) );
			}

			if( plr.HeldItem.glowMask != -1 ) {
				DrawData drawData = getDrawData(
					Main.glowMaskTexture[(int)plr.HeldItem.glowMask],
					new Color( 250, 250, 250, plr.HeldItem.alpha )
				);
				yield return drawData;
			}
		}
	}
}
