using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using ModLibsCore.Services.Timers;
using ModLibsGeneral.Libraries.Audio;
using TheMadRanger.Items.Weapons;


namespace TheMadRanger.Items {
	public partial class SpeedloaderItem : ModItem {
		private static string GetReloadingTimerName( int plrWho ) {
			return "TheMadRangerSpeedloaderLoad_" + plrWho;
		}


		////

		public static bool IsReloading( int plrWho ) {
			string timerName = SpeedloaderItem.GetReloadingTimerName( plrWho );
			return Timers.GetTimerTickDuration( timerName ) > 0;
		}



		////////////////

		public bool AttemptReload( Player player ) {
			int plrWho = player.whoAmI;
			string result;

			bool Reload() {
				Player plr = Main.player[plrWho];
				if( !TheMadRangerItem.IsAmmoSourceAvailable(plr, true, out result) ) {
					if( plrWho == Main.myPlayer ) {
						Main.NewText( result, Color.Yellow );
					}
					return false;
				}

				this.LoadedRounds = TheMadRangerItem.CylinderCapacity;
				return true;
			}

			//

			if( !TheMadRangerItem.IsAmmoSourceAvailable(player, true, out result) ) {
				if( plrWho == Main.myPlayer ) {
					Main.NewText( result, Color.Yellow );
				}
				return false;
			}

			var myplayer = player.GetModPlayer<TMRPlayer>();
			if( myplayer.GunHandling.IsAnimating ) {
				return false;
			}

			string timerName = SpeedloaderItem.GetReloadingTimerName( plrWho );
			if( Timers.GetTimerTickDuration(timerName) > 0 ) {
				return false;
			}

			//

			SoundLibraries.PlaySound( TMRMod.Instance, "RevolverReloadBegin", player.Center, 0.5f );

			int duration = TMRConfig.Instance.Get<int>( nameof(TMRConfig.SpeedloaderLoadTickDuration) );
			Timers.SetTimer( timerName, duration, false, () => {
				Reload();
				return false;
			} );

			//

			return true;
		}

		////

		public void TransferRoundsOut( Player player ) {
			this.LoadedRounds = 0;

			SoundLibraries.PlaySound( TMRMod.Instance, "RevolverReloadRound", player.Center, 0.5f );
		}
	}
}
