using System;
using Terraria;
using Terraria.ModLoader;
using ModLibsCore.Libraries.Debug;


namespace TheMadRanger.NPCs {
	partial class BanditNPC : ModNPC {
		private void AI_CheckRetreatIf() {
			if( !this.npc.HasPlayerTarget || this.npc.target < 0 ) {
				return;
			}

			Player player = Main.player[ this.npc.target ];
			if( player?.active != true || player.dead ) {
				return;
			}

			if( !this.HasAttemptedRetreat ) {
				this.AI_CheckRetreatWithTargetIf( player );
			}
		}

		private void AI_CheckRetreatWithTargetIf( Player targetPlayer ) {
			var config = TMRConfig.Instance;
			int retreatDist = config.Get<int>( nameof(config.BanditRetreatTileDistance) );
			int retreatDistSqr = retreatDist * retreatDist;

			if( (targetPlayer.Center - npc.Center).LengthSquared() < retreatDistSqr ) {
				this.BeginRetreat();
			}
		}
	}
}
