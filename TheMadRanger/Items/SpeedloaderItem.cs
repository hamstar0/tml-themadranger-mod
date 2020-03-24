using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using HamstarHelpers.Services.Timers;
using TheMadRanger.Helpers.Misc;
using TheMadRanger.Items.Weapons;


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
			if( this.LoadedRounds > 0 ) {
				return false;
			}

			this.AttemptReload( player );

			return false;
		}


		////////////////

		public bool AttemptReload( Player player ) {
			int plrWho = player.whoAmI;

			bool Reload() {
				Player plr = Main.player[plrWho];
				if( !TheMadRangerItem.IsAmmoSourceAvailable( plr, true ) ) {
					return false;
				}

				this.LoadedRounds = TheMadRangerItem.CylinderCapacity;
				return true;
			}

			//

			if( !TheMadRangerItem.IsAmmoSourceAvailable(player, true) ) {
				return false;
			}

			var myplayer = player.GetModPlayer<TMRPlayer>();

			if( myplayer.GunHandling.IsAnimating ) {
				return false;
			}

			string timerName = "TheMadRangerSpeedloaderLoad_" + plrWho;

			if( Timers.GetTimerTickDuration(timerName) > 0 ) {
				return false;
			}

			SoundHelpers.PlaySound( "RevolverReloadBegin", player.Center, 0.5f );

			Timers.SetTimer( timerName, TMRConfig.Instance.SpeedloaderLoadTickDuration, false, () => {
				Reload();
				return false;
			} );

			return true;
		}

		////

		public void TransferRoundsOut( Player player ) {
			this.LoadedRounds = 0;

			SoundHelpers.PlaySound( "RevolverReloadRound", player.Center, 0.5f );
		}
	}
}
