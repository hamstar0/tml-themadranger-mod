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
		public static void BroadcastFromLocalPlayer( GunAnimationType animType, float data = 0f ) {
			if( Main.netMode != NetmodeID.MultiplayerClient ) {
				throw new ModLibsException( "Not a client." );
			}
			
			var packet = new GunAnimationPacket( Main.myPlayer, animType, data );

			SimplePacket.SendToServer( packet );
		}



		////////////////

		public int PlayerWho;
		public int AnimType;
		public float Data;



		////////////////

		private GunAnimationPacket() { }

		private GunAnimationPacket( int playerWho, GunAnimationType animType, float data ) {
			this.PlayerWho = playerWho;
			this.AnimType = (int)animType;
			this.Data = (float)data;
		}


		////////////////

		public override void ReceiveOnServer( int fromWho ) {
			Player plr = Main.player[this.PlayerWho];
			var otherplr = TmlLibraries.SafelyGetModPlayer<TMRPlayer>( plr );
			GunAnimationType animType = (GunAnimationType)this.AnimType;

			//

			switch( animType ) {
			//case GunAnimationType.Recoil:
			//	otherplr.GunHandling.BeginRecoil( 0f );
			//	break;
			case GunAnimationType.Holster:  // Might interrupt other item actions such that server should know
				otherplr.GunHandling.BeginHolster( plr );
				break;
			//case GunAnimationType.Reload:
			//	Item gun = plr.HeldItem;
			//	if( gun?.active != true ) {
			//		break;
			//	}
			//
			//	var mygun = gun.modItem as TheMadRangerItem;
			//	if( mygun == null ) {
			//		break;
			//	}
			//
			//	otherplr.GunHandling.BeginReload( plr );
			//	break;
			}

			//

			SimplePacket.SendToClient( this, -1, fromWho );
		}

		public override void ReceiveOnClient() {
			Player plr = Main.player[this.PlayerWho];
			var otherplr = TmlLibraries.SafelyGetModPlayer<TMRPlayer>( plr );
			GunAnimationType animType = (GunAnimationType)this.AnimType;

			//

			switch( animType ) {
			case GunAnimationType.Recoil:
				otherplr.GunHandling.BeginRecoil( this.Data );
				break;
			case GunAnimationType.Holster:
				otherplr.GunHandling.BeginHolster( plr );
				break;
			case GunAnimationType.Reload:
				Item gun = plr.HeldItem;
				if( gun?.active != true ) {
					break;
				}

				var mygun = gun.modItem as TheMadRangerItem;
				if( mygun == null ) {
					break;
				}

				//

				otherplr.GunHandling.BeginReload_If( plr, mygun, true );
				break;
			}
		}
	}
}
