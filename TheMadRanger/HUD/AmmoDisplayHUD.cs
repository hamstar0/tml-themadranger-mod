using System;
using Microsoft.Xna.Framework;
using Terraria;
using HamstarHelpers.Classes.Loadable;
using HamstarHelpers.Helpers.Debug;


namespace TheMadRanger.HUD {
	partial class AmmoDisplayHUD : ILoadable {
		public static Vector2 GetAmmoHUDCenter() {
			var pos = new Vector2(
				Main.screenWidth - 96,
				Main.screenHeight - 128
			);

			var myplayer = Main.LocalPlayer.GetModPlayer<TMRPlayer>();
			pos += myplayer.AmmoDisplayOffset;

			return pos;
		}



		////////////////
		
		private bool IsHovering = false;



		////////////////

		void ILoadable.OnModsLoad() {
		}

		void ILoadable.OnModsUnload() {
		}

		void ILoadable.OnPostModsLoad() {
		}


		////////////////

		public void Update( HUDDrawData hudDrawData ) {
			if( Main.playerInventory ) {
				hudDrawData.IsAmmoHUDBeingEdited = this.RunHUDEditor( out this.IsHovering );
			} else {
				this.IsHovering = false;
				hudDrawData.IsAmmoHUDBeingEdited = false;
			}
		}


		////////////////

		public bool ConsumesCursor( HUDDrawData hudDrawData ) {
			return this.BaseAmmoDragOffset.HasValue;
		}
	}
}