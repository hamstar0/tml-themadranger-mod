using System;
using Terraria;
using Terraria.ID;
using ModLibsCore.Classes.Errors;
using ModLibsCore.Libraries.TModLoader;
using ModLibsCore.Services.Network.SimplePacket;
using TheMadRanger.Items.Weapons;


namespace TheMadRanger.NetProtocols {
	public enum GunAnimationType {
		Recoil,
		Holster,
		Reload
	}




	class GunAnimationPacket : SimplePacketPayload {
		public static void Broadcast( GunAnimationType animType ) {
			if( Main.netMode != NetmodeID.MultiplayerClient ) {
				throw new ModLibsException( "Not a client." );
			}
			
			var packet = new GunAnimationPacket( Main.myPlayer, animType );

			SimplePacket.SendToServer( packet );
		}



		////////////////

		public int PlayerWho;
		public int AnimType;



		////////////////

		private GunAnimationPacket() { }

		private GunAnimationPacket( int playerWho, GunAnimationType animType ) {
			this.PlayerWho = playerWho;
			this.AnimType = (int)animType;
		}


		////////////////

		public override void ReceiveOnServer( int fromWho ) {
			Player plr = Main.player[this.PlayerWho];
			var otherplr = TmlLibraries.SafelyGetModPlayer<TMRPlayer>( plr );
			GunAnimationType animType = (GunAnimationType)this.AnimType;

			Item gun = plr.HeldItem;
			if( gun?.active != true ) {
				return;
			}

			var mygun = gun.modItem as TheMadRangerItem;
			if( mygun == null ) {
				return;
			}

			switch( animType ) {
			//case GunAnimationType.Recoil:
			//	otherplr.GunHandling.BeginRecoil( 0f );
			//	break;
			case GunAnimationType.Holster:  // Might interrupt other item actions such that server should know
				otherplr.GunHandling.BeginHolster( plr, mygun );
				break;
				//case GunAnimationType.Reload:
				//	otherplr.GunHandling.BeginReload( plr );
				//	break;
			}

			SimplePacket.SendToClient( this, -1, fromWho );
		}

		public override void ReceiveOnClient() {
			Player plr = Main.player[this.PlayerWho];
			var otherplr = TmlLibraries.SafelyGetModPlayer<TMRPlayer>( plr );
			GunAnimationType animType = (GunAnimationType)this.AnimType;

			Item gun = plr.HeldItem;
			if( gun?.active != true ) {
				return;
			}

			var mygun = gun.modItem as TheMadRangerItem;
			if( mygun == null ) {
				return;
			}

			switch( animType ) {
			case GunAnimationType.Recoil:
				otherplr.GunHandling.BeginRecoil( 0f );
				break;
			case GunAnimationType.Holster:
				otherplr.GunHandling.BeginHolster( plr, mygun );
				break;
			case GunAnimationType.Reload:
				otherplr.GunHandling.BeginReload_If( plr, mygun );
				break;
			}
		}
	}
}
