using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;


namespace TheMadRanger.Items {
	partial class SpeedloaderItem : ModItem {
		public override bool PreDrawInInventory(
					SpriteBatch spriteBatch,
					Vector2 position,
					Rectangle frame,
					Color drawColor,
					Color itemColor,
					Vector2 origin,
					float scale ) {
			Texture2D tex = Main.itemTexture[ this.item.type ];
			if( this.LoadedRounds > 0 ) {
				tex = ModContent.GetTexture( this.Texture );
			}

			spriteBatch.Draw( tex, position, frame, drawColor, 0f, origin, scale, SpriteEffects.None, 0f );
			if( this.item.color != Color.Transparent ) {
				spriteBatch.Draw( tex, position, frame, itemColor, 0f, origin, scale, SpriteEffects.None, 0f );
			}

			return false;
		}
		
		public override void PostDrawInWorld(
					SpriteBatch spriteBatch,
					Color lightColor,
					Color alphaColor,
					float rotation,
					float scale,
					int whoAmI ) {
			Texture2D tex = Main.itemTexture[this.item.type];
			if( this.LoadedRounds > 0 ) {
				tex = ModContent.GetTexture( this.Texture );
			}

			float offsetX = this.item.height - tex.Height;
			float offsetY = (this.item.width / 2) - (tex.Width / 2);

			Color alpha = this.item.GetAlpha( lightColor );

			Main.spriteBatch.Draw(
				tex,
				(this.item.position - Main.screenPosition) + new Vector2(
					(float)( tex.Width / 2 ) + offsetY,
					(float)( tex.Height / 2 ) + offsetX + 2f
				),
				new Rectangle( 0, 0, tex.Width, tex.Height ),
				alpha,
				rotation,
				new Vector2( tex.Width / 2, tex.Height / 2 ),
				scale,
				SpriteEffects.None,
				0f
			);
			if( this.item.color != default(Color) ) {
				Main.spriteBatch.Draw(
					tex,
					(this.item.position - Main.screenPosition) + new Vector2(
						(float)( tex.Width / 2 ) + offsetY,
						(float)( tex.Height / 2 ) + offsetX + 2f
					),
					new Rectangle( 0, 0, tex.Width, tex.Height ),
					this.item.GetColor( lightColor ),
					rotation,
					new Vector2( tex.Width / 2, tex.Height / 2 ),
					scale,
					SpriteEffects.None,
					0f
				);
			}
		}
	}
}
