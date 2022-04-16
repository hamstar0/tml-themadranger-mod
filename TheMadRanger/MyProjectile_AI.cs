using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheMadRanger.NetProtocols;


namespace TheMadRanger {
	partial class TMRProjectile : GlobalProjectile {
		private void AI_BanditShot( Projectile projectile ) {
			if( Main.netMode != NetmodeID.Server ) {
				return;
			}

			if( this.IsBanditShotSynced ) {
				return;
			}

			int idx = Array.IndexOf( Main.projectile, projectile );
			if( idx == -1 ) {
				return;
			}

			//

			this.IsBanditShotSynced = true;

			BanditBulletSyncPacket.SendToClient( idx );
		}

		private void AI_PlayerBullet( Projectile projectile ) {
			if( this.IsFiredFromRevolver.HasValue ) {
				return;
			}

			//

			this.InitializeTMRBullet_If( projectile );
		}
	}
}
