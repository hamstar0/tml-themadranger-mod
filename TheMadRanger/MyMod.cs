using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheMadRanger.HUD;
using HUDElementsLib;


namespace TheMadRanger {
	public partial class TMRMod : Mod {
		public static TMRMod Instance { get; private set; }


		////////////////

		public static string GithubUserName => "hamstar0";
		public static string GithubProjectName => "tml-themadranger-mod";



		////////////////

		internal ModHotKey ReloadKey = null;

		public AmmoDisplayHUD AmmoHUD { get; private set; }



		////////////////

		public TMRMod() {
			TMRMod.Instance = this;
		}

		public override void Load() {
			this.ReloadKey = this.RegisterHotKey( "Reload", "R" );
		}

		public override void Unload() {
			TMRMod.Instance = null;
		}


		////

		public override void PostSetupContent() {
			if( !Main.dedServ && Main.netMode != NetmodeID.Server ) {
				this.AmmoHUD = AmmoDisplayHUD.CreateDefault();

				HUDElementsLibAPI.AddWidget( this.AmmoHUD );
			}
		}


		////////////////

		/*internal float RotRad = 0f;
		internal int RotDeg = 0;
		public override void PostSetupContent() {
			CustomHotkeys.BindActionToKey1( "TheMadRangerRotAngInc", () => {
				this.RotRad += 0.0174533f;
				this.RotDeg += 1;
				Main.NewText( "+1 deg ("+this.RotDeg+")" );
			} );
			CustomHotkeys.BindActionToKey2( "TheMadRangerRotAngDec", () => {
				this.RotRad -= 0.0174533f;
				this.RotDeg -= 1;
				Main.NewText( "-1 deg ("+this.RotDeg+")" );
			} );
			//candidate 1: +21 deg
			//candidate 2: +28 deg
			//candidate 3: +36 deg
			//candidate 4: -28 deg
			//candidate 5: -37 deg
			//candidate 6: -66 deg
			//30?
		}*/
	}
}