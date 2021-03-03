using System;
using Terraria;
using TheMadRanger.Logic;


namespace TheMadRanger.HUD {
	class HUDDrawData {
		public bool HasGun;
		public bool IsReloading;
		public bool IsPreAimMode;
		public bool IsAimMode;
		public float AimPercent;

		public bool IsAmmoHUDBeingEdited;



		////////////////

		public HUDDrawData( HUDDrawData prevHudData ) {
			var myplayer = Main.LocalPlayer.GetModPlayer<TMRPlayer>();

			this.HasGun = !myplayer.GunHandling.IsAnimating && PlayerLogic.IsHoldingGun( Main.LocalPlayer );
			this.IsReloading = myplayer.GunHandling.IsReloading;
			this.AimPercent = myplayer.AimMode.AimPercent;
			this.IsAimMode = myplayer.AimMode.IsModeActive;
			this.IsPreAimMode = !myplayer.AimMode.IsModeActive && myplayer.AimMode.IsModeActivating;

			this.IsAmmoHUDBeingEdited = prevHudData?.IsAmmoHUDBeingEdited ?? false;
		}
	}
}
