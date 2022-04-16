using System;
using Terraria;
using ModLibsCore.Libraries.Debug;


namespace TheMadRanger.Logic {
	partial class PlayerAimMode {
		public void InitializeBulletProjectile( Projectile projectile ) {
			projectile.penetrate = 2;

			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = -1;
		}
	}
}
