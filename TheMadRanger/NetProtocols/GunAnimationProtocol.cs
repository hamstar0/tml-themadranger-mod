using System;
using Terraria;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Helpers.TModLoader;
using HamstarHelpers.Services.Network.NetIO;
using HamstarHelpers.Services.Network.NetIO.PayloadTypes;


namespace TheMadRanger.NetProtocols {
	public enum GunAnimationType {
		Recoil,
		Holster,
		Reload
	}




	[Serializable]
	class GunAnimationProtocol : NetIOBroadcastPayload {
		public static void Broadcast( GunAnimationType animType ) {
			if( Main.netMode != 1 ) { throw new ModHelpersException( "Not a client." ); }
			
			var protocol = new GunAnimationProtocol( Main.myPlayer, animType );

			NetIO.Broadcast( protocol );
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

		public override bool ReceiveOnServerBeforeRebroadcast( int fromWho ) {
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
			return true;
		}

		public override void ReceiveBroadcastOnClient() {
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
	}
}
