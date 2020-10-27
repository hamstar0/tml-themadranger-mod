using System;
using Terraria;
using Terraria.ID;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Classes.PlayerData;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Services.Network;
using HamstarHelpers.Services.Messages.Inbox;


namespace TheMadRanger {
	class TMRCustomPlayer : CustomPlayerData {
		protected TMRCustomPlayer() { }


		protected override void OnEnter( bool isCurrentPlayer, object data ) {
			if( !isCurrentPlayer ) {
				return;
			}

			if( Main.netMode == NetmodeID.MultiplayerClient ) {
				Client.StartBroadcastingMyCursorPosition();
			}

			InboxMessages.SetMessage(
				"TheMadRangerInfo",
				"Want more control over reloading? Be sure to bind gun reloading for The Mad Ranger mod to a button of your choice. Also see the mod's configs for more options.",
				false
			);
		}

		protected override object OnExit() {
			if( Main.netMode == 1 && this.PlayerWho == Main.myPlayer ) {
				Client.StopBroadcastingMyCursorPosition();
			}
			return base.OnExit();
		}
	}
}

