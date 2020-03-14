using HamstarHelpers.Services.AnimatedColor;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;


namespace BigIron {
	public partial class BigIronMod : Mod {
		public const float CrosshairDurationTicksMax = 9f;



		////////////////

		private float AnimDuration = -1f;
		private AnimatedColors ColorAnim = null;



		////////////////

		public override void ModifyInterfaceLayers( List<GameInterfaceLayer> layers ) {
			if( !this.RunAnimation() ) { return; }

			int idx = layers.FindIndex( layer => layer.Name.Equals( "Vanilla: Cursor" ) );
			if( idx == -1 ) { return; }

			if( this.ColorAnim == null ) {
				this.ColorAnim = AnimatedColors.Create( 6, AnimatedColors.Alert.Colors.ToArray() );
			}

			GameInterfaceDrawMethod draw = () => {
				this.DrawAimCursor();
				return true;
			};
			var interfaceLayer = new LegacyGameInterfaceLayer( "BigIron: Crosshair", draw, InterfaceScaleType.UI );

			//layers.RemoveAt( idx );
			layers.Insert( idx, interfaceLayer );
		}


		////////////////
		
		private bool RunAnimation() {
			var myplayer = Main.LocalPlayer.GetModPlayer<BigIronPlayer>();
			if( !myplayer.AimMode.IsModeActive ) {
				if( this.AnimDuration >= 0f && this.AnimDuration < BigIronMod.CrosshairDurationTicksMax ) {
					this.AnimDuration += 0.25f;
				} else {
					this.AnimDuration = -1f;
				}

				return false;
			}

			this.AnimDuration = (float)Math.Floor( this.AnimDuration );

			if( this.AnimDuration == -1f ) {
				this.AnimDuration = BigIronMod.CrosshairDurationTicksMax;
			} else if( this.AnimDuration > 0 ) {
				this.AnimDuration -= 1f;
			}

			return true;
		}


		////////////////

		private void DrawAimCursor() {
			Texture2D tex = this.GetTexture( "crosshair" );

			float percentEmpty = this.AnimDuration / BigIronMod.CrosshairDurationTicksMax;
			float scale = 0.25f + (1.75f * percentEmpty );

			Color color = this.ColorAnim.CurrentColor;
			if( percentEmpty > 0f ) {
				color = Color.Lerp( color * 0.5f, Color.Transparent, percentEmpty );
			}

			Main.spriteBatch.Draw(
				texture: tex,
				position: new Vector2( Main.mouseX, Main.mouseY ),
				sourceRectangle: null,
				color: color,
				rotation: 0f,
				origin: new Vector2( tex.Width / 2, tex.Height / 2 ),
				scale: scale,
				effects: SpriteEffects.None,
				layerDepth: 0f
			);
		}
	}
}