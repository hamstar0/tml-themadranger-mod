using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using HamstarHelpers.Helpers.Debug;


namespace TheMadRanger.NPCs {
	partial class BanditNPC : ModNPC {
		public static int RetreatTickDuration { get; } = 60 * 15;
		public static int ContactDamage { get; } = 10;
		public static int ShotDamage { get; } = 10;
		public static int RetreatTileDistance { get; } = 9;
		public static float MaxChaseSpeed { get; } = 3.5f;
		public static float MaxRetreatSpeed { get; } = 4f;

		////

		public static bool IsFiring { get; private set; } = false;



		////////////////

		public bool IsRetreatingNow {
			get => this.RetreatTimer > 0;
			private set {
				if( value ) {
					this.RetreatTimer = BanditNPC.RetreatTickDuration;
				} else {
					this.RetreatTimer = 0;
				}
			}
		}

		public bool IsBraveNow {
			get => this.BraveryTimer > 0;
			private set {
				if( value ) {
					this.BraveryTimer = 60 * 3;
				} else {
					this.BraveryTimer = 0;
				}
			}
		}


		////////////////

		private int RetreatTimer = 0;

		private int RetreatStuckElapsed = 0;

		private int BraveryTimer = 0;



		////////////////

		public override void SetStaticDefaults() {
			this.DisplayName.SetDefault( "Bandit" );
			Main.npcFrameCount[ this.npc.type ] = Main.npcFrameCount[ NPCID.SkeletonSniper ];
		}

		public override void SetDefaults() {
			NPC npc = this.npc;

			// Adjust coin drop amount on a curve
			float valMul = Main.rand.NextFloat();
			valMul = valMul * valMul * valMul * valMul * valMul * valMul;
			int minVal = Item.buyPrice( 0, 0, 10, 0 );
			int maxVal = Item.buyPrice( 0, 10, 0, 0 );
			int value = minVal + (int)((float)(maxVal - minVal) * valMul);

			npc.width = 18;
			npc.height = 40;

			npc.lifeMax = 50;
			npc.defense = 1;
			npc.damage = BanditNPC.ContactDamage;
			npc.knockBackResist = 0.3f;

			npc.value = value;

			npc.aiStyle = 3;
			this.aiType = NPCID.SkeletonSniper;
			this.animationType = NPCID.SkeletonSniper;

			//this.banner = Item.NPCtoBanner( NPCID.Zombie );
			//this.bannerItem = Item.BannerToItem( this.banner );

			npc.buffImmune[ BuffID.Poisoned ] = true;
			npc.buffImmune[ BuffID.Confused ] = false;

			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
		}


		////////////////

		public override bool PreAI() {
			BanditNPC.IsFiring = true;

			bool isRetreat = this.PreAI_ApplyRetreatEffectsIf();

			this.PreAI_ApplyRetreatEffectsIf();

			return isRetreat;
		}

		public override void AI() {
//DebugHelpers.Print( "ai", string.Join(", ", this.npc.ai) );
			this.AI_CheckRetreatIf();
		}

		public override void PostAI() {
			BanditNPC.IsFiring = false;

			this.PostAI_ApplyNormalMovementChangesIf();
			this.PostAI_ApplyRetreatMovementChangesIf();
		}


		////////////////

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
				Gore.NewGore( npc.position, npc.velocity, GoreID.GoblinScoutHead, 1f );
				Gore.NewGore( new Vector2( npc.position.X, npc.position.Y + 20f ), npc.velocity, GoreID.GoblinScoutHand, 1f );
				Gore.NewGore( new Vector2( npc.position.X, npc.position.Y + 20f ), npc.velocity, GoreID.GoblinScoutHand, 1f );
				Gore.NewGore( new Vector2( npc.position.X, npc.position.Y + 34f ), npc.velocity, GoreID.GoblinScoutLeg, 1f );
				Gore.NewGore( new Vector2( npc.position.X, npc.position.Y + 34f ), npc.velocity, GoreID.GoblinScoutLeg, 1f );
			}
		}
	}
}
