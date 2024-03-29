using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using ModLibsCore.Libraries.Debug;
using TheMadRanger.HUD;


namespace TheMadRanger {
	public partial class TMRMod : Mod {
		private HUDDrawData HUDData = null;



		////////////////

		public override void UpdateUI( GameTime gameTime ) {
			this.HUDData = new HUDDrawData( this.HUDData );

			var crosshairHUD = ModContent.GetInstance<CrosshairHUD>();

			crosshairHUD.Update( this.HUDData );
		}


		////////////////

		public override void ModifyInterfaceLayers( List<GameInterfaceLayer> layers ) {
			if( this.HUDData == null ) {
				return;
			}

			int cursorIdx = layers.FindIndex( layer => layer.Name.Equals( "Vanilla: Cursor" ) );
			if( cursorIdx == -1 ) {
				return;
			}
			
			var crosshairHUD = ModContent.GetInstance<CrosshairHUD>();

			//

			GameInterfaceDrawMethod draw = () => {
				crosshairHUD.DrawIf( Main.spriteBatch, this.HUDData );

				if( TMRConfig.Instance.DebugModeGunInfo ) {
					this.DrawDebugLine();
				}
				return true;
			};

			var interfaceLayer = new LegacyGameInterfaceLayer(
				"TheMadRanger: Crosshair",
				draw,
				InterfaceScaleType.UI
			);

			//

			if( crosshairHUD.ConsumesCursor(this.HUDData) ) {
				layers.RemoveAt( cursorIdx );
			}

			layers.Insert( cursorIdx, interfaceLayer );
		}


		////////////////

		private void DrawDebugLine() {
			Player plr = Main.LocalPlayer;

			Vector2 fro = plr.MountedCenter;
			fro -= Main.screenPosition;

			Vector2 to = new Vector2( (float)Math.Cos( plr.itemRotation ), (float)Math.Sin( plr.itemRotation ) );
			to *= plr.direction * 64;
			to += plr.MountedCenter;
			to -= Main.screenPosition;

			Utils.DrawLine( Main.spriteBatch, fro, to, Color.White );
			Utils.DrawLaser( Main.spriteBatch, Main.magicPixel, fro, to, Vector2.One * 4f, new Utils.LaserLineFraming( DelegateMethods.RainbowLaserDraw ) );
		}
	}
}