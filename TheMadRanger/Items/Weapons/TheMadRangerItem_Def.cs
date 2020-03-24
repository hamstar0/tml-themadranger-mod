using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.Players;
using TheMadRanger.Items.Accessories;


namespace TheMadRanger.Items.Weapons {
	public partial class TheMadRangerItem : ModItem {
		public static int Width => 28;
		public static int Height => 14;
		public static float Scale => 0.65f;
		public static int CylinderCapacity => 6;



		////////////////

		public static bool IsAmmoSourceAvailable( Player player, bool skipSpeedloaders ) {
			if( TMRConfig.Instance.InfiniteAmmoCheat ) {
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
						return true;
					}
				}
			}

			if( TMRConfig.Instance.BandolierNeededToReload ) {
				int bandolierType = ModContent.ItemType<BandolierItem>();
				int max = PlayerItemHelpers.GetCurrentVanillaMaxAccessories( player );

				for( int i = PlayerItemHelpers.VanillaAccessorySlotFirst; i < max; i++ ) {
					Item item = player.armor[i];
					if( item == null || item.IsAir || item.type != bandolierType ) {
						continue;
					}

					return true;
				}
			}

			return false;
		}



		////////////////

		private int[] Cylinder;

		private int CylinderIdx = 0;

		private int ElapsedTimeSinceLastShotAttempt = 0;


		////////////////

		public override bool CloneNewInstances => false;



		////////////////

		public TheMadRangerItem() {
			this.Cylinder = new int[TheMadRangerItem.CylinderCapacity];
			for( int i=0; i<TheMadRangerItem.CylinderCapacity; i++ ) {
				this.Cylinder[i] = 1;
			}
		}

		public override void SetStaticDefaults() {
			this.DisplayName.SetDefault( "The Mad Ranger" );
			this.Tooltip.SetDefault( "An antique .357 revolver from a far away land."
				+ "\nUnusually powerful; needs a steady hand"
				+ "\nOnly uses a particular, manufactured ammo"
			);
		}

		public override void SetDefaults() {
			this.item.width = TheMadRangerItem.Width;
			this.item.height = TheMadRangerItem.Height;
			this.item.scale = TheMadRangerItem.Scale;

			this.item.ranged = true;
			this.item.useStyle = 5;
			this.item.useTime = 6;
			this.item.useAnimation = 6;
			this.item.autoReuse = false;
			this.item.noMelee = true;
			this.item.shoot = ProjectileID.Bullet;
			this.item.shootSpeed = 32f;

			//this.item.UseSound = this.mod.GetLegacySoundSlot( SoundType.Custom, "Sounds/Custom/RevolverFire" )
			//	.WithVolume( 0.25f );
			////.WithPitchVariance( 0.5f );

			this.item.damage = TMRConfig.Instance.MaximumAimedGunDamage;
			this.item.knockBack = 4;

			this.item.rare = 4;
			this.item.value = Item.buyPrice( 0, 5, 0, 0 );
		}


		////////////////

		public override void Load( TagCompound tag ) {
			if( !tag.ContainsKey("cylinder_idx") ) {
				return;
			}

			this.CylinderIdx = tag.GetInt( "cylinder_idx" );

			for( int i = 0; i < this.Cylinder.Length; i++ ) {
				if( !tag.ContainsKey("cylinder_round_" + i) ) {
					break;
				}
				this.Cylinder[i] = tag.GetInt( "cylinder_round_" + i );
			}
		}

		public override TagCompound Save() {
			var tag = new TagCompound {
				{ "cylinder_idx", this.CylinderIdx }
			};
			for( int i = 0; i < this.Cylinder.Length; i++ ) {
				tag["cylinder_round_" + i ] = this.Cylinder[i];
			}

			return tag;
		}


		////////////////

		public override void UpdateInventory( Player player ) {
			this.ElapsedTimeSinceLastShotAttempt++;

			if( TMRConfig.Instance.DebugModeInfo ) {
				DebugHelpers.Print( "cylinder", this.CylinderIdx + " = " + string.Join( ", ", this.Cylinder ) );
			}
		}


		////////////////

		public override bool CanUseItem( Player player ) {
			return player.GetModPlayer<TMRPlayer>().CanAttemptToShootGun();
		}

		public override bool Shoot(
					Player player,
					ref Vector2 position,
					ref float speedX,
					ref float speedY,
					ref int type,
					ref int damage,
					ref float knockBack ) {
			bool canShoot = this.AttemptGunShot( player, ref speedX, ref speedY, ref damage, ref knockBack );
			return canShoot;
		}


		////////////////

		public bool IsCylinderFull() {
			return this.Cylinder.All( c => c == 1 );
		}

		public bool IsCylinderEmpty() {
			return this.Cylinder.All( c => c == 0 );
		}
	}
}