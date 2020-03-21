using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using HamstarHelpers.Services.AnimatedColor;


namespace TheMadRanger {
	public partial class TMRMod : Mod {
		public const float CrosshairDurationTicksMax = 12f;



		////////////////

		private float PreAimZoomAnimationPercent = 0f;
		private float AimZoomAnimationPercent = -1f;
		private AnimatedColors ColorAnim = null;



		////////////////

		public override void ModifyInterfaceLayers( List<GameInterfaceLayer> layers ) {
			float aimPercent;
			bool isAimMode = this.RunAimCursorAnimation( out aimPercent );
			bool isPreAimMode = isAimMode
				? false
				: this.RunPreAimCursorAnimation();

			if( !isAimMode && !isPreAimMode && aimPercent <= 0f ) {
				return;
			}

			int idx = layers.FindIndex( layer => layer.Name.Equals( "Vanilla: Cursor" ) );
			if( idx == -1 ) { return; }

			if( this.ColorAnim == null ) {
				this.ColorAnim = AnimatedColors.Create( 6, AnimatedColors.Alert.Colors.ToArray() );
			}

			GameInterfaceDrawMethod draw = () => {
				if( isPreAimMode ) {
					this.DrawPreAimCursor();
				} else if( isAimMode ) {
					this.DrawAimCursor();
				} else if( aimPercent > 0f ) {
					this.DrawUnaimCursor();
				}

				if( TMRConfig.Instance.DebugModeInfo ) {
					Player plr = Main.LocalPlayer;

					Vector2 fro = plr.MountedCenter;
					fro -= Main.screenPosition;

					Vector2 to = new Vector2( (float)Math.Cos(plr.itemRotation), (float)Math.Sin(plr.itemRotation) );
					to *= plr.direction * 64;
					to += plr.MountedCenter;
					to -= Main.screenPosition;

					//Utils.DrawLine( Main.spriteBatch, fro, to, Color.White );
					Utils.DrawLaser( Main.spriteBatch, Main.magicPixel, fro, to, Vector2.One, new Utils.LaserLineFraming(DelegateMethods.RainbowLaserDraw) );
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

			this.PreAimZoomAnimationPercent += 1f / 20f;
			if( this.PreAimZoomAnimationPercent > 1f ) {
				this.PreAimZoomAnimationPercent = 0f;
			}
			return true;
		}
		
		private bool RunAimCursorAnimation( out float aimPercent ) {
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
				color: Color.White * 0.05f * this.PreAimZoomAnimationPercent,
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
				color: Color.Gray,
				rotation: 0f,
				origin: new Vector2( tex.Width / 2, tex.Height / 2 ),
				scale: 0.25f,
				effects: SpriteEffects.None,
				layerDepth: 0f
			);
		}
	}
}