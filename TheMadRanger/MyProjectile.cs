using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace TheMadRanger {
	class MyProjectile : GlobalProjectile {
		public bool IsFiredFromRevolver { get; internal set; } = false;
		public bool IsQuickFiredFromRevolver { get; internal set; } = false;


		////////////////

		public override bool InstancePerEntity => true;



		////////////////

		public override bool PreAI( Projectile projectile ) {
			if( projectile.type != ProjectileID.Bullet || projectile.npcProj || this.IsFiredFromRevolver ) {
				return true;
			}

			Player plr = Main.player[ projectile.owner ];
			if( plr?.active != true ) {
				return true;
			}

			this.IsFiredFromRevolver = true;

			var myplayer = plr.GetModPlayer<TMRPlayer>();
			if( myplayer.AimMode.IsQuickDraw ) {
				this.IsQuickFiredFromRevolver = true;
			}

			myplayer.AimMode.InitializeBulletProjectile( projectile );

			return base.PreAI( projectile );
		}


		////////////////

		public override void OnHitNPC( Projectile projectile, NPC target, int damage, float knockback, bool crit ) {
			if( this.IsFiredFromRevolver ) {
				this.OnHit( projectile );
			}
		}

		public override void OnHitPvp( Projectile projectile, Player target, int damage, bool crit ) {
			if( this.IsFiredFromRevolver ) {
				this.OnHit( projectile );
			}
		}

		////

		public override bool OnTileCollide( Projectile projectile, Vector2 oldVelocity ) {
			if( projectile.owner < 0 ) { return true; }
			Player plr = Main.player[projectile.owner];
			if( !plr.active ) { return false; }

			var myplayer = plr.GetModPlayer<TMRPlayer>();
			myplayer.AimMode.ApplyUnsuccessfulHit( plr );
			return true;
		}

		////

		private void OnHit( Projectile projectile ) {
			if( projectile.owner < 0 ) { return; }
			Player plr = Main.player[projectile.owner];
			if( !plr.active ) { return; }

			var myplayer = plr.GetModPlayer<TMRPlayer>();
			myplayer.AimMode.ApplySuccessfulHit( plr );
		}
	}
}
