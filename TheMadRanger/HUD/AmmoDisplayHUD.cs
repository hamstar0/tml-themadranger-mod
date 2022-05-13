using System;
using Microsoft.Xna.Framework;
using Terraria;
using ModLibsCore.Libraries.Debug;
using HUDElementsLib;


namespace TheMadRanger.HUD {
	public partial class AmmoDisplayHUD : HUDElement {
		public static AmmoDisplayHUD CreateDefault() {
			var config = TMRConfig.Instance;

			var posOffset = new Vector2(
				config.Get<float>( nameof(config.AmmoHUDPositionX) ),
				config.Get<float>( nameof(config.AmmoHUDPositionY) )
			);
			var posPerc = new Vector2(
				posOffset.X < 0f ? 1f : 0f,
				posOffset.Y < 0f ? 1f : 0f
			);

			return new AmmoDisplayHUD( "Ammo Display", posOffset, posPerc );
		}



		////////////////

		private AmmoDisplayHUD( string name, Vector2 posOffset, Vector2 posPerc )
			: base( name, posOffset, posPerc, new Vector2(56f, 56f), AmmoDisplayHUD.CanDrawAmmoDisplay ) {
		}


		////////////////

		public Vector2 GetDrawPositionOrigin() {
			Vector2 pos = this.GetHUDComputedPosition( true );
			Vector2 dim = this.GetHUDComputedDimensions();
			float width = dim.X * 0.5f;
			float height = dim.Y * 0.5f;

			return new Vector2( pos.X+width, pos.Y+height );
		}
	}
}