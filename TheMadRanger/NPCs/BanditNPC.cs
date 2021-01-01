using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace TheMadRanger.NPCs {
	partial class BanditNPC : ModNPC {
		public static bool IsFiring { get; private set; } = false;

		////

		public static int ContactDamage { get; } = 10;
		public static int ShotDamage { get; } = 20;



		////////////////

		public override void SetStaticDefaults() {
			this.DisplayName.SetDefault( "Bandit" );
			Main.npcFrameCount[ this.npc.type ] = Main.npcFrameCount[ NPCID.SkeletonSniper ];
		}

		public override void SetDefaults() {
			NPC npc = this.npc;

			// Adjust coin drop amount on a curve
			float valueScale = Main.rand.NextFloat();
			valueScale = valueScale * valueScale * valueScale * valueScale * valueScale * valueScale;
			int minVal = Item.buyPrice( 0, 0, 10, 0 );
			int maxVal = Item.buyPrice( 0, 10, 0, 0 );
			int value = minVal + (int)((float)(maxVal - minVal) * valueScale);

			npc.width = 18;
			npc.height = 40;
			npc.aiStyle = 3;
			npc.damage = BanditNPC.ContactDamage;
			npc.defense = 1;
			npc.lifeMax = 50;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.knockBackResist = 0.3f;
			npc.value = value;
			npc.buffImmune[ BuffID.Poisoned ] = true;
			npc.buffImmune[ BuffID.Confused ] = false;

			this.aiType = NPCID.SkeletonSniper;
			this.animationType = NPCID.SkeletonSniper;

			//this.banner = Item.NPCtoBanner( NPCID.Zombie );
			//this.bannerItem = Item.BannerToItem( this.banner );
		}


		////////////////

		public override bool PreAI() {
			BanditNPC.IsFiring = true;
			return true;
		}

		public override void PostAI() {
			BanditNPC.IsFiring = false;
		}


		////////////////

		public override void HitEffect( int hitDirection, double damage ) {
			NPC npc = this.npc;
			
			if( npc.life <= 0 ) {
				Gore.NewGore( npc.position, npc.velocity, GoreID.GoblinScoutHead, 1f );
				Gore.NewGore( new Vector2( npc.position.X, npc.position.Y + 20f ), npc.velocity, GoreID.GoblinScoutHand, 1f );
				Gore.NewGore( new Vector2( npc.position.X, npc.position.Y + 20f ), npc.velocity, GoreID.GoblinScoutHand, 1f );
				Gore.NewGore( new Vector2( npc.position.X, npc.position.Y + 34f ), npc.velocity, GoreID.GoblinScoutLeg, 1f );
				Gore.NewGore( new Vector2( npc.position.X, npc.position.Y + 34f ), npc.velocity, GoreID.GoblinScoutLeg, 1f );
			}
		}
	}
}
