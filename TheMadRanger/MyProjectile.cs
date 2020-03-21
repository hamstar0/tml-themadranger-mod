using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace TheMadRanger {
	class MyProjectile : GlobalProjectile {
		public bool QuickFiredFromRevolver { get; internal set; } = false;


		////////////////

		public override bool InstancePerEntity => true;



		////////////////

		public override bool PreAI( Projectile projectile ) {
			if( projectile.type != ProjectileID.Bullet || projectile.npcProj && this.QuickFiredFromRevolver ) {
				return true;
			}

			Player plr = Main.player[projectile.owner];
			if( plr?.active != true ) {
				return true;
			}

			var myplayer = plr.GetModPlayer<TMRPlayer>();
			if( myplayer.AimMode.IsQuickDraw ) {
				this.QuickFiredFromRevolver = true;
			}

			return base.PreAI( projectile );
		}


		////////////////

		public override void OnHitNPC( Projectile projectile, NPC target, int damage, float knockback, bool crit ) {
			if( this.QuickFiredFromRevolver ) {
				this.OnHit( projectile );
			}
		}

		public override void OnHitPvp( Projectile projectile, Player target, int damage, bool crit ) {
			if( this.QuickFiredFromRevolver ) {
				this.OnHit( projectile );
			}
		}

		////

		private void OnHit( Projectile projectile ) {
			if( projectile.owner < 0 ) { return; }
			Player plr = Main.player[projectile.owner];
			if( !plr.active ) { return; }

			var myplayer = plr.GetModPlayer<TMRPlayer>();
			myplayer.AimMode.ApplyQuickDraw( plr );
		}
	}
}
