using HamstarHelpers.Services.AnimatedColor;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;


namespace TheMadRanger {
	public partial class TMRMod : Mod {
		public const float CrosshairDurationTicksMax = 9f;



		////////////////

		private float PreAimZoomAnimationPercent = 0f;
		private float AimZoomAnimationDuration = -1f;
		private AnimatedColors ColorAnim = null;



		////////////////

		public override void ModifyInterfaceLayers( List<GameInterfaceLayer> layers ) {
			bool isAiming = this.RunAimCursorAnimation();
			bool isPreAiming = isAiming
				? false
				: this.RunPreAimCursorAnimation();

			if( !isAiming && !isPreAiming ) {
				return;
			}

			int idx = layers.FindIndex( layer => layer.Name.Equals( "Vanilla: Cursor" ) );
			if( idx == -1 ) { return; }

			if( this.ColorAnim == null ) {
				this.ColorAnim = AnimatedColors.Create( 6, AnimatedColors.Alert.Colors.ToArray() );
			}

			GameInterfaceDrawMethod draw = () => {
				if( isPreAiming ) {
					this.DrawPreAimCursor();
				} else if( isAiming ) {
					this.DrawAimCursor();
				}
				return true;
			};
			var interfaceLayer = new LegacyGameInterfaceLayer( "TheMadRanger: Crosshair", draw, InterfaceScaleType.UI );

			//layers.RemoveAt( idx );
			layers.Insert( idx, interfaceLayer );
		}


		////////////////

		private bool RunPreAimCursorAnimation() {
			var myplayer = Main.LocalPlayer.GetModPlayer<TMRPlayer>();
			if( !myplayer.AimMode.IsModeBeingActivated || myplayer.AimMode.IsModeActive ) {
				return false;
			}

			this.PreAimZoomAnimationPercent += 1f / 15f;
			if( this.PreAimZoomAnimationPercent > 1f ) {
				this.PreAimZoomAnimationPercent = 0f;
			}
			return true;
		}
		
		private bool RunAimCursorAnimation() {
			var myplayer = Main.LocalPlayer.GetModPlayer<TMRPlayer>();

			// While aim mode gone
			if( !myplayer.AimMode.IsModeActive ) {
				// Fade out and zoom out
				if( this.AimZoomAnimationDuration >= 0f && this.AimZoomAnimationDuration < TMRMod.CrosshairDurationTicksMax ) {
					this.AimZoomAnimationDuration += 0.25f;
				} else {
					this.AimZoomAnimationDuration = -1f;	// Cursor is now gone
				}

				return false;
			}

			// Skim off fade
			this.AimZoomAnimationDuration = (float)Math.Floor( this.AimZoomAnimationDuration );

			// Begin fade in and zoom in
			if( this.AimZoomAnimationDuration == -1f ) {
				if( myplayer.AimMode.IsQuickDraw ) {
					this.AimZoomAnimationDuration = 0f;
				} else {
					this.AimZoomAnimationDuration = TMRMod.CrosshairDurationTicksMax;
				}
			} else if( this.AimZoomAnimationDuration > 0 ) {
				this.AimZoomAnimationDuration -= 1f;	// Actual fading in
			}

			return true;
		}


		////////////////

		private void DrawPreAimCursor() {
			Texture2D tex = this.GetTexture( "crosshair" );

			float zoomFocus = 1f - this.PreAimZoomAnimationPercent;
			float scale = 0.5f + (0.25f * zoomFocus);

			Main.spriteBatch.Draw(
				texture: tex,
				position: new Vector2( Main.mouseX, Main.mouseY ),
				sourceRectangle: null,
				color: Color.White * 0.025f * this.PreAimZoomAnimationPercent,
				rotation: 0f,
				origin: new Vector2( tex.Width / 2, tex.Height / 2 ),
				scale: scale,
				effects: SpriteEffects.None,
				layerDepth: 0f
			);
		}

		private void DrawAimCursor() {
			Texture2D tex = this.GetTexture( "crosshair" );

			float percentEmpty = this.AimZoomAnimationDuration / TMRMod.CrosshairDurationTicksMax;
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