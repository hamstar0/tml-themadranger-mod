using System;
using Terraria;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Classes.Protocols.Packet.Interfaces;
using HamstarHelpers.Helpers.TModLoader;

namespace TheMadRanger.NetProtocols {
	public enum GunAnimationType {
		Recoil,
		Holster,
		Reload
	}



	class GunAnimationProtocol : PacketProtocolBroadcast {
		public static void SendAnim( GunAnimationType animType ) {
			if( Main.netMode != 1 ) { throw new ModHelpersException("Not a client."); }

			var protocol = new GunAnimationProtocol( animType, Main.myPlayer );

			protocol.SendToServer( true );
		}



		////////////////

		public int AnimType;
		public int PlayerWho;



		////////////////

		private GunAnimationProtocol() { }

		private GunAnimationProtocol( GunAnimationType animType, int playerWho ) {
			this.AnimType = (int)animType;
			this.PlayerWho = playerWho;
		}

		////////////////

		protected override void ReceiveOnClient() {
			Player plr = Main.player[this.PlayerWho];
			var otherplr = TmlHelpers.SafelyGetModPlayer<TMRPlayer>( plr );
			GunAnimationType animType = (GunAnimationType)this.AnimType;

			switch( animType ) {
			case GunAnimationType.Recoil:
				otherplr.GunHandling.BeginRecoil( 0f );
				break;
			case GunAnimationType.Holster:
				otherplr.GunHandling.BeginHolster( plr );
				break;
			case GunAnimationType.Reload:
				otherplr.GunHandling.BeginReload( plr );
				break;
			}
		}

		protected override void ReceiveOnServer( int fromWho ) {
			Player plr = Main.player[this.PlayerWho];
			var otherplr = TmlHelpers.SafelyGetModPlayer<TMRPlayer>( plr );
			GunAnimationType animType = (GunAnimationType)this.AnimType;

			switch( animType ) {
			//case GunAnimationType.Recoil:
			//	otherplr.GunHandling.BeginRecoil( 0f );
			//	break;
			case GunAnimationType.Holster:	// Might interrupt other item actions such that server should know
				otherplr.GunHandling.BeginHolster( plr );
				break;
			//case GunAnimationType.Reload:
			//	otherplr.GunHandling.BeginReload( plr );
			//	break;
			}
		}
	}
}
