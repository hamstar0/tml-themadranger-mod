using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;


namespace TheMadRanger.Items.Weapons {
	public partial class TheMadRangerItem : ModItem {
		public static int Width { get; } = 24;
		public static int Height { get; } = 16;
		public static float Scale { get; } = 0.5f;



		////////////////

		private bool[] Cylinder = new bool[6] { true, true, true, true, true, true };

		private int CylinderPos = 0;

		private int ElapsedTimeSinceLastShotAttempt = 0;

		private SoundStyle FireSound;
		private SoundStyle DryFireSound;
		private SoundStyle ReloadBeginSound;
		private SoundStyle ReloadRoundSound;
		private SoundStyle ReloadEndSound;


		////////////////

		public bool IsCylinderOpen { get; private set; } = false;

		////

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

			this.item.damage = BigIronConfig.Instance.MaximumGunDamage;
			this.item.knockBack = 4;

			this.item.rare = 2;
			this.item.value = Item.buyPrice( 0, 5, 0, 0 );
		}


		////////////////

		public override void Load( TagCompound tag ) {
			if( !tag.ContainsKey("cylinder_pos") ) {
				return;
			}

			this.CylinderPos = tag.GetInt( "cylinder_pos" );

			for( int i = 0; i < 6; i++ ) {
				this.Cylinder[i] = tag.GetBool( "cylinder_" + i );
			}
		}

		public override TagCompound Save() {
			var tag = new TagCompound {
				{ "cylinder_pos", this.CylinderPos }
			};
			for( int i=0; i<6; i++ ) {
				tag[ "cylinder_"+i ] = this.Cylinder[i];
			}

			return tag;
		}


		////////////////

		public override void UpdateInventory( Player player ) {
			this.ElapsedTimeSinceLastShotAttempt++;
		}


		////////////////

		public override bool CanUseItem( Player player ) {
			return !player.GetModPlayer<TMRPlayer>().CanShootGun();
		}
	}
}