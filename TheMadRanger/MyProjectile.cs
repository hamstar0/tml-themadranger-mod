using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModLibsCore.Services.ProjectileOwner;
using TheMadRanger.NPCs;


namespace TheMadRanger {
	partial class TMRProjectile : GlobalProjectile {
		public bool? IsFiredFromRevolver { get; private set; } = null;
		public bool IsQuickFiredFromRevolver { get; private set; } = false;


		////////////////

		public override bool InstancePerEntity => true;

		////////////////

		public bool IsBanditShot { get; internal set; } = false;

		public bool IsBanditShotSynced { get; private set; } = false;



		////////////////

		public override void SetDefaults( Projectile projectile ) {
			if( BanditNPC.IsFiring && projectile.type == ProjectileID.SniperBullet ) {
				this.IsBanditShot = true;
			}
		}


		////////////////

		public override bool PreAI( Projectile projectile ) {
			if( this.IsBanditShot ) {
				this.AI_BanditShot( projectile );
			}

			//

			if( projectile.type == ProjectileID.Bullet ) {
				if( ProjectileOwner.GetOwner(projectile) is Player ) {
					this.AI_PlayerBullet( projectile );
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
