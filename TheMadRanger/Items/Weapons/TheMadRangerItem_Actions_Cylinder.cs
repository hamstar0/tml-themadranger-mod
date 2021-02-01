using System.Linq;
using Terraria;
using Terraria.ModLoader;
using HamstarHelpers.Helpers.Audio;


namespace TheMadRanger.Items.Weapons {
	public partial class TheMadRangerItem : ModItem {
		public bool IsCylinderFull() {
			return this.Cylinder.All( c => c == 1 );
		}

		public bool IsCylinderEmpty() {
			return this.Cylinder.All( c => c == 0 );
		}


		////////////////

		public void OpenCylinder( Player player ) {
			SoundHelpers.PlaySound( TMRMod.Instance, "RevolverReloadBegin", player.Center, 0.5f );
		}

		public void CloseCylinder( Player player ) {
			SoundHelpers.PlaySound( TMRMod.Instance, "RevolverDryFire", player.Center, 0.2f );
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

		public bool InsertRound( Player player, bool playSound=true ) {
			bool hasInserted = false;
			int initPos = this.CylinderIdx;

			do {
				if( this.CylinderInsertRound() ) {
					hasInserted = true;

					if( playSound ) {
						SoundHelpers.PlaySound( TMRMod.Instance, "RevolverReloadRound", player.Center, 0.5f );
					}

					break;
				}
			} while( this.CylinderIdx != initPos );	// full cylinder loop

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

				return this.InsertSpeedloaderIntoEmptyGun( player, item );
			}

			return false;
		}

		private bool InsertSpeedloaderIntoEmptyGun( Player player, Item speedloaderItem ) {
			int inserted = 0;
			var speedloader = speedloaderItem.modItem as SpeedloaderItem;

			for( int j = 0; j < speedloader.LoadedRounds; j++ ) {
				if( this.CylinderInsertRound() ) {
					inserted += 1;
				}
			}

			if( inserted > 0 ) {
				speedloader.TransferRoundsOut( player );
			}

			return inserted > 0;
		}

		////

		internal void InsertAllOnRespawn( Player player ) {
			while( this.InsertRound(player, false) ) { }
		}


		////////////////

		public bool CylinderCanShootNow() {
			return this.Cylinder[ this.CylinderIdx ] == 1;
		}


		////////////////

		private bool CylinderAttemptShoot() {
			bool hasShot = false;
			int roundSlot = this.Cylinder[ this.CylinderIdx ];

			if( roundSlot == 1 ) {
				hasShot = true;
				this.Cylinder[ this.CylinderIdx ] = -1;	// -1 = empty shell casing is now in this slot
			}

			this.RotateCylinder( 1 );
			return hasShot;
		}

		////

		private bool CylinderInsertRound() {
			bool hasInsertedRound = false;
			int roundSlot = this.Cylinder[ this.CylinderIdx ];

			if( roundSlot == 0 ) {
				this.Cylinder[ this.CylinderIdx ] = 1;
				hasInsertedRound = true;
			}

			this.RotateCylinder( -1 );

			return hasInsertedRound;
		}


		////////////////

		public void RotateCylinder( int dir ) {
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