using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModLibsCore.Classes.Errors;
using ModLibsCore.Classes.PlayerData;
using ModLibsCore.Libraries.Debug;
using ModLibsNet.Services.Network;


namespace TheMadRanger {
	class TMRCustomPlayer : CustomPlayerData {
		/*private static void MessageAboutReloadKey() {
			Messages.MessagesAPI.AddMessagesCategoriesInitializeEvent( () => {
				Messages.MessagesAPI.AddMessage(
					title: "Reminder: Bind your gun's reload key",
					description: "Want more control over gun reloading? Be sure to bind it to a key. "
						+ "Also see the mod's configs for more options.",
					modOfOrigin: TMRMod.Instance,
					id: "TheMadRangerInfo",
					parentMessage: Messages.MessagesAPI.ModInfoCategoryMsg
				);
			} );
		}*/



		////////////////

		protected TMRCustomPlayer() { }


		protected override void OnEnter( bool isCurrentPlayer, object data ) {
			if( !isCurrentPlayer ) {
				return;
			}
			
			if( Main.netMode == NetmodeID.MultiplayerClient ) {
				ClientCursorData.StartBroadcastingMyCursorPosition();
			}

			/*if( ModLoader.GetMod("Messages") != null ) {
				TMRCustomPlayer.MessageAboutReloadKey();
			}*/
		}

		protected override object OnExit() {
			if( Main.netMode == 1 && this.PlayerWho == Main.myPlayer ) {
				ClientCursorData.StopBroadcastingMyCursorPosition();
			}
			return base.OnExit();
		}
	}
}

