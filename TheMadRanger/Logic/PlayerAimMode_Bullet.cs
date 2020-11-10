using System;
using Terraria;
using HamstarHelpers.Helpers.Debug;


namespace TheMadRanger.Logic {
	partial class PlayerAimMode {
		public void InitializeBulletProjectile( Projectile projectile ) {
			projectile.penetrate = 2;
		}
	}
}
