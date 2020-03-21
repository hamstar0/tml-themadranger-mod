using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using HamstarHelpers.Helpers.Debug;


namespace TheMadRanger.Items.Weapons {
	public partial class TheMadRangerItem : ModItem {
		public static int Width { get; } = 28;
		public static int Height { get; } = 14;
		public static float Scale { get; } = 0.65f;



		////////////////

		private int[] Cylinder = new int[6] { 1, 1, 1, 1, 1, 1 };

		private int CylinderIdx = 0;

		private int ElapsedTimeSinceLastShotAttempt = 0;


		////////////////

		public override bool CloneNewInstances => false;



		////////////////

		public override void SetStaticDefaults() {
			this.DisplayName.SetDefault( "The Mad Ranger" );
			this.Tooltip.SetDefault( "An antique gun from a far away land."
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

			this.item.rare = 2;
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


		////

		public bool IsCylinderFull() {
			return this.Cylinder.All( c => c == 1 );
		}

		public bool IsCylinderEmpty() {
			return this.Cylinder.All( c => c == 0 );
		}
	}
}