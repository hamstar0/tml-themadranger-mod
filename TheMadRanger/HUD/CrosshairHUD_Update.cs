using System;
using Terraria;
using ModLibsCore.Classes.Loadable;


namespace TheMadRanger.HUD {
	partial class CrosshairHUD : ILoadable {
		public void Update( HUDDrawData hudDrawData ) {
			this.UpdatePreAimCursorAnimation( hudDrawData );
			this.UpdateAimCursorAnimation( hudDrawData );
		}


		////////////////

		private void UpdatePreAimCursorAnimation( HUDDrawData hudDrawData ) {
			var myplayer = Main.LocalPlayer.GetModPlayer<TMRPlayer>();
			if( !myplayer.AimMode.IsModeActivating || myplayer.AimMode.IsModeActive ) {
				return;
			}

			this.PreAimZoomAnimationPercent += 1f / 20f;
			if( this.PreAimZoomAnimationPercent > 1f ) {
				this.PreAimZoomAnimationPercent = 0f;
			}
		}
		
		private void UpdateAimCursorAnimation( HUDDrawData hudDrawData ) {
			var myplayer = Main.LocalPlayer.GetModPlayer<TMRPlayer>();

			if( !myplayer.AimMode.IsModeActive ) {
				// Fade out and zoom out slowly, invisibly
				if( this.AimZoomAnimationPercent >= 0f ) {
					this.AimZoomAnimationPercent -= 1f / CrosshairHUD.CrosshairDurationTicksMax;
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
					this.AimZoomAnimationPercent += 1f / CrosshairHUD.CrosshairDurationTicksMax;  // Actual fading in
				}
				
				if( this.AimZoomAnimationPercent > 1f ) {
					this.AimZoomAnimationPercent = 1f;
				}
			}
		}
	}
}