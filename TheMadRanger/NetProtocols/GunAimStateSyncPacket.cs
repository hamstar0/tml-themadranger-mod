using System;
using Terraria;
using Terraria.ID;
using ModLibsCore.Classes.Errors;
using ModLibsCore.Services.Network.SimplePacket;


namespace TheMadRanger.NetProtocols {
	class GunAimStateSyncPacket : SimplePacketPayload {
		public static void BroadcastFromLocalPlayer() {
			if( Main.netMode != NetmodeID.MultiplayerClient ) {
				throw new ModLibsException( "Not a client." );
			}

			var myplayer = Main.LocalPlayer.GetModPlayer<TMRPlayer>();

			var packet = new GunAimStateSyncPacket(
				Main.myPlayer,
				myplayer.AimMode.AimElapsed,
				myplayer.AimMode.WasApplyingModeLock_Client
			);

			SimplePacket.SendToServer( packet );
		}



		////////////////

		public int PlayerWho;
		public float AimElapsed;
		public bool WasApplyingModeLock;



		////////////////

		private GunAimStateSyncPacket() { }

		private GunAimStateSyncPacket( int playerWho, float aimElapsed, bool wasApplyingModeLock ) {
			this.PlayerWho = playerWho;
			this.AimElapsed = aimElapsed;
			this.WasApplyingModeLock = wasApplyingModeLock;
		}


		////////////////

		public override void ReceiveOnServer( int fromWho ) {
			this.Receive();

			SimplePacket.SendToClient( this, -1, this.PlayerWho );
		}

		public override void ReceiveOnClient() {
			this.Receive();
		}

		////

		private void Receive() {
			var myplayer = Main.player[this.PlayerWho].GetModPlayer<TMRPlayer>();

			myplayer.AimMode.SyncAimState( this.AimElapsed, this.WasApplyingModeLock );
		}
	}
}
