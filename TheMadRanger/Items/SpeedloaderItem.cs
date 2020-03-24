using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using TheMadRanger.Helpers.Misc;


namespace TheMadRanger.Items {
	class SpeedloaderItem : ModItem {
		public static int Width = 12;
		public static int Height = 12;



		////////////////

		public int LoadedRounds { get; private set; } = 0;


		////////////////

		public override bool CloneNewInstances => false;



		////////////////

		public override void SetStaticDefaults() {
			this.DisplayName.SetDefault( "Speedloader" );
			this.Tooltip.SetDefault( "Quickly loads a revolver cylinder." );
		}

		public override void SetDefaults() {
			this.item.width = SpeedloaderItem.Width;
			this.item.height = SpeedloaderItem.Height;
			this.item.maxStack = 1;
			this.item.value = Item.buyPrice( 0, 2, 5, 0 );
			this.item.rare = 5;
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

		public void TransferRounds( Player player ) {
			this.LoadedRounds = 0;

			SoundHelpers.PlaySound( "RevolverReloadRound", player.Center, 0.5f );
		}
	}
}
