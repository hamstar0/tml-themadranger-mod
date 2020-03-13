using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.TModLoader;
using Terraria.Utilities;


namespace BigIron {
	partial class BigIronPlayer : ModPlayer {
		public static float GetAimShakeRadOffset() {
			UnifiedRandom rand = TmlHelpers.SafelyGetRand();

			return (rand.NextFloat() * 1.5f) - 0.75f;
		}



		////////////////

		private float AimElapsed = 0f;
		private Vector2 LastAimMousePosition = default( Vector2 );



		////////////////

		private void CheckAimState() {
			if( this.player.velocity.LengthSquared() > 1f ) {
				this.AimElapsed = Math.Max( this.AimElapsed - 2f, 0f );
				return;
			}

			var mousePos = new Vector2( Main.mouseX, Main.mouseY );

			if( (this.LastAimMousePosition - mousePos).LengthSquared() > 1f ) {
				this.AimElapsed = Math.Max( this.AimElapsed - 0.5f, 0f );
			} else {
				this.AimElapsed = Math.Min( this.AimElapsed + 1f, 60f );
			}

			this.LastAimMousePosition = mousePos;
		}


		////////////////

		public void ApplyAimStateShakeAmount() {
DebugHelpers.Print( "aiming", ""+ this.AimElapsed );
			if( this.AimElapsed >= 60 ) {
				return;
			}

			this.player.itemRotation += BigIronPlayer.GetAimShakeRadOffset() * 0.01f;
		}


		public void ApplyAimStateShakeDamage( ref int damage ) {
			if( this.AimElapsed >= 60 ) {
				return;
			}

			UnifiedRandom rand = TmlHelpers.SafelyGetRand();

			damage = (int)(rand.NextFloat() * (float)(damage - 1)) + 1;
		}
	}
}
