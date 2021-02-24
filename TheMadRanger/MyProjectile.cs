using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheMadRanger.NPCs;


namespace TheMadRanger {
	partial class TMRProjectile : GlobalProjectile {
		public bool? IsFiredFromRevolver { get; private set; } = null;
		public bool IsQuickFiredFromRevolver { get; private set; } = false;


		////////////////

		public override bool InstancePerEntity => true;

		////////////////

		private bool IsBanditShot = false;



		////////////////

		public override void SetDefaults( Projectile projectile ) {
			if( BanditNPC.IsFiring && projectile.type == ProjectileID.SniperBullet ) {
				this.IsBanditShot = true;
			}
		}


		////////////////

		public override bool PreAI( Projectile projectile ) {
			if( projectile.type == ProjectileID.Bullet && !projectile.npcProj ) {
				if( !this.IsFiredFromRevolver.HasValue ) {
					this.InitializeTMRBulletIf( projectile );
				}
			}

			return base.PreAI( projectile );
		}


		////////////////

		public override void ModifyHitNPC( Projectile projectile, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection ) {
			if( this.IsBanditShot ) {
				var config = TMRConfig.Instance;
				damage = config.Get<int>( nameof(config.BanditShotDamage) );
			}
		}

		public override void ModifyHitPlayer( Projectile projectile, Player target, ref int damage, ref bool crit ) {
			if( this.IsBanditShot ) {
				var config = TMRConfig.Instance;
				damage = config.Get<int>( nameof(config.BanditShotDamage) );
			}
		}
	}
}
