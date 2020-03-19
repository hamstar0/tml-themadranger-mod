using Terraria;
using Terraria.ModLoader;
using TheMadRanger.Helpers.Misc;


namespace TheMadRanger.Items.Weapons {
	public partial class TheMadRangerItem : ModItem {
		public void OpenCylinder( Player player ) {
			SoundHelpers.PlaySound( "RevolverReloadBegin", player.Center, 0.5f );
		}

		public void CloseCylinder( Player player ) {
			SoundHelpers.PlaySound( "RevolverDryFire", player.Center, 0.2f );
		}


		////

		public (int Shells, int Rounds) UnloadCylinder( Player player ) {
			int liveRounds = 0;
			int shells = 0;

			for( int i=0; i<this.Cylinder.Length; i++ ) {
				if( this.Cylinder[i] >= 1 ) {
					liveRounds += 1;	//this.Cylinder[i];?
				} else if( this.Cylinder[i] <= -1 ) {
					shells += 1;
				}

				this.Cylinder[i] = 0;
			}

			return (shells, liveRounds);
		}


		////

		public bool InsertRound( Player player ) {
			bool hasInserted = false;
			int initPos = this.CylinderIdx;

			do {
				if( this.CylinderInsertOnce() ) {
					hasInserted = true;
					SoundHelpers.PlaySound( "RevolverReloadRound", player.Center, 0.5f );
					break;
				}
			} while( this.CylinderIdx != initPos );

			return hasInserted;
		}


		////////////////

		private bool CylinderShootOnce() {
			int roundSlot = this.Cylinder[ this.CylinderIdx ];

			if( roundSlot == 1 ) {
				this.Cylinder[this.CylinderIdx] = -1;
			}

			this.CylinderIdx = (this.CylinderIdx + 1) % this.Cylinder.Length;

			return roundSlot == 1;
		}

		////

		private bool CylinderInsertOnce() {
			int roundSlot = this.Cylinder[ this.CylinderIdx ];

			if( roundSlot == 0 ) {
				this.Cylinder[this.CylinderIdx] = 1;
			}

			this.CylinderIdx = this.CylinderIdx - 1;
			if( this.CylinderIdx < 0 ) {
				this.CylinderIdx = this.Cylinder.Length - 1;
			}

			return roundSlot == 0;
		}
	}
}