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
				if( !this.Cylinder.Any(c=>c) ) {
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

		////

		public bool OpenCylinder( Player player ) {
			SoundHelpers.PlaySound( "RevolverReloadBegin", player.Center, 0.5f );

			this.IsCylinderOpen = true;
			return true;
		}

		public bool CloseCylinder( Player player ) {
			SoundHelpers.PlaySound( "RevolverDryFire", player.Center, 0.2f );

			this.IsCylinderOpen = false;
			return true;
		}

		////

		public bool ReloadRound( Player player ) {
			int initPos = this.CylinderPos;

			do {
				if( !this.CylinderReload() ) {
					SoundHelpers.PlaySound( "RevolverReloadRound", player.Center, 1f );

					return true;
				}
			} while( this.CylinderPos != initPos );

			return false;
		}


		////////////////
		
		private bool CylinderShoot() {
			bool canShoot = this.Cylinder[ this.CylinderPos ];

			this.Cylinder[ this.CylinderPos ] = false;
			this.CylinderPos = (this.CylinderPos + 1) % 6;

			return canShoot;
		}

		private bool CylinderReload() {
			bool isLoaded = this.Cylinder[ this.CylinderPos ];

			this.Cylinder[ this.CylinderPos ] = true;
			this.CylinderPos = (this.CylinderPos + 1) % 6;

			return isLoaded;
		}
	}
}