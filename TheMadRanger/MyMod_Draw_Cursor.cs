using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using TheMadRanger.Logic;


namespace TheMadRanger {
	public partial class TMRMod : Mod {
		private bool RunPreAimCursorAnimation( float aimPercent ) {
			var myplayer = Main.LocalPlayer.GetModPlayer<TMRPlayer>();
			if( !myplayer.AimMode.IsModeActivating || myplayer.AimMode.IsModeActive ) {
				return false;
			}

			this.PreAimZoomAnimationPercent += 1f / 20f;
			if( this.PreAimZoomAnimationPercent > 1f ) {
				this.PreAimZoomAnimationPercent = 0f;
			}
			return true;
		}
		
		private bool RunAimCursorAnimation( out bool isReloading, out bool hasGun, out float aimPercent ) {
			var myplayer = Main.LocalPlayer.GetModPlayer<TMRPlayer>();

			if( !myplayer.AimMode.IsModeActive ) {
				// Fade out and zoom out slowly, invisibly
				if( this.AimZoomAnimationPercent >= 0f ) {
					this.AimZoomAnimationPercent -= 1f / TMRMod.CrosshairDurationTicksMax;
				} else {
					this.AimZoomAnimationPercent = -1f;
				}
			} else {
				// Begin fade in and zoom in
				if( this.AimZoomAnimationPercent == -1f ) {
					if( myplayer.AimMode.IsQuickDrawActive ) {
						this.AimZoomAnimationPercent = 1f;
					} else {
						this.AimZoomAnimationPercent = 0f;
					}
				} else if( this.AimZoomAnimationPercent <= 1f ) {
					this.AimZoomAnimationPercent += 1f / TMRMod.CrosshairDurationTicksMax;  // Actual fading in
				}
				
				if( this.AimZoomAnimationPercent > 1f ) {
					this.AimZoomAnimationPercent = 1f;
				}
			}

			isReloading = myplayer.GunHandling.IsReloading;
			hasGun = !myplayer.GunHandling.IsAnimating && PlayerLogic.IsHoldingGun( Main.LocalPlayer );
			aimPercent = myplayer.AimMode.AimPercent;
			return myplayer.AimMode.IsModeActive;
		}


		////////////////

		private void DrawPreAimCursor( float aimPercent ) {
			Texture2D tex = this.GetTexture( "crosshair" );

			float zoomFocus = 1f - this.PreAimZoomAnimationPercent;
			float scale = 0.2f
				+ ((1f - aimPercent) * 0.4f)
				+ (0.1f * zoomFocus);

			float intensity = 0.1f + (aimPercent * 0.5f);
			float flicker = 0.25f + (this.PreAimZoomAnimationPercent * 0.75f);
			Color fgColor = Color.White * intensity * flicker;
			Color bgColor = Color.Black * intensity * flicker;

			Main.spriteBatch.Draw(
				texture: tex,
				position: new Vector2( Main.mouseX, Main.mouseY ),
				sourceRectangle: null,
				color: bgColor,
				rotation: 0f,
				origin: new Vector2( tex.Width / 2, tex.Height / 2 ),
				scale: scale + 0.1f,
				effects: SpriteEffects.None,
				layerDepth: 0f
			);

			Main.spriteBatch.Draw(
				texture: tex,
				position: new Vector2( Main.mouseX, Main.mouseY ),
				sourceRectangle: null,
				color: fgColor,
				rotation: 0f,
				origin: new Vector2( tex.Width / 2, tex.Height / 2 ),
				scale: scale,
				effects: SpriteEffects.None,
				layerDepth: 0f
			);
		}

		private void DrawAimCursor() {
			var config = TMRConfig.Instance;
			Texture2D tex = this.GetTexture( "crosshair" );

			float percentEmpty = 1f - this.AimZoomAnimationPercent;
			float scale = 0.25f + (1.75f * percentEmpty);

			float intensity = config.Get<float>( nameof(TMRConfig.ReticuleIntensityPercent) );
			float pulse = (float)Main.mouseTextColor / 255f;
			Color fgColor = Color.Yellow * intensity * pulse * pulse * pulse * pulse;
			Color bgColor = Color.Black * intensity * pulse * pulse * pulse * pulse;

			Main.spriteBatch.Draw(
				texture: tex,
				position: new Vector2( Main.mouseX, Main.mouseY ),
				sourceRectangle: null,
				color: bgColor,
				rotation: 0f,
				origin: new Vector2( tex.Width, tex.Height ) * 0.5f,
				scale: scale + 0.1f,
				effects: SpriteEffects.None,
				layerDepth: 0f
			);

			Main.spriteBatch.Draw(
				texture: tex,
				position: new Vector2( Main.mouseX, Main.mouseY ),
				sourceRectangle: null,
				color: fgColor,
				rotation: 0f,
				origin: new Vector2( tex.Width, tex.Height ) * 0.5f,
				scale: scale,
				effects: SpriteEffects.None,
				layerDepth: 0f
			);
		}

		private void DrawUnaimCursor() {
			/*var config = TMRConfig.Instance;
			Texture2D tex = this.GetTexture( "crosshair" );

			Main.spriteBatch.Draw(
				texture: tex,
				position: new Vector2( Main.mouseX, Main.mouseY ),
				sourceRectangle: null,
				color: Color.Black * 0.5f * config.Get<float>( nameof(TMRConfig.ReticuleIntensityPercent) ),
				rotation: 0f,
				origin: new Vector2( tex.Width / 2, tex.Height / 2 ),
				scale: 0.25f,
				effects: SpriteEffects.None,
				layerDepth: 0f
			);*/
		}
	}
}