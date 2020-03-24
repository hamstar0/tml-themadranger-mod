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

			for( int i = 0; i < this.Cylinder.Length; i++ ) {
				if( this.Cylinder[i] >= 1 ) {
					liveRounds += 1;    //this.Cylinder[i];?
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
				if( this.CylinderInsertOnce() == 0 ) {
					hasInserted = true;
					SoundHelpers.PlaySound( "RevolverReloadRound", player.Center, 0.5f );
					break;
				}
			} while( this.CylinderIdx != initPos );

			return hasInserted;
		}

		public bool InsertSpeedloader( Player player ) {
			if( !this.IsCylinderEmpty() ) {
				return false;
			}

			int speedloaderType = ModContent.ItemType<SpeedloaderItem>();

			for( int i = 0; i < player.inventory.Length; i++ ) {
				Item item = player.inventory[i];
				if( item == null || item.IsAir || item.type != speedloaderType ) {
					continue;
				}

				var myitem = item.modItem as SpeedloaderItem;
				if( myitem == null || myitem.LoadedRounds == 0 ) {
					continue;
				}

				return this.InsertSpeedloader( player, item );
			}

			return false;
		}

		private bool InsertSpeedloader( Player player, Item speedloaderItem ) {
			int inserted = 0;
			var myitem = speedloaderItem.modItem as SpeedloaderItem;

			for( int j = 0; j < myitem.LoadedRounds; j++ ) {
				if( this.CylinderInsertOnce() == 0 ) {
					inserted += 1;
				}
			}

			if( inserted > 0 ) {
				myitem.TransferRounds( player );
			}

			return inserted > 0;
		}


		////////////////

		private bool CylinderShootOnce() {
			int roundSlot = this.Cylinder[this.CylinderIdx];

			if( roundSlot == 1 ) {
				this.Cylinder[this.CylinderIdx] = -1;
			}

			this.RotateCylinder( 1 );

			return roundSlot == 1;
		}

		////

		private int CylinderInsertOnce() {
			int roundSlot = this.Cylinder[this.CylinderIdx];

			if( roundSlot == 0 ) {
				this.Cylinder[this.CylinderIdx] = 1;
			}

			this.RotateCylinder( -1 );

			return roundSlot;
		}


		////////////////

		private void RotateCylinder( int dir ) {
			dir %= this.Cylinder.Length;

			if( dir > 0 ) {
				this.CylinderIdx = (this.CylinderIdx + dir) % this.Cylinder.Length;
			} else {
				this.CylinderIdx = this.CylinderIdx + dir;
				if( this.CylinderIdx < 0 ) {
					this.CylinderIdx = this.Cylinder.Length - 1;
				}
			}
		}
	}
}