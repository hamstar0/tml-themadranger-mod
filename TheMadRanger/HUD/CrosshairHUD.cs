using System;
using System.Linq;
using Terraria;
using HamstarHelpers.Classes.Loadable;


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
			if( !hudDrawData.IsEditingHUD.Values.Any(b => b) ) {
				this.RunPreAimCursorAnimation( hudDrawData );
				this.RunAimCursorAnimation( hudDrawData );
			}
		}


		////////////////

		public void Draw( HUDDrawData hudDrawData ) {
			if( Main.InGameUI.CurrentState != null ) {
				return;
			}
			if( hudDrawData.IsEditingHUD.Values.Any(b => b) ) {
				return;
			}

			if( hudDrawData.IsPreAimMode ) {
				this.DrawPreAimCursor( hudDrawData.AimPercent );
			} else if( hudDrawData.IsAimMode ) {
				this.DrawAimCursor();
			} else if( hudDrawData.HasGun ) {
				this.DrawUnaimCursor();
			}
		}


		////////////////

		public bool ConsumesCursor( HUDDrawData hudDrawData ) {
			if( Main.playerInventory ) {
				return false;
			}
			if( Main.InGameUI.CurrentState == null ) {
				return false;
			}

			return hudDrawData.IsAimMode
				|| (hudDrawData.IsPreAimMode && hudDrawData.AimPercent > 0.25f);
		}
	}
}
