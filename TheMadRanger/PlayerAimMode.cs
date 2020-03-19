using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Utilities;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.TModLoader;


namespace TheMadRanger {
	class PlayerAimMode {
		public static float ComputeAimShakeMaxConeRadians() {
			UnifiedRandom rand = TmlHelpers.SafelyGetRand();
			float radRange = MathHelper.ToRadians( TMRConfig.Instance.UnaimedConeDegreesRange );

			return (rand.NextFloat() * radRange) - (radRange * 0.5f);
		}



		////////////////

		public bool IsModeActive => this.AimElapsed >= TMRConfig.Instance.AimModeActivationDurationWhileIdling;

		public bool IsModeBeingActivated => this.AimElapsed > 0 && this.AimElapsed >= this.PrevAimElapsed;


		////////////////

		private float PrevAimElapsed = 0f;
		private float AimElapsed = 0f;

		private Vector2 LastAimMousePosition = default( Vector2 );



		////////////////

		public void CheckEquippedAimState( Player plr ) {
			this.PrevAimElapsed = this.AimElapsed;

			var myplayer = plr.GetModPlayer<TMRPlayer>();

			// Animations cancel aim mode
			if( myplayer.GunAnim.IsAnimating ) {
				this.AimElapsed = 0f;
				return;
			}

			// Player is moving
			if( plr.velocity.LengthSquared() > 1f ) {
				this.AimElapsed = Math.Max( this.AimElapsed - 2f, 0f );
				return;
			}

			var mousePos = new Vector2( Main.mouseX, Main.mouseY );

			// Mouse is moving
			if( (this.LastAimMousePosition - mousePos).LengthSquared() > 1f ) {
				this.AimElapsed = Math.Max( this.AimElapsed - 0.5f, 0f );
			} else {
				int maxDuration = TMRConfig.Instance.AimModeActivationDurationWhileIdling + 2;	// Added buffer for slight aim tweaks
				this.AimElapsed = Math.Min( this.AimElapsed + 1f, (float)maxDuration );
			}

			this.LastAimMousePosition = mousePos;
		}

		public void CheckUnequippedAimState() {
			this.AimElapsed = 0f;
			this.PrevAimElapsed = 0f;
		}


		////////////////

		public float GetAimStateShakeAddedRadians( bool isIdling ) {
			if( this.IsModeActive ) {
				return 0f;
			}

			float rads = PlayerAimMode.ComputeAimShakeMaxConeRadians();
			if( isIdling ) {
				rads *= 0.03f;
			}

			return rads;
		}


		public int GetAimStateShakeDamage( int damage ) {
			if( this.IsModeActive ) {
				return damage;
			}

			UnifiedRandom rand = TmlHelpers.SafelyGetRand();

			return (int)(rand.NextFloat() * (float)(damage - 1)) + 1;
		}
	}
}
