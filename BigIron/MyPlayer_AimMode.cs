using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.Utilities;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.TModLoader;


namespace BigIron {
	partial class BigIronPlayer : ModPlayer {
		public static float ComputeAimShakeMaxConeRadians() {
			UnifiedRandom rand = TmlHelpers.SafelyGetRand();
			float radRange = MathHelper.ToRadians( BigIronConfig.Instance.UnaimedConeDegreesRange );

			return (rand.NextFloat() * radRange) - (radRange * 0.5f);
		}



		////////////////

		public bool IsAimingModeActive => this.AimElapsed >= BigIronConfig.Instance.TickDurationUntilAimModeWhileIdling;


		////////////////

		private float AimElapsed = 0f;

		private Vector2 LastAimMousePosition = default( Vector2 );



		////////////////

		private void CheckEquippedAimState() {
			if( this.player.velocity.LengthSquared() > 1f ) {
				this.AimElapsed = Math.Max( this.AimElapsed - 4f, 0f );
				return;
			}

			var mousePos = new Vector2( Main.mouseX, Main.mouseY );

			if( (this.LastAimMousePosition - mousePos).LengthSquared() > 1f ) {
				this.AimElapsed = Math.Max( this.AimElapsed - 0.5f, 0f );
			} else {
				this.AimElapsed = Math.Min( this.AimElapsed + 1f, (float)BigIronConfig.Instance.TickDurationUntilAimModeWhileIdling );
			}

			this.LastAimMousePosition = mousePos;
		}

		private void CheckUnequippedAimState() {
			this.AimElapsed = 0f;
		}


		////////////////

		public float GetAimStateShakeAddedRadians( bool isIdling ) {
			if( this.IsAimingModeActive ) {
				return 0f;
			}

			float rads = BigIronPlayer.ComputeAimShakeMaxConeRadians();
			if( isIdling ) {
				rads *= 0.02f;
			}

			return rads;
		}


		public int GetAimStateShakeDamage( int damage ) {
			if( this.IsAimingModeActive ) {
				return damage;
			}

			UnifiedRandom rand = TmlHelpers.SafelyGetRand();

			return (int)(rand.NextFloat() * (float)(damage - 1)) + 1;
		}
	}
}
