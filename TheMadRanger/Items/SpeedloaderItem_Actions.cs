using System;
using Terraria;
using Terraria.ModLoader;
using HamstarHelpers.Services.Timers;
using TheMadRanger.Items.Weapons;
using TheMadRanger.Helpers.Misc;


namespace TheMadRanger.Items {
	partial class SpeedloaderItem : ModItem {
		public bool AttemptReload( Player player ) {
			int plrWho = player.whoAmI;

			bool Reload() {
				Player plr = Main.player[plrWho];
				if( !TheMadRangerItem.IsAmmoSourceAvailable( plr, true ) ) {
					return false;
				}

				this.LoadedRounds = TheMadRangerItem.CylinderCapacity;
				return true;
			}

			//

			if( !TheMadRangerItem.IsAmmoSourceAvailable(player, true) ) {
				return false;
			}

			var myplayer = player.GetModPlayer<TMRPlayer>();

			if( myplayer.GunHandling.IsAnimating ) {
				return false;
			}

			string timerName = "TheMadRangerSpeedloaderLoad_" + plrWho;

			if( Timers.GetTimerTickDuration(timerName) > 0 ) {
				return false;
			}

			SoundHelpers.PlaySound( "RevolverReloadBegin", player.Center, 0.5f );

			Timers.SetTimer( timerName, TMRConfig.Instance.SpeedloaderLoadTickDuration, false, () => {
				Reload();
				return false;
			} );

			return true;
		}

		////

		public void TransferRoundsOut( Player player ) {
			this.LoadedRounds = 0;

			SoundHelpers.PlaySound( "RevolverReloadRound", player.Center, 0.5f );
		}
	}
}
