using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using ModLibsCore.Libraries.Debug;
using ModLibsGeneral.Libraries.Players;
using ModLibsGeneral.Libraries.Items.Attributes;
using TheMadRanger.Recipes;
using TheMadRanger.Items.Accessories;


namespace TheMadRanger.Items.Weapons {
	public partial class TheMadRangerItem : ModItem {
		public static int Width => 28;
		public static int Height => 14;
		public static float Scale => 0.65f;
		public static int CylinderCapacity => 6;



		////////////////

		public static bool IsAmmoSourceAvailable( Player player, bool skipSpeedloaders, out string result ) {
			var config = TMRConfig.Instance;

			if( config.Get<bool>( nameof(TMRConfig.InfiniteAmmoCheat) ) ) {
				result = "Cheater.";
				return true;
			}

			if( !skipSpeedloaders ) {
				int speedloaderType = ModContent.ItemType<SpeedloaderItem>();

				for( int i = 0; i < player.inventory.Length; i++ ) {
					Item item = player.inventory[i];
					if( item == null || item.IsAir || item.type != speedloaderType ) {
						continue;
					}

					var myitem = item.modItem as SpeedloaderItem;
					if( ( myitem?.LoadedRounds ?? 0 ) > 0 ) {
						result = " Speedloader available.";
						return true;
					}
				}
				result = "Loaded speedloader needed.";
			} else {
				result = "Cannot reload.";
			}

			if( config.Get<bool>( nameof(TMRConfig.BandolierNeededToReload) ) ) {
				int bandolierType = ModContent.ItemType<BandolierItem>();
				int max = PlayerItemLibraries.VanillaAccessorySlotFirst
					+ PlayerItemLibraries.GetCurrentVanillaMaxAccessories( player );

				for( int i = PlayerItemLibraries.VanillaAccessorySlotFirst; i < max; i++ ) {
					Item item = player.armor[i];
					if( item == null || item.IsAir || item.type != bandolierType ) {
						continue;
					}

					result = "Bandolier available.";
					return true;
				}
				result = "Bandolier needed.";
			}

			return false;
		}



		////////////////

		private int[] Cylinder;

		private int ElapsedTimeSinceLastShotAttempt = 0;


		////////////////

		public int CurrentCylinderSlot { get; private set; } = 0;

		////

		public override bool CloneNewInstances => false;



		////////////////

		public TheMadRangerItem() {
			this.Cylinder = new int[TheMadRangerItem.CylinderCapacity];
			for( int i=0; i<TheMadRangerItem.CylinderCapacity; i++ ) {
				this.Cylinder[i] = 1;
			}
		}
		public override ModItem Clone( Item item ) {
			var myitem = base.Clone( item ) as TheMadRangerItem;
			myitem.Cylinder = this.Cylinder.ToArray();

			return myitem;
		}

		////

		public override void SetStaticDefaults() {
			this.DisplayName.SetDefault( "The Mad Ranger" );
			this.Tooltip.SetDefault( "A mysterious, antique .357 revolver from a far away land."
				+ "\nUnusually powerful; needs a steady hand"
				+ "\nOnly uses its own ammo type"
				+ "\nHold right-click to keep aim steady"
				+ "\nUses loaded speedloaders for faster reloads"
			);
		}

		public override void SetDefaults() {
			this.item.width = TheMadRangerItem.Width;
			this.item.height = TheMadRangerItem.Height;
			this.item.scale = TheMadRangerItem.Scale;

			this.item.ranged = true;
			this.item.useStyle = 5;
			this.item.useTime = 7;
			this.item.useAnimation = 7;
			this.item.autoReuse = false;
			this.item.noMelee = true;
			this.item.shoot = ProjectileID.Bullet;
			this.item.shootSpeed = 32f;

			//this.item.UseSound = this.mod.GetLegacySoundSlot( SoundType.Custom, "Sounds/Custom/RevolverFire" )
			//	.WithVolume( 0.25f );
			////.WithPitchVariance( 0.5f );

			this.item.damage = TMRConfig.Instance.Get<int>( nameof(TMRConfig.MaximumAimedGunDamage) );
			this.item.knockBack = 4;

			this.item.rare = 4;
			this.item.value = Item.buyPrice( 0, 5, 0, 0 );
		}


		////////////////

		public override void AddRecipes() {
			var recipe = new TheMadRangerRecipe( this );
			recipe.AddRecipe();
		}


		////////////////

		public override void Load( TagCompound tag ) {
			if( !tag.ContainsKey("cylinder_idx") ) {
				return;
			}

			this.CurrentCylinderSlot = tag.GetInt( "cylinder_idx" );

			for( int i = 0; i < this.Cylinder.Length; i++ ) {
				if( !tag.ContainsKey("cylinder_round_" + i) ) {
					break;
				}
				this.Cylinder[i] = tag.GetInt( "cylinder_round_" + i );
			}
		}

		public override TagCompound Save() {
			var tag = new TagCompound {
				{ "cylinder_idx", this.CurrentCylinderSlot }
			};
			for( int i = 0; i < this.Cylinder.Length; i++ ) {
				tag["cylinder_round_" + i ] = this.Cylinder[i];
			}

			return tag;
		}


		////////////////

		public override void ModifyTooltips( List<TooltipLine> tooltips ) {
			var config = TMRConfig.Instance;
			int dmgPer32 = config.Get<int>( nameof(config.DamagePerTargetVolumeUnitsOf32Sqr) );
			float dmgMulForBoss = config.Get<float>( nameof(config.DamageScaleAgainstBosses) );

			if( dmgPer32 < 0 ) {
				string dmgText = "Damage reduced against larger targets ("+dmgPer32+" per 32 in. sqr. area)";
				var tip = new TooltipLine( this.mod, "TMRDamagePer32", dmgText );

				ItemInformationAttributeLibraries.ApplyTooltipAt( tooltips, tip, VanillaTooltipName.Damage, true );
			}

			if( dmgMulForBoss != 1f ) {
				string dmgText = "Damage scaled to "+(int)(dmgMulForBoss * 100f)+"% against bosses";
				var tip = new TooltipLine( this.mod, "TMRDamagePerBoss", dmgText );

				ItemInformationAttributeLibraries.ApplyTooltipAt( tooltips, tip, VanillaTooltipName.Damage, true );
			}
		}


		////////////////

		public override void UpdateInventory( Player player ) {
			if( !Main.gamePaused && !player.dead ) {
				this.ElapsedTimeSinceLastShotAttempt++;
			}

			if( TMRConfig.Instance.DebugModeGunInfo && player.whoAmI == Main.myPlayer ) {
				DebugLibraries.Print( "cylinder", this.CurrentCylinderSlot + " = " + string.Join(", ", this.Cylinder) );
			}
		}
	}
}
