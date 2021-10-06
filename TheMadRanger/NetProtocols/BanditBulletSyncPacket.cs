using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModLibsCore.Classes.Errors;
using ModLibsCore.Libraries.Debug;
using ModLibsCore.Services.Network.SimplePacket;
using TheMadRanger.NPCs;


namespace TheMadRanger.NetProtocols {
	class BanditBulletSyncPacket : SimplePacketPayload {
		public static void SendToClient( int projWho, int toWho=-1, int ignoreWho=-1 ) {
			if( Main.netMode != NetmodeID.Server ) { throw new ModLibsException( "Not server." ); }
			
			var packet = new BanditBulletSyncPacket( projWho );

			SimplePacket.SendToClient( packet );
		}



		////////////////

		public int ProjWho;



		////////////////

		private BanditBulletSyncPacket() { }

		private BanditBulletSyncPacket( int projWho ) {
			this.ProjWho = projWho;
		}


		////////////////

		public override void ReceiveOnServer( int fromWho ) {
			throw new NotImplementedException();
		}

		public override void ReceiveOnClient() {
			Projectile proj = Main.projectile[ this.ProjWho ];
			if( proj?.active != true || proj.type != ProjectileID.SniperBullet ) {
				LogLibraries.Alert( "Mismatched sniper bullet ("+proj.ToString()+")" );
			}

			proj.GetGlobalProjectile<TMRProjectile>().IsBanditShot = true;
		}
	}
}
