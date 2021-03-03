using System;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using HamstarHelpers.Classes.Loadable;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.HUD;


namespace TheMadRanger.HUD {
	partial class CrosshairHUD : ILoadable {
		public const float CrosshairDurationTicksMax = 7f;



		////////////////

		private float PreAimZoomAnimationPercent = 0f;
		private float AimZoomAnimationPercent = -1f;



		////////////////

		void ILoadable.OnModsLoad() {
		}

		void ILoadable.OnModsUnload() {
		}

		void ILoadable.OnPostModsLoad() {
		}


		////////////////

		public void Update( HUDDrawData hudDrawData ) {
			this.RunPreAimCursorAnimation( hudDrawData );
			this.RunAimCursorAnimation( hudDrawData );
		}


		////////////////

		public void DrawIf( SpriteBatch sb, HUDDrawData hudDrawData ) {
			/*if( Main.InGameUI.CurrentState != null ) {
				return;
			}*/
			if( Main.LocalPlayer.mouseInterface ) {	//not HUDHelpers.IsMouseInterfacingWithUI; inventory always == true
				return;
			}
			if( hudDrawData.IsAmmoHUDBeingEdited ) {
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

		public bool ConsumesCursor( HUDDrawData hudDrawData ) {
			/*if( Main.InGameUI.CurrentState != null ) {
				return false;
			}*/
			if( HUDHelpers.IsMouseInterfacingWithUI ) { //Main.LocalPlayer.mouseInterface
				return false;
			}

			return hudDrawData.IsAimMode
				|| (hudDrawData.IsPreAimMode && hudDrawData.AimPercent > 0.25f);
		}
	}
}
