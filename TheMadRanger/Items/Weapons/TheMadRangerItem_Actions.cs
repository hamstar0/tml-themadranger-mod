using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;


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
				if( this.FireSound == null ) {
					this.FireSound = TMRMod.Instance.GetLegacySoundSlot(
						Terraria.ModLoader.SoundType.Custom,
						"Sounds/Custom/RevolverFire"
					).WithVolume( 0.2f );
				}

				Main.PlaySound( (LegacySoundStyle)this.FireSound, player.Center );
				hasShot = true;
			} else {
				if( this.DryFireSound == null ) {
					this.DryFireSound = TMRMod.Instance.GetLegacySoundSlot(
						Terraria.ModLoader.SoundType.Custom,
						"Sounds/Custom/RevolverDryFire"
					).WithVolume( 0.2f );
				}

				Main.PlaySound( (LegacySoundStyle)this.DryFireSound, player.Center );
				hasShot = false;
			}

			this.ElapsedTimeSinceLastShotAttempt = 0;

			return hasShot;
		}

		////

		public bool OpenCylinder( Player player ) {
			if( this.ReloadBeginSound == null ) {
				this.ReloadBeginSound = TMRMod.Instance.GetLegacySoundSlot(
					Terraria.ModLoader.SoundType.Custom,
					"Sounds/Custom/RevolverReloadBegin"
				).WithVolume( 0.5f );
			}

			Main.PlaySound( (LegacySoundStyle)this.ReloadBeginSound, player.Center );

			this.IsCylinderOpen = true;
			return true;
		}

		public bool CloseCylinder( Player player ) {
			if( this.ReloadEndSound == null ) {
				this.ReloadEndSound = TMRMod.Instance.GetLegacySoundSlot(
					Terraria.ModLoader.SoundType.Custom,
					"Sounds/Custom/RevolverDryFire"
				).WithVolume( 0.2f );
			}

			Main.PlaySound( (LegacySoundStyle)this.ReloadEndSound, player.Center );

			this.IsCylinderOpen = false;
			return true;
		}

		////

		public bool ReloadRound( Player player ) {
			int initPos = this.CylinderPos;

			do {
				if( this.CylinderReload() ) {
					if( this.ReloadRoundSound == null ) {
						this.ReloadRoundSound = TMRMod.Instance.GetLegacySoundSlot(
							Terraria.ModLoader.SoundType.Custom,
							"Sounds/Custom/RevolverReloadRound"
						).WithVolume( 1f );
					}

					Main.PlaySound( (LegacySoundStyle)this.ReloadRoundSound, player.Center );

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