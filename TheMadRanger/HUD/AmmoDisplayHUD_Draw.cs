using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using HamstarHelpers.Classes.Loadable;
using HamstarHelpers.Helpers.Debug;
using TheMadRanger.Items.Weapons;


namespace TheMadRanger.HUD {
	partial class AmmoDisplayHUD : ILoadable {
		public void Draw( HUDDrawData hudDrawData ) {
			if( Main.InGameUI.CurrentState != null ) {
				return;
			}
			//if( hudDrawData.IsReloading || hudDrawData.IsPreAimMode || hudDrawData.IsAimMode ) {

			Item heldItem = Main.LocalPlayer.HeldItem;
			var myitem = heldItem.modItem as TheMadRangerItem;
			if( myitem == null ) {
				return;
			}

			float opacity = Main.playerInventory ? 0.5f : 1f;

			int[] bullets = myitem.GetCylinder();
			int slot = myitem.CurrentCylinderSlot;
			int maxBullets = bullets.Length;

			for( int i=0; i<maxBullets; i++ ) {
				int rotSlot = slot - i;
				if( rotSlot < 0 ) {
					rotSlot += maxBullets;
				}

				this.DrawBullet(
					cylinderSlot: i,
					bulletState: bullets[rotSlot],
					opacity
					//aimPercent: hudDrawData.AimPercent,
					//isReloading: hudDrawData.IsReloading
				);
			}
		}


		private void DrawBullet( int cylinderSlot, int bulletState, float opacity ) {  //float aimPercent, bool isReloading
			Texture2D tex = TMRMod.Instance.GetTexture( "bulletbutt" );

			float radians = MathHelper.ToRadians( cylinderSlot * 60 );
			Vector2 pos = AmmoDisplayHUD.GetAmmoHUDCenter()
				+ (new Vector2(0f, -1f).RotatedBy(radians) * 24f);

			Color color = bulletState == 1
				? Color.White * 0.6f
				: Color.Red * 0.2f;
			//if( !isReloading ) {
			//	if( aimPercent >= 1f ) {
			//		float pulse = (float)Main.mouseTextColor / 255f;
			//		color *= pulse * pulse;
			//	} else {
			//		color *= aimPercent;
			//	}
			//}

			Main.spriteBatch.Draw(
				texture: tex,
				position: pos,
				sourceRectangle: null,
				color: color * opacity,
				rotation: 0f,
				origin: new Vector2(tex.Width, tex.Height) * 0.5f,
				scale: 0.65f,
				effects: SpriteEffects.None,
				layerDepth: 0f
			);
		}
	}
}