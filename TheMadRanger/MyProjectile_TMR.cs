using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;


namespace TheMadRanger {
	partial class TMRProjectile : GlobalProjectile {
		private void InitializeTMRBulletIf( Projectile projectile ) {
			Player plr = Main.player[projectile.owner];
			if( plr?.active != true ) {
				return;
			}

			this.IsFiredFromRevolver = true;

			var myplayer = plr.GetModPlayer<TMRPlayer>();
			if( myplayer.AimMode.IsQuickDrawActive ) {
				this.IsQuickFiredFromRevolver = true;
			}

			myplayer.AimMode.InitializeBulletProjectile( projectile );
		}


		////////////////

		public override bool OnTileCollide( Projectile projectile, Vector2 oldVelocity ) {
			if( !this.IsFiredFromRevolver ) { return true; }
			if( projectile.owner < 0 ) { return true; }

			Player plr = Main.player[projectile.owner];
			if( !plr.active ) { return false; }

			var myplayer = plr.GetModPlayer<TMRPlayer>();
			myplayer.AimMode.ApplyUnsuccessfulHit( plr );

			this.IsFiredFromRevolver = false;
			this.IsQuickFiredFromRevolver = false;

			return true;
		}


		////

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


		////////////////

		private void OnHit( Projectile projectile ) {
			if( !this.IsFiredFromRevolver ) { return; }
			if( projectile.owner < 0 ) { return; }

			Player plr = Main.player[projectile.owner];
			if( !plr.active ) { return; }

			var myplayer = plr.GetModPlayer<TMRPlayer>();
			myplayer.AimMode.ApplySuccessfulHit( plr );
		}
	}
}
