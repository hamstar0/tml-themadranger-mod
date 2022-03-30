using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Utilities;
using ModLibsCore.Libraries.Debug;
using ModLibsCore.Libraries.TModLoader;
using TheMadRanger.NetProtocols;


namespace TheMadRanger.Logic {
	partial class PlayerAimMode {
		public static float ComputeAimShakeRadiansOffsetWithinCone() {
			var config = TMRConfig.Instance;
			UnifiedRandom rand = TmlLibraries.SafelyGetRand();
			float radRange = MathHelper.ToRadians( config.Get<float>( nameof(TMRConfig.UnaimedConeDegreesRange) ) );

			return (rand.NextFloat() * radRange) - (radRange * 0.5f);
		}



		////////////////

		public bool IsModeActive {
			get {
				var config = TMRConfig.Instance;
				int aimDuration = config.Get<int>( nameof(config.AimModeActivationTickDuration) );
				
				return this.AimElapsed >= aimDuration;
			}
		}

		public bool IsModeActivating =>
			this.AimElapsed > 0
			&& this.AimElapsed >= this.PrevAimElapsed;

		public bool IsApplyingModeLock_Local =>
			Main.mouseRight
			&& !this.IsQuickDrawActive;
		public bool IsApplyingModeLock_NonLocal =>
			this.WasApplyingModeLock_Client
			&& !this.IsQuickDrawActive;

		public bool IsModeLocked =>
			this.IsModeActive
			&& (this.IsApplyingModeLock_Local || this.IsApplyingModeLock_NonLocal);

		////

		public bool IsQuickDrawActive => this.QuickDrawDuration > 0;

		////

		public float AimPercent {
			get {
				int aimDuration = TMRConfig.Instance.Get<int>( nameof(TMRConfig.AimModeActivationTickDuration) );
				return (float)this.AimElapsed / (float)aimDuration;
			}
		}


		////////////////
		
		private float PrevAimElapsed = 0f;
		public float AimElapsed { get; private set; } = 0f;

		private int QuickDrawDuration = 0;
		
		internal bool WasApplyingModeLock_Client { get; private set; } = false;



		////////////////

		public void PreUpdateAimState( Player plr ) {
			this.PrevAimElapsed = this.AimElapsed;
			
			if( this.QuickDrawDuration > 1 ) {
				int aimDuration = TMRConfig.Instance.Get<int>( nameof(TMRConfig.AimModeActivationTickDuration) );

				this.QuickDrawDuration--;
				this.AimElapsed = aimDuration + 2f;	// "cheat" until quick draw mode ends
			} else if( this.QuickDrawDuration == 1 ) {
				this.QuickDrawDuration = 0;
				this.AimElapsed = 0f;
			}
		}


		 private int _AimModeSyncTimer = 0;

		public void PostUpdateAimState( Player plr ) {
			if( Main.netMode == NetmodeID.MultiplayerClient ) {
				if( plr.whoAmI == Main.myPlayer ) {
					if( this._AimModeSyncTimer-- <= 0 ) {
						this._AimModeSyncTimer = 7;

						GunAimStateSyncPacket.BroadcastFromLocalPlayer();
					}
				}
			}
		}


		////

		public void UpdateEquippedAimState( Player plr, Item prevHeldItem ) {
			var myplayer = plr.GetModPlayer<TMRPlayer>();

			// On fresh re-equip
			if( prevHeldItem != plr.HeldItem ) {
				if( !myplayer.GunHandling.IsAnimating ) {
					this.AttemptQuickDrawMode( plr );
				}
			}

			//

			// Animations cancel aim mode
			if( myplayer.GunHandling.IsAnimating ) {
				this.AimElapsed = 0f;

				return;
			}

			//

			this.UpdateEquippedAimStateValue_If( plr );
		}

		public void UpdateUnequippedAimState() {
			this.AimElapsed = 0f;
			this.PrevAimElapsed = 0f;
			this.QuickDrawDuration = 0;
		}


		////////////////

		public float GetAimStateShakeRadiansOffset( bool isIdling ) {
			if( this.IsModeActive ) {
				return 0f;
			}

			float rads = PlayerAimMode.ComputeAimShakeRadiansOffsetWithinCone();
			if( isIdling ) {
				rads *= 0.03f;
			}

			return rads;
		}


		public int GetAimStateShakeDamage( int damage ) {
			if( this.IsModeActive ) {
				return damage;
			}

			var config = TMRConfig.Instance;
			UnifiedRandom rand = TmlLibraries.SafelyGetRand();
			float maxAimDmg = config.Get<int>( nameof(TMRConfig.MaximumAimedGunDamage) );
			float minUnaimDmg = config.Get<int>( nameof(TMRConfig.MinimumUnaimedGunDamage) );
			float maxUnaimDmg = config.Get<int>( nameof(TMRConfig.MaximumUnaimedGunDamage) );
			float dmgPercent = (float)damage / maxAimDmg;

			float baseDmg = maxUnaimDmg * dmgPercent;
			float dmgMinPercent = minUnaimDmg / maxUnaimDmg;
			float dmgRand = dmgMinPercent + (rand.NextFloat() * (1f - dmgMinPercent) );
			
			int newDmg = (int)(dmgRand * (baseDmg - 1f)) + 1;
			return newDmg;
		}


		////////////////

		internal void SyncAimState( float aimElapsed, bool wasApplyingModeLock ) {
			this.AimElapsed = aimElapsed;
			this.WasApplyingModeLock_Client = wasApplyingModeLock;
		}
	}
}
