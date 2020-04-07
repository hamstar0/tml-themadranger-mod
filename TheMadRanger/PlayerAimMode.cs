using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Utilities;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.TModLoader;


namespace TheMadRanger {
	partial class PlayerAimMode {
		public static float ComputeAimShakeMaxConeRadians() {
			UnifiedRandom rand = TmlHelpers.SafelyGetRand();
			float radRange = MathHelper.ToRadians( TMRConfig.Instance.UnaimedConeDegreesRange );

			return (rand.NextFloat() * radRange) - (radRange * 0.5f);
		}



		////////////////

		public bool IsModeActive => this.AimElapsed >= TMRConfig.Instance.AimModeActivationThreshold;

		public bool IsModeActivating => this.AimElapsed > 0 && this.AimElapsed >= this.PrevAimElapsed;

		public bool IsQuickDrawActive => this.QuickDrawDuration > 0;

		////

		public float AimPercent => (float)this.AimElapsed / (float)TMRConfig.Instance.AimModeActivationThreshold;


		////////////////

		private float PrevAimElapsed = 0f;
		private float AimElapsed = 0f;

		private int QuickDrawDuration = 0;

		private Vector2 LastAimMousePosition = default( Vector2 );



		////////////////

		public void UpdateAimState( Player plr ) {
			this.PrevAimElapsed = this.AimElapsed;

			if( this.QuickDrawDuration > 1 ) {
				this.QuickDrawDuration--;
				this.AimElapsed = TMRConfig.Instance.AimModeActivationThreshold + 2f;
			} else if( this.QuickDrawDuration == 1 ) {
				this.QuickDrawDuration = 0;
				this.AimElapsed = 0;
			}
		}

		public void UpdateEquippedAimState( Player plr, Item prevHeldItem ) {
			var myplayer = plr.GetModPlayer<TMRPlayer>();

			// On fresh re-equip
			if( prevHeldItem != plr.HeldItem ) {
				if( !myplayer.GunHandling.IsAnimating ) {
					this.AttemptQuickDrawMode( plr );
				}
			}

			// Animations cancel aim mode
			if( myplayer.GunHandling.IsAnimating ) {
				this.AimElapsed = 0f;
				return;
			}

			this.UpdateEquippedAimStateValue( plr );
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
			float maxAimDmg = TMRConfig.Instance.MaximumAimedGunDamage;
			float minUnaimDmg = TMRConfig.Instance.MinimumUnaimedGunDamage;
			float maxUnaimDmg = TMRConfig.Instance.MaximumUnaimedGunDamage;
			float dmgPercent = (float)damage / maxAimDmg;

			float baseDmg = maxUnaimDmg * dmgPercent;
			float dmgMinPercent = minUnaimDmg / maxUnaimDmg;
			float dmgRand = dmgMinPercent + (rand.NextFloat() * (1f - dmgMinPercent) );
			
			int newDmg = (int)(dmgRand * (baseDmg - 1f)) + 1;
			return newDmg;
		}
	}
}
