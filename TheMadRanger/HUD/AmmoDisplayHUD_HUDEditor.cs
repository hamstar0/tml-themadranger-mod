using System;
using Microsoft.Xna.Framework;
using Terraria;
using HamstarHelpers.Classes.Loadable;
using HamstarHelpers.Helpers.Debug;


namespace TheMadRanger.HUD {
	partial class AmmoDisplayHUD : ILoadable {
		private Vector2? BaseAmmoDragOffset = null;
		private Vector2 PreviousAmmoDragMousePos = default;



		////////////////

		private bool RunHUDEditor( out bool isHovering ) {
			Vector2 basePos = AmmoDisplayHUD.GetAmmoHUDCenter();
			var area = new Rectangle(
				(int)basePos.X - 28,
				(int)basePos.Y - 28,
				56,
				56
			);

			isHovering = area.Contains( Main.MouseScreen.ToPoint() );

			if( Main.mouseLeft ) {
				if( this.BaseAmmoDragOffset.HasValue || isHovering ) {
					this.HUDEditor_Bullets_Drag( basePos );
				}
			} else {
				this.BaseAmmoDragOffset = null;
			}

			return this.BaseAmmoDragOffset.HasValue;
		}


		private void HUDEditor_Bullets_Drag( Vector2 basePos ) {
			if( !this.BaseAmmoDragOffset.HasValue ) {
				this.BaseAmmoDragOffset = basePos - Main.MouseScreen;
				this.PreviousAmmoDragMousePos = Main.MouseScreen;

				return;
			}

			Vector2 movedSince = Main.MouseScreen - this.PreviousAmmoDragMousePos;
			this.PreviousAmmoDragMousePos = Main.MouseScreen;

			var myplayer = Main.LocalPlayer.GetModPlayer<TMRPlayer>();
			myplayer.AmmoDisplayOffset += movedSince;
		}
	}
}