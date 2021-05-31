using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using ModLibsGeneral.Libraries.Audio;
using ModLibsGeneral.Libraries.Tiles;


namespace TheMadRanger.Gores {
	class ShellCasing : ModGore {
		public const int MaxFrames = 3;



		////////////////
		
		public static byte GetFrame() {
			int frame = Main.rand.Next( -2, ShellCasing.MaxFrames );
			if( frame < 0 ) {
				frame = 0;
			}

			return (byte)frame;
		}

		public static Vector2 GetVelocity() {
			float spread = 2f;//0.4f;
			return new Vector2(
				(Main.rand.NextFloat() * spread) - (spread * 0.5f),
				Main.rand.NextFloat()
			);
		}

		public static float GetScale() {
			return 0.5f;
		}



		////////////////

		public override void OnSpawn( Gore gore ) {
			gore.scale = ShellCasing.GetScale();
			gore.drawOffset.X = 0f;
			gore.drawOffset.Y = 8f;
			gore.rotation = Main.rand.NextFloat() * (float)Math.PI * 2f;
			gore.velocity = ShellCasing.GetVelocity();
			gore.numFrames = ShellCasing.MaxFrames;
			gore.frame = ShellCasing.GetFrame();
			gore.frameCounter = 6;
			this.updateType = 3;
		}


		public override bool Update( Gore gore ) {
			bool lockFrames = false;
			
			if( gore.velocity.Y == 0 ) {
				if( !this.UpdateGround(gore, out lockFrames) ) {
					gore.active = false;
					return false;
				}
			}

			if( !lockFrames ) {
				if( gore.frameCounter-- <= 0 ) {
					gore.frameCounter = 6;
					gore.frame = ShellCasing.GetFrame();
				}
			}

			return true;
		}


		private bool UpdateGround( Gore gore, out bool lockFrames ) {
			lockFrames = false;

			if( gore.drawOffset.X == 0f ) {
//Main.NewText("bounce "+gore.GetHashCode());
				gore.drawOffset.X = 0.001f;
				SoundLibraries.PlaySound( TMRMod.Instance, "ShellBounce", gore.position );
			}

			int tileX = (int)gore.position.X >> 4;
			int tileY = (int)gore.position.Y >> 4;
			Tile tile = Main.tile[tileX, tileY];

			if( TileLibraries.IsSolid(tile, false, false) ) {
				if( tile.slope() != 0 ) {
					return false;
				}

				tile = Main.tile[tileX, tileY - 1];
				if( !TileLibraries.IsSolid( tile, false, false ) ) {
					gore.drawOffset.Y = 8f;
					gore.position.Y -= 16f;
				} else {
					return false;
				}
			}

			gore.rotation = (float)Math.PI * 0.5f;

			if( gore.velocity.X == 0 ) {
				lockFrames = true;
				gore.frameCounter = 2;
			}

			return true;
		}
	}
}
