using System;
using Microsoft.Xna.Framework;
using Terraria;
using HamstarHelpers.Helpers.Debug;
using HUDElementsLib;


namespace TheMadRanger.HUD {
	public partial class AmmoDisplayHUD : HUDElement {
		public static AmmoDisplayHUD CreateDefault() {
			var config = TMRConfig.Instance;
			var pos = new Vector2(
				config.Get<float>( nameof(config.AmmoHUDPositionX) ),
				config.Get<float>( nameof(config.AmmoHUDPositionY) )
			);
			return new AmmoDisplayHUD( "Ammo Display", pos );
		}



		////////////////

		private AmmoDisplayHUD( string name, Vector2 pos ) : base( name, pos, new Vector2(56f, 56f) ) {
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