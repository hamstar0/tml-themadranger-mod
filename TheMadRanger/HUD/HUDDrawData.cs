using System;
using System.Collections.Generic;
using Terraria;
using TheMadRanger.Logic;


namespace TheMadRanger.HUD {
	class HUDDrawData {
		public bool HasGun;
		public bool IsReloading;
		public bool IsPreAimMode;
		public bool IsAimMode;
		public float AimPercent;


		////////////////

		public IDictionary<object, bool> IsEditingHUD { get; } = new Dictionary<object, bool>();



		////////////////

		public HUDDrawData() {
			var myplayer = Main.LocalPlayer.GetModPlayer<TMRPlayer>();

			this.HasGun = !myplayer.GunHandling.IsAnimating && PlayerLogic.IsHoldingGun( Main.LocalPlayer );
			this.IsReloading = myplayer.GunHandling.IsReloading;
			this.AimPercent = myplayer.AimMode.AimPercent;
			this.IsAimMode = myplayer.AimMode.IsModeActive;
			this.IsPreAimMode = !myplayer.AimMode.IsModeActive && myplayer.AimMode.IsModeActivating;
		}
	}
}
