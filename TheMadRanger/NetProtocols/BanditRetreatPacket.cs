using System;
using Terraria;
using Terraria.ModLoader;
using ModLibsCore.Classes.Errors;
using ModLibsCore.Libraries.Debug;
using ModLibsCore.Services.Network.SimplePacket;
using TheMadRanger.NPCs;


namespace TheMadRanger.NetProtocols {
	class BanditRetreatPacket : SimplePacketPayload {
		public static void BroadcastToClients( int npcWho ) {
			if( Main.netMode != 2 ) { throw new ModLibsException( "Not server." ); }
			
			var packet = new BanditRetreatPacket( npcWho );

			SimplePacket.SendToClient( packet );
		}



		////////////////

		public int NpcWho;



		////////////////

		private BanditRetreatPacket() { }

		private BanditRetreatPacket( int npcWho ) {
			this.NpcWho = npcWho;
		}


		////////////////

		public override void ReceiveOnServer( int fromWho ) {
			throw new NotImplementedException();
		}

		public override void ReceiveOnClient() {
			NPC npc = Main.npc[this.NpcWho];
			if( npc?.active != true || npc.type != ModContent.NPCType<BanditNPC>() ) {
				LogLibraries.Alert( "Mismatched npc." );
			}

			var mynpc = npc.modNPC as BanditNPC;
			mynpc?.BeginRetreat();
		}
	}
}
