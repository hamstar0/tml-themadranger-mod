using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;


namespace TheMadRanger {
	public partial class TMRMod : Mod {
		private bool RunPreAimCursorAnimation() {
			var myplayer = Main.LocalPlayer.GetModPlayer<TMRPlayer>();
			if( !myplayer.AimMode.IsModeBeingActivated || myplayer.AimMode.IsModeActive ) {
				return false;
			}

			this.PreAimZoomAnimationPercent += 1f / 20f;
			if( this.PreAimZoomAnimationPercent > 1f ) {
				this.PreAimZoomAnimationPercent = 0f;
			}
			return true;
		}
		
		private bool RunAimCursorAnimation( out bool hasGun, out float aimPercent ) {
			var myplayer = Main.LocalPlayer.GetModPlayer<TMRPlayer>();

			if( !myplayer.AimMode.IsModeActive ) {
				// Fade out and zoom out slowly, invisibly
				if( this.AimZoomAnimationPercent >= 0f ) {
					this.AimZoomAnimationPercent -= TMRMod.CrosshairDurationTicksMax;
				} else {
					this.AimZoomAnimationPercent = -1f;
				}
			} else {
				// Begin fade in and zoom in
				if( this.AimZoomAnimationPercent == -1f ) {
					if( myplayer.AimMode.IsQuickDraw ) {
						this.AimZoomAnimationPercent = 1f;
					} else {
						this.AimZoomAnimationPercent = 0f;
					}
				} else if( this.AimZoomAnimationPercent <= 1f ) {
					this.AimZoomAnimationPercent += 1f / TMRMod.CrosshairDurationTicksMax;  // Actual fading in
				} else if( this.AimZoomAnimationPercent > 1f ) {
					this.AimZoomAnimationPercent = 1f;
				}
			}

			hasGun = !myplayer.GunHandling.IsAnimating && TMRPlayer.IsHoldingGun( Main.LocalPlayer );
			aimPercent = myplayer.AimMode.AimPercent;
			return myplayer.AimMode.IsModeActive;
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
				color: Color.White * 0.1f * this.PreAimZoomAnimationPercent,
				rotation: 0f,
				origin: new Vector2( tex.Width / 2, tex.Height / 2 ),
				scale: scale,
				effects: SpriteEffects.None,
				layerDepth: 0f
			);
		}

		private void DrawAimCursor() {
			Texture2D tex = this.GetTexture( "crosshair" );

			float percentEmpty = 1f - this.AimZoomAnimationPercent;
			float scale = 0.25f + (1.75f * percentEmpty);

			Color color = Color.Lerp(
				Color.Transparent,
				this.ColorAnim.CurrentColor,
				this.AimZoomAnimationPercent
			);

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

		private void DrawUnaimCursor() {
			Texture2D tex = this.GetTexture( "crosshair" );

			Main.spriteBatch.Draw(
				texture: tex,
				position: new Vector2( Main.mouseX, Main.mouseY ),
				sourceRectangle: null,
				color: Color.Black * 0.5f,
				rotation: 0f,
				origin: new Vector2( tex.Width / 2, tex.Height / 2 ),
				scale: 0.25f,
				effects: SpriteEffects.None,
				layerDepth: 0f
			);
		}
	}
}