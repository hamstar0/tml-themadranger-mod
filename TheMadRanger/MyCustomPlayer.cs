using System;
using Terraria;
using Terraria.ID;
using ModLibsCore.Classes.Errors;
using ModLibsCore.Classes.PlayerData;
using ModLibsCore.Libraries.Debug;
using ModLibsNet.Services.Network;
using ModLibsInterMod.Libraries.Mods.APIMirrors.ModHelpersAPIMirrors;


namespace TheMadRanger {
	class TMRCustomPlayer : CustomPlayerData {
		protected TMRCustomPlayer() { }


		protected override void OnEnter( bool isCurrentPlayer, object data ) {
			if( !isCurrentPlayer ) {
				return;
			}
			
			if( Main.netMode == NetmodeID.MultiplayerClient ) {
				ClientCursorData.StartBroadcastingMyCursorPosition();
			}
			
			InboxAPIMirrorsLibraries.SetMessage(
				"TheMadRangerInfo",
				"Want more control over gun reloading? Be sure to bind a key for reloading. Also see the mod's configs for more options.",
				false
			);
		}

		protected override object OnExit() {
			if( Main.netMode == 1 && this.PlayerWho == Main.myPlayer ) {
				ClientCursorData.StopBroadcastingMyCursorPosition();
			}
			return base.OnExit();
		}
	}
}

