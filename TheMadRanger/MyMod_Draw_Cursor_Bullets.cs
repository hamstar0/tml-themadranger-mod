using System;
using HamstarHelpers.Helpers.Debug;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using TheMadRanger.Items.Weapons;


namespace TheMadRanger {
	public partial class TMRMod : Mod {
		private void DrawBullets( float aimPercent, bool isReloading ) {
			Item heldItem = Main.LocalPlayer.HeldItem;
			var myitem = heldItem.modItem as TheMadRangerItem;
			if( myitem == null ) {
				return;
			}

			//if( !isReloading && aimPercent < 1f ) {
			//	return;
			//}

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
					aimPercent: aimPercent,
					isReloading: isReloading
				);
			}
		}


		private void DrawBullet( int cylinderSlot, int bulletState, float aimPercent, bool isReloading ) {
			Texture2D tex = this.GetTexture( "bulletbutt" );
			var basePos = new Vector2( Main.mouseX, Main.mouseY );

			float radians = MathHelper.ToRadians( cylinderSlot * 60 );
			Vector2 pos = basePos + (new Vector2(0f, -1f).RotatedBy(radians) * 36f);

			Color color = bulletState == 1
				? Color.White * 0.4f
				: Color.Red * 0.15f;
			if( !isReloading ) {
				if( aimPercent >= 1f ) {
					float pulse = (float)Main.mouseTextColor / 255f;
					color *= pulse * pulse;
				} else {
					color *= aimPercent;
				}
			}

			Main.spriteBatch.Draw(
				texture: tex,
				position: pos,
				sourceRectangle: null,
				color: color,
				rotation: 0f,
				origin: new Vector2(tex.Width, tex.Height) * 0.5f,
				scale: 0.5f,
				effects: SpriteEffects.None,
				layerDepth: 0f
			);
		}
	}
}