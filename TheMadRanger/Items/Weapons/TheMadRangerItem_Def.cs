using HamstarHelpers.Helpers.Debug;
using Microsoft.Xna.Framework;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;


namespace TheMadRanger.Items.Weapons {
	public partial class TheMadRangerItem : ModItem {
		public static int Width { get; } = 24;
		public static int Height { get; } = 16;
		public static float Scale { get; } = 0.5f;



		////////////////

		private int[] Cylinder = new int[6] { 1, 1, 1, 1, 1, 1 };

		private int CylinderPos = 0;

		private int ElapsedTimeSinceLastShotAttempt = 0;


		////////////////

		public override bool CloneNewInstances => false;



		////////////////

		public override void SetStaticDefaults() {
			this.DisplayName.SetDefault( "The Mad Ranger" );
			this.Tooltip.SetDefault( "A temperamental gun from a far away land."
				+ "\nUnusually powerful; needs a steady hand"
				+ "\nOnly uses a specific, manufactured ammo"
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
			this.item.shootSpeed = 14f;

			//this.item.UseSound = this.mod.GetLegacySoundSlot( SoundType.Custom, "Sounds/Custom/RevolverFire" )
			//	.WithVolume( 0.25f );
			////.WithPitchVariance( 0.5f );

			this.item.damage = TMRConfig.Instance.MaximumGunDamage;
			this.item.knockBack = 4;

			this.item.rare = 2;
			this.item.value = Item.buyPrice( 0, 5, 0, 0 );
		}


		////////////////

		public override void Load( TagCompound tag ) {
			if( !tag.ContainsKey("cylinder_idx") ) {
				return;
			}

			this.CylinderPos = tag.GetInt( "cylinder_idx" );

			for( int i = 0; i < 6; i++ ) {
				this.Cylinder[i] = tag.GetInt( "cylinder_round_" + i );
			}
		}

		public override TagCompound Save() {
			var tag = new TagCompound {
				{ "cylinder_idx", this.CylinderPos }
			};
			for( int i=0; i<6; i++ ) {
				tag["cylinder_round_" + i ] = this.Cylinder[i];
			}

			return tag;
		}


		////////////////

		public override void UpdateInventory( Player player ) {
			this.ElapsedTimeSinceLastShotAttempt++;
DebugHelpers.Print( "cylinder", this.CylinderPos+" - "+string.Join( ", ", this.Cylinder) );
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

		public bool IsCylinderEmpty() {
			return this.Cylinder.Any( c => c != 0 );
		}
	}
}