using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using TheMadRanger.Recipes;


namespace TheMadRanger.Items {
	public partial class SpeedloaderItem : ModItem {
		public static int Width = 7;
		public static int Height = 7;



		////////////////

		public int LoadedRounds { get; private set; } = 0;


		////////////////

		public override bool CloneNewInstances => false;

		public override string Texture {
			get {
				if( this.LoadedRounds > 0 ) {
					return "TheMadRanger/Items/SpeedloaderItem_Loaded";
				}
				return base.Texture;
			}
		}



		////////////////

		public override void SetStaticDefaults() {
			this.DisplayName.SetDefault( ".357 Speedloader" );
			this.Tooltip.SetDefault(
				"Quickly swaps in a pre-loaded \"Mad Ranger\" cylinder."
				+"\nRight-click to pre-load ammo (needs Bandolier equipped)"
			);
		}

		public override void SetDefaults() {
			this.item.width = SpeedloaderItem.Width;
			this.item.height = SpeedloaderItem.Height;
			this.item.maxStack = 1;
			this.item.value = Item.buyPrice( 0, 5, 0, 0 );
			this.item.rare = 4;
			//this.item.ammo = this.item.type;	Isn't reloadable in ammo slots
			this.item.scale = 0.75f;
		}


		////////////////

		public override void AddRecipes() {
			var recipe = new SpeedloaderRecipe( this );
			recipe.AddRecipe();
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

		public override void NetRecieve( BinaryReader reader ) {
			this.LoadedRounds = reader.ReadInt32();
		}

		public override void NetSend( BinaryWriter writer ) {
			writer.Write( (int)this.LoadedRounds );
		}


		////////////////

		public override bool CanRightClick() {
			return true;
		}

		public override bool ConsumeItem( Player player ) {
			if( this.LoadedRounds > 0 ) {
				return false;
			}

			this.AttemptReload( player );

			return false;
		}
	}
}
