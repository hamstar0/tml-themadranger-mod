using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using TheMadRanger.Helpers.Misc;


namespace TheMadRanger.Items {
	class SpeedloaderItem : ModItem {
		public static int Width = 10;
		public static int Height = 10;



		////////////////

		public int LoadedRounds { get; private set; } = 0;


		////////////////

		public override bool CloneNewInstances => false;

		public override string Texture {
			get {
				if( this.LoadedRounds > 0 ) {
					return "Items/SpeedloaderItem_Loaded";
				}
				return base.Texture;
			}
		}



		////////////////

		public override void SetStaticDefaults() {
			this.DisplayName.SetDefault( "Speedloader" );
			this.Tooltip.SetDefault( "Quickly loads a revolver cylinder." );
		}

		public override void SetDefaults() {
			this.item.width = SpeedloaderItem.Width;
			this.item.height = SpeedloaderItem.Height;
			this.item.maxStack = 1;
			this.item.value = Item.buyPrice( 0, 1, 0, 0 );
			this.item.rare = 4;
			this.item.scale = 0.75f;
		}


		////////////////

		public override void Load( TagCompound tag ) {
			if( !tag.ContainsKey( "rounds" ) ) {
				return;
			}

			this.LoadedRounds = tag.GetInt( "rounds" );
		}

		public override TagCompound Save() {
			var tag = new TagCompound {
				{ "rounds", this.LoadedRounds }
			};

			return tag;
		}


		////////////////

		public override bool CanRightClick() {
			return true;
		}

		public override bool ConsumeItem( Player player ) {
			if( this.LoadedRounds == 0 ) {
				return false;
			}

			if( )

			return false;
		}


		////////////////

		public void TransferRounds( Player player ) {
			this.LoadedRounds = 0;

			SoundHelpers.PlaySound( "RevolverReloadRound", player.Center, 0.5f );
		}
	}
}
