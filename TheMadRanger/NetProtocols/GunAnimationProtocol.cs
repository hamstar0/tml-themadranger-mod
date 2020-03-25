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
		public static void Broadcast( GunAnimationType animType ) {
			if( Main.netMode != 1 ) { throw new ModHelpersException( "Not a client." ); }

			var protocol = new GunAnimationProtocol( Main.myPlayer, animType );

			protocol.SendToServer( true );
		}



		////////////////

		public int PlayerWho;
		public int AnimType;



		////////////////

		private GunAnimationProtocol() { }

		private GunAnimationProtocol( int playerWho, GunAnimationType animType ) {
			this.PlayerWho = playerWho;
			this.AnimType = (int)animType;
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
			case GunAnimationType.Holster:  // Might interrupt other item actions such that server should know
				otherplr.GunHandling.BeginHolster( plr );
				break;
				//case GunAnimationType.Reload:
				//	otherplr.GunHandling.BeginReload( plr );
				//	break;
			}
		}
	}
}
