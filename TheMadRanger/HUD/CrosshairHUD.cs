using System;
using Terraria;
using ModLibsCore.Classes.Loadable;
using ModLibsCore.Libraries.Debug;
using ModLibsGeneral.Libraries.HUD;


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

		public bool ConsumesCursor( HUDDrawData hudDrawData ) {
			/*if( Main.InGameUI.CurrentState != null ) {
				return false;
			}*/
			if( HUDLibraries.IsMouseInterfacingWithUI ) { //Main.LocalPlayer.mouseInterface
				return false;
			}

			return hudDrawData.IsAimMode
				|| (hudDrawData.IsPreAimMode && hudDrawData.AimPercent > 0.25f);
		}
	}
}
