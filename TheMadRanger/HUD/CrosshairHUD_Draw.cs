using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using ModLibsCore.Classes.Loadable;
using HUDElementsLib;


namespace TheMadRanger.HUD {
	partial class CrosshairHUD : ILoadable {
		public void DrawIf( SpriteBatch sb, HUDDrawData hudDrawData ) {
			/*if( Main.InGameUI.CurrentState != null ) {
				return;
			}*/
			if( Main.LocalPlayer.mouseInterface ) { //not HUDLibraries.IsMouseInterfacingWithUI; inventory always == true
				return;
			}
			if( HUDElementsLibAPI.GetDraggingElement() != null ) {
				return;
			}

			if( hudDrawData.IsPreAimMode ) {
				this.DrawPreAimCursor( sb, hudDrawData.AimPercent );
			} else if( hudDrawData.IsAimMode ) {
				this.DrawAimCursor( sb );
			} else if( hudDrawData.HasGun ) {
				this.DrawUnaimCursor( sb );
			}
		}


		////////////////

		private void DrawPreAimCursor( SpriteBatch sb, float aimPercent ) {
			Texture2D tex = TMRMod.Instance.GetTexture( "HUD/crosshair" );

			float zoomFocus = 1f - this.PreAimZoomAnimationPercent;
			float aimFocus = 1f - aimPercent;
			float scale = 0.2f
				+ (0.4f * aimFocus)
				+ (0.1f * zoomFocus);

			float intensity = 0.1f + (aimPercent * 0.5f);
			float flicker = 0.25f + (this.PreAimZoomAnimationPercent * 0.75f);
			Color fgColor = Color.White * intensity * flicker;
			Color bgColor = Color.Black * intensity * flicker;

			sb.Draw(
				texture: tex,
				position: new Vector2( Main.mouseX, Main.mouseY ),
				sourceRectangle: null,
				color: bgColor,
				rotation: 0f,
				origin: new Vector2( tex.Width / 2, tex.Height / 2 ),
				scale: scale + 0.05f,
				effects: SpriteEffects.None,
				layerDepth: 0f
			);

			sb.Draw(
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

		private void DrawAimCursor( SpriteBatch sb ) {
			var config = TMRConfig.Instance;
			Texture2D tex = TMRMod.Instance.GetTexture( "HUD/crosshair" );

			float percentEmpty = 1f - this.AimZoomAnimationPercent;
			float scale = 0.25f
				+ (1.75f * percentEmpty);

			float intensity = config.Get<float>( nameof(TMRConfig.ReticuleIntensityPercent) );
			float pulse = (float)Main.mouseTextColor / 255f;
			Color fgColor = Color.Yellow * intensity * pulse * pulse * pulse * pulse;
			Color bgColor = Color.Black * intensity * pulse * pulse * pulse * pulse;

			sb.Draw(
				texture: tex,
				position: new Vector2( Main.mouseX, Main.mouseY ),
				sourceRectangle: null,
				color: bgColor,
				rotation: 0f,
				origin: new Vector2( tex.Width / 2, tex.Height / 2 ),
				scale: scale + 0.05f,
				effects: SpriteEffects.None,
				layerDepth: 0f
			);

			sb.Draw(
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

		private void DrawUnaimCursor( SpriteBatch sb ) {
			/*var config = TMRConfig.Instance;
			Texture2D tex = this.GetTexture( "HUD/crosshair" );

			sb.Draw(
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