using System;
using Terraria;
using Terraria.ModLoader;


namespace TheMadRanger {
	class TMRNPC : GlobalNPC {
		public override void ModifyHitByProjectile(
					NPC npc,
					Projectile projectile,
					ref int damage,
					ref float knockback,
					ref bool crit,
					ref int hitDirection ) {
			var myproj = projectile.GetGlobalProjectile<TMRProjectile>();
			if( myproj.IsFiredFromRevolver == true ) {
				this.ModifyTMRHit( npc, ref damage );
			}
		}


		////

		private void ModifyTMRHit( NPC npc, ref int damage ) {
			var config = TMRConfig.Instance;
			int dmgPer32 = config.Get<int>( nameof(config.DamagePerTargetVolumeUnitsOf32Sqr) );
			float dmgScaleForBoss = config.Get<float>( nameof(config.DamageScaleAgainstBosses ) );

			int npcFatness = (npc.width * npc.height) / 1024;	// 1024 = 32 * 32

			damage += npcFatness * dmgPer32;
			if( damage < 0 ) {
				damage = 0;
			}

			if( npc.boss ) {
				damage = (int)((float)damage * dmgScaleForBoss);
			}
		}
	}
}
