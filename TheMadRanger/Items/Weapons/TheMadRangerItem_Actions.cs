using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using TheMadRanger.Helpers.Misc;


namespace TheMadRanger.Items.Weapons {
	public partial class TheMadRangerItem : ModItem {
		internal bool AttemptShot( Player player, out bool wantsReload ) {
			wantsReload = false;

			if( this.ElapsedTimeSinceLastShotAttempt >= 60 ) {
				if( !this.Cylinder.Any(c => c >= 1) ) {
					wantsReload = true;
					return false;
				}
			}

			bool hasShot = false;

			if( this.CylinderShoot() ) {
				SoundHelpers.PlaySound( "RevolverFire", player.Center, 0.2f );
				hasShot = true;
			} else {
				SoundHelpers.PlaySound( "RevolverDryFire", player.Center, 0.2f );
				hasShot = false;
			}

			this.ElapsedTimeSinceLastShotAttempt = 0;

			return hasShot;
		}


		////////////////

		public bool ReloadRound( Player player ) {
			int initPos = this.CylinderPos;
			bool hasReloaded = false;

			do {
				if( this.CylinderReload() ) {
					hasReloaded = true;
					SoundHelpers.PlaySound( "RevolverReloadRound", player.Center, 1f );
					break;
				}
			} while( this.CylinderPos != initPos );

			return hasReloaded;
		}

		////

		public int UnloadCylinder( Player player ) {
			int liveRounds = 0;

			for( int i=0; i<this.Cylinder.Length; i++ ) {
				if( this.Cylinder[i] >= 1 ) {
					liveRounds += this.Cylinder[i];
				}
				this.Cylinder[i] = 0;
			}

			return liveRounds;
		}

		public bool CloseCylinder( Player player ) {
			SoundHelpers.PlaySound( "RevolverDryFire", player.Center, 0.2f );

			return true;
		}


		////////////////

		private bool CylinderShoot() {
			int roundSlot = this.Cylinder[ this.CylinderPos ];

			if( roundSlot == 1 ) {
				this.Cylinder[this.CylinderPos] = -1;
			}

			this.CylinderPos = (this.CylinderPos + 1) % 6;

			return roundSlot == 1;
		}

		////

		private bool CylinderReload() {
			int roundSlot = this.Cylinder[ this.CylinderPos ];

			if( roundSlot == 0 ) {
				this.Cylinder[this.CylinderPos] = 1;
			}

			this.CylinderPos = (this.CylinderPos + 1) % 6;

			return roundSlot == 0;
		}
	}
}