﻿using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using HamstarHelpers.Helpers.Debug;


namespace TheMadRanger.NPCs {
	partial class BanditNPC : ModNPC {
		public override void OnHitByItem( Player player, Item item, int damage, float knockback, bool crit ) {
			if( !this.IsRetreatingNow && !this.IsBraveNow ) {
				this.IsRetreatingNow = true;
			}
		}

		public override void OnHitByProjectile( Projectile projectile, int damage, float knockback, bool crit ) {
			if( !this.IsRetreatingNow && !this.IsBraveNow ) {
				this.IsRetreatingNow = true;
			}
		}


		////////////////

		public override void HitEffect( int hitDirection, double damage ) {
			NPC npc = this.npc;
			
			if( npc.life <= 0 ) {
				Mod mod = this.mod;
				Vector2 pos = npc.position;
				Vector2 vel = npc.velocity;
				float scale = npc.scale;

				Gore.NewGore( npc.position, vel, mod.GetGoreSlot("Gores/BanditHead"), scale );
				Gore.NewGore( new Vector2(pos.X, pos.Y + 20f), vel, mod.GetGoreSlot("Gores/BanditArm"), scale );
				Gore.NewGore( new Vector2(pos.X, pos.Y + 20f), vel, mod.GetGoreSlot("Gores/BanditArm"), scale );
				Gore.NewGore( new Vector2(pos.X, pos.Y + 34f), vel, mod.GetGoreSlot("Gores/BanditLeg"), scale );
				Gore.NewGore( new Vector2(pos.X, pos.Y + 34f), vel, mod.GetGoreSlot("Gores/BanditLeg"), scale );
			}
		}
	}
}
