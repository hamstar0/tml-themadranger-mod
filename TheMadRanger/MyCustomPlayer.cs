using System;
using Terraria;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Classes.PlayerData;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Services.Network;


namespace TheMadRanger {
	class TMRCustomPlayer : CustomPlayerData {
		protected TMRCustomPlayer() { }


		protected override void OnEnter( object data ) {
			if( Main.netMode == 1 && this.PlayerWho == Main.myPlayer ) {
				Client.StartBroadcastingMyCursorPosition();
			}
		}

		protected override object OnExit() {
			Client.StopBroadcastingMyCursorPosition();
			return base.OnExit();
		}
	}
}

