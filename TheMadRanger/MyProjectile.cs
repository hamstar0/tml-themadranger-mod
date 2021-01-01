using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheMadRanger.NPCs;


namespace TheMadRanger {
	class TMRProjectile : GlobalProjectile {
		public bool IsFiredFromRevolver { get; private set; } = false;
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
			if( projectile.type != ProjectileID.Bullet || projectile.npcProj || this.IsFiredFromRevolver ) {
				return true;
			}

			Player plr = Main.player[ projectile.owner ];
			if( plr?.active != true ) {
				return true;
			}

			this.IsFiredFromRevolver = true;

			var myplayer = plr.GetModPlayer<TMRPlayer>();
			if( myplayer.AimMode.IsQuickDrawActive ) {
				this.IsQuickFiredFromRevolver = true;
			}

			myplayer.AimMode.InitializeBulletProjectile( projectile );

			return base.PreAI( projectile );
		}


		////////////////

		public override void ModifyHitNPC( Projectile projectile, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection ) {
			if( this.IsBanditShot ) {
				damage = BanditNPC.ShotDamage;
			}
		}

		public override void ModifyHitPlayer( Projectile projectile, Player target, ref int damage, ref bool crit ) {
			if( this.IsBanditShot ) {
				damage = BanditNPC.ShotDamage;
			}
		}

		public override void ModifyHitPvp( Projectile projectile, Player target, ref int damage, ref bool crit ) {
			if( this.IsBanditShot ) {
				damage = BanditNPC.ShotDamage;
			}
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

		////
		
		public override bool OnTileCollide( Projectile projectile, Vector2 oldVelocity ) {
			if( !this.IsFiredFromRevolver ) { return true; }
			if( projectile.owner < 0 ) { return true; }

			Player plr = Main.player[ projectile.owner ];
			if( !plr.active ) { return false; }

			var myplayer = plr.GetModPlayer<TMRPlayer>();
			myplayer.AimMode.ApplyUnsuccessfulHit( plr );

			this.IsFiredFromRevolver = false;
			this.IsQuickFiredFromRevolver = false;

			return true;
		}

		////

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
