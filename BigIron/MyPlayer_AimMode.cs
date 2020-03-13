using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.TModLoader;
using Terraria.Utilities;


namespace BigIron {
	partial class BigIronPlayer : ModPlayer {
		public static float GetAimShakeAddedRadians() {
			UnifiedRandom rand = TmlHelpers.SafelyGetRand();
			float radRange = MathHelper.ToRadians( BigIronConfig.Instance.UnaimedConeDegreesRange );

			return (rand.NextFloat() * radRange) - (radRange * 0.5f);
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

		public float GetAimStateShakeAddedRadians() {
			if( this.AimElapsed >= 60 ) {
				return 0f;
			}

			return BigIronPlayer.GetAimShakeAddedRadians() * 0.01f;
		}


		public int GetAimStateShakeDamage( int damage ) {
			if( this.AimElapsed >= 60 ) {
				return damage;
			}

			UnifiedRandom rand = TmlHelpers.SafelyGetRand();

			return (int)(rand.NextFloat() * (float)(damage - 1)) + 1;
		}
	}
}
