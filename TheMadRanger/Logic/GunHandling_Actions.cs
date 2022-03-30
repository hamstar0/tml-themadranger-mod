using System;
using Terraria;
using Terraria.ID;
using ModLibsCore.Libraries.Debug;
using ModLibsGeneral.Libraries.Audio;
using TheMadRanger.Items.Weapons;
using TheMadRanger.Items;


namespace TheMadRanger.Logic {
	partial class GunHandling {
		/// <summary>
		/// Begins a gun firing recoil sequence. Recoil does not affect repeat firing (at present).
		/// </summary>
		/// <param name="addedRotationDegrees"></param>
		public void BeginRecoil( float addedRotationDegrees ) {
//Main.NewText( $"Recoil MiscAddedRotationDegrees:{this.MiscAddedRotationDegrees} + {addedRotationDegrees}" );
			this.MiscAddedRotationDegrees = addedRotationDegrees;
			this.RecoilDuration = 17;
		}

		/// <summary>
		/// Begins a gun reload sequence, if the gun or player allows. Not synced.
		/// </summary>
		/// <param name="plr"></param>
		/// <param name="mygun"></param>
		/// <returns></returns>
		public bool BeginReload_If( Player plr, TheMadRangerItem mygun, bool forceReload ) {
			if( this.IsReloading ) {
				return false;
			}
			
			if( SpeedloaderItem.IsReloading(plr.whoAmI) ) {
				return false;
			}
			
			if( mygun.IsCylinderFull() ) {
				if( forceReload ) {
					mygun.UnloadCylinder( plr );
				} else {
					return false;
				}
			}

			//

			mygun.OpenCylinder( plr );

			//

			this.ReloadDuration = TMRConfig.Instance.Get<int>( nameof(TMRConfig.ReloadInitTickDuration) );

			this.IsQuickDrawReady = true;
			
			return true;
		}

		/// <summary>
		/// Begins a gun holstering sequence. Currently ignores all other sequences (needs testing).
		/// </summary>
		/// <param name="plr"></param>
		public void BeginHolster( Player plr ) {
			this.IsQuickDrawReady = true;

			if( Main.netMode != NetmodeID.Server ) {
				this.HolsterDuration = TMRConfig.Instance.Get<int>( nameof(TMRConfig.HolsterTwirlTickDuration) );
//Main.NewText( $"Holster plr:{plr.name}, IsQuickDrawReady:{this.IsQuickDrawReady}" );

				SoundLibraries.PlaySound( TMRMod.Instance, "RevolverTwirl", plr.Center, 0.65f );
			}
		}


		////

		/// <summary>
		/// Interrupts any reloading of a player's currently-held gun.
		/// </summary>
		/// <param name="plr"></param>
		public void StopReloading( Player plr, TheMadRangerItem mygun ) {
			mygun.CloseCylinder( plr );

			this.ReloadDuration = 0;
			this.ReloadingRounds = false;
		}
	}
}
