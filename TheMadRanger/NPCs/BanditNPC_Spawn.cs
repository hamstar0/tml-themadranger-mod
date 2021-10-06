using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using ModLibsCore.Libraries.Debug;
using ModLibsGeneral.Libraries.World;


namespace TheMadRanger.NPCs {
	partial class BanditNPC : ModNPC {
		public override float SpawnChance( NPCSpawnInfo spawnInfo ) {
			if( spawnInfo.invasion || spawnInfo.playerSafe || spawnInfo.playerInTown ) {
				if( TMRConfig.Instance.DebugModeBanditInfo ) {
					LogLibraries.AlertOnce( "BANDIT SPAWN FAIL 1 - "
						+spawnInfo.invasion
						+", "+spawnInfo.playerSafe
						+", "+spawnInfo.playerInTown );
				}
				return 0f;
			}
			if( Main.eclipse || !Main.dayTime ) {
				if( TMRConfig.Instance.DebugModeBanditInfo ) {
					LogLibraries.AlertOnce( "BANDIT SPAWN FAIL 2 - "+Main.eclipse+", "+!Main.dayTime );
				}
				return 0f;
			}
			if( spawnInfo.sky || spawnInfo.spawnTileY > WorldLocationLibraries.SurfaceLayerBottomTileY ) {
				if( TMRConfig.Instance.DebugModeBanditInfo ) {
					LogLibraries.AlertOnce( "BANDIT SPAWN FAIL 3 - "
						+spawnInfo.sky+", "
						+(spawnInfo.spawnTileY > WorldLocationLibraries.SurfaceLayerBottomTileY) );
				}
				return 0f;
			}

			Player plr = spawnInfo.player;
			if( plr.ZoneBeach || plr.ZoneCorrupt || plr.ZoneCrimson || plr.ZoneHoly || plr.ZoneDungeon || plr.ZoneJungle ) {
				if( TMRConfig.Instance.DebugModeBanditInfo ) {
					LogLibraries.AlertOnce( "BANDIT SPAWN FAIL 4 - "
						+plr.ZoneBeach+", "
						+plr.ZoneCorrupt+", "
						+plr.ZoneCrimson+", "
						+plr.ZoneHoly+", "
						+plr.ZoneDungeon+", "
						+plr.ZoneJungle );
				}
				return 0f;
			}
			if( plr.ZoneSnow && plr.sandStorm ) {
				if( TMRConfig.Instance.DebugModeBanditInfo ) {
					LogLibraries.AlertOnce( "BANDIT SPAWN FAIL 5 - "+plr.ZoneSnow+", "+plr.sandStorm );
				}
				return 0f;
			}

			if( BanditNPC.IsComboSpawn ) {
				return 1f;
			} else {
				var config = TMRConfig.Instance;
				return config.Get<float>( nameof(config.BanditSpawnChance) );
			}
		}

		
		public override int SpawnNPC( int tileX, int tileY ) {
			if( Main.netMode != NetmodeID.MultiplayerClient ) {
				var config = TMRConfig.Instance;

				if( !BanditNPC.IsComboSpawn ) {
					float comboChance = config.Get<float>( nameof(config.BanditComboSpawnChance) );

					BanditNPC.IsComboSpawn = Main.rand.NextFloat() < comboChance;
				} else {
					float comboChainChance = config.Get<float>( nameof(config.BanditComboChainSpawnChance) );

					BanditNPC.IsComboSpawn = Main.rand.NextFloat() < comboChainChance;
				}
			}
			
			if( TMRConfig.Instance.DebugModeBanditInfo ) {
				NetMessage.BroadcastChatMessage(
					NetworkText.FromLiteral( "SPAWNING BANDIT AT "+tileX+", "+tileY
						+" ("+(Main.netMode==NetmodeID.Server?"server":"client")+")" ),
					Color.Teal
				);
			}

			return base.SpawnNPC( tileX, tileY );
		}
	}
}
