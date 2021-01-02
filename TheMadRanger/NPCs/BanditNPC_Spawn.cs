using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.World;


namespace TheMadRanger.NPCs {
	partial class BanditNPC : ModNPC {
		public override float SpawnChance( NPCSpawnInfo spawnInfo ) {
			if( spawnInfo.invasion || spawnInfo.playerSafe || spawnInfo.playerInTown ) {
				return 0f;
			}
			if( Main.eclipse || !Main.dayTime ) {
				return 0f;
			}
			if( spawnInfo.sky || spawnInfo.spawnTileY > WorldHelpers.SurfaceLayerBottomTileY ) {
				return 0f;
			}

			Player plr = spawnInfo.player;
			if( plr.ZoneBeach || plr.ZoneCorrupt || plr.ZoneCrimson || plr.ZoneHoly || plr.ZoneDungeon || plr.ZoneJungle ) {
				return 0f;
			}
			if( plr.ZoneSnow && plr.sandStorm ) {
				return 0f;
			}

			if( BanditNPC.IsComboSpawn ) {
				return 1f;
			}

			var config = TMRConfig.Instance;
			return config.Get<float>( nameof(config.BanditSpawnChance) );
		}

		
		public override int SpawnNPC( int tileX, int tileY ) {
			if( Main.netMode != NetmodeID.MultiplayerClient ) {
				var config = TMRConfig.Instance;

				if( !BanditNPC.IsComboSpawn ) {
					float comboChance = config.Get<float>( nameof(config.BanditComboSpawnChance) );

					BanditNPC.IsComboSpawn = Main.rand.NextFloat() < comboChance;
				} else {
					float comboChance = config.Get<float>( nameof(config.BanditComboChainSpawnChance) );

					BanditNPC.IsComboSpawn = Main.rand.NextFloat() < comboChance;
				}
			}

			return base.SpawnNPC( tileX, tileY );
		}
	}
}
