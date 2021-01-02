using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using HamstarHelpers.Helpers.Debug;
using TheMadRanger.Items.Weapons;
using TheMadRanger.Items;
using TheMadRanger.Items.Accessories;


namespace TheMadRanger.NPCs {
	partial class BanditNPC : ModNPC {
		private static bool IsComboSpawn = false;


		////////////////

		public static int RetreatTickDuration { get; } = 60 * 15;
		public static int ContactDamage { get; } = 10;
		public static int ShotDamage { get; } = 20;
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

			npc.width = 18;
			npc.height = 40;

			npc.lifeMax = 50;
			npc.defense = 1;
			npc.damage = BanditNPC.ContactDamage;
			npc.knockBackResist = 0.3f;

			npc.value = Item.buyPrice( 0, 0, 10, 0 );

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

		public override bool PreNPCLoot() {
			// Adjust coin drop amount on a curve
			float valMul = Main.rand.NextFloat();
			valMul = (float)Math.Pow( (double)valMul, 8 );

			int minVal = Item.buyPrice( 0, 0, 10, 0 );
			int maxVal = Item.buyPrice( 0, 3, 0, 0 );

			npc.value = minVal + (int)((float)(maxVal - minVal) * valMul);

			return base.PreNPCLoot();
		}

		public override void NPCLoot() {
			var config = TMRConfig.Instance;
			float tmrChance = config.Get<float>( nameof(config.BanditLootGunDropPercentChance) );
			float speedloaderChance = config.Get<float>( nameof(config.BanditLootSpeedloaderDropPercentChance) );
			float bandolierChance = config.Get<float>( nameof(config.BanditLootBandolierDropPercentChance) );
			
			if( Main.rand.NextFloat() < tmrChance ) {
				Item.NewItem( this.npc.position, ModContent.ItemType<TheMadRangerItem>(), 1 );
			}
			if( Main.rand.NextFloat() < speedloaderChance ) {
				Item.NewItem( this.npc.position, ModContent.ItemType<SpeedloaderItem>(), 1 );
			}
			if( Main.rand.NextFloat() < bandolierChance ) {
				Item.NewItem( this.npc.position, ModContent.ItemType<BandolierItem>(), 1 );
			}
		}
	}
}
