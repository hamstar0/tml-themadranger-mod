using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;


namespace BigIron.Items.Weapons {
	public partial class BigIronItem : ModItem {
		public static int Width { get; } = 24;
		public static int Height { get; } = 16;
		public static float Scale { get; } = 0.5f;



		////////////////

		private bool[] Cylinder = new bool[6] { true, true, true, true, true, true };

		private int CylinderPos = 0;

		private SoundStyle FireSound;
		private SoundStyle DryFireSound;
		private SoundStyle ReloadSound;


		////////////////

		public override bool CloneNewInstances => false;



		////////////////

		public override void SetStaticDefaults() {
			this.DisplayName.SetDefault( "Ol' Big Iron" );
			this.Tooltip.SetDefault( "An antique gun from a far away land."
				+ "\nAn unusually powerful hand gun; needs a steady hand"
				+ "\nOnly uses a specific, manufactured ammo"
			);
		}

		public override void SetDefaults() {
			this.item.width = BigIronItem.Width;
			this.item.height = BigIronItem.Height;
			this.item.scale = BigIronItem.Scale;

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

		public override bool CanUseItem( Player player ) {
			return !player.GetModPlayer<BigIronPlayer>().CanShootGun();
		}


		////////////////

		internal bool Shoot( Player player ) {
			if( this.CylinderShoot() ) {
				if( this.FireSound == null ) {
					this.FireSound = BigIronMod.Instance.GetLegacySoundSlot(
						Terraria.ModLoader.SoundType.Custom,
						"Sounds/Custom/RevolverFire"
					).WithVolume( 0.2f );
				}

				Main.PlaySound( (LegacySoundStyle)this.FireSound, player.Center );
			} else {
				if( this.DryFireSound == null ) {
					this.DryFireSound = BigIronMod.Instance.GetLegacySoundSlot(
						Terraria.ModLoader.SoundType.Custom,
						"Sounds/Custom/RevolverDryFire"
					).WithVolume( 0.2f );
				}

				Main.PlaySound( (LegacySoundStyle)this.DryFireSound, player.Center );
			}

			return true;
		}

		////

		public bool ReloadRound( Player player ) {
			int initPos = this.CylinderPos;

			do {
				if( this.CylinderReload() ) {
					if( this.ReloadSound == null ) {
						this.ReloadSound = BigIronMod.Instance.GetLegacySoundSlot(
							Terraria.ModLoader.SoundType.Custom,
							"Sounds/Custom/RevolverReload"
						).WithVolume( 1f );
					}

					Main.PlaySound( (LegacySoundStyle)this.ReloadSound, player.Center );

					return true;
				}
			} while( this.CylinderPos != initPos );

			return false;
		}


		////////////////
		
		private bool CylinderShoot() {
			bool canShoot = this.Cylinder[ this.CylinderPos ];

			this.Cylinder[ this.CylinderPos ] = false;
			this.CylinderPos = (this.CylinderPos + 1) % 6;

			return canShoot;
		}

		private bool CylinderReload() {
			bool isLoaded = this.Cylinder[ this.CylinderPos ];

			this.Cylinder[ this.CylinderPos ] = true;
			this.CylinderPos = (this.CylinderPos + 1) % 6;

			return isLoaded;
		}
	}
}