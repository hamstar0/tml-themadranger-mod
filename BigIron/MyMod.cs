using Terraria;
using Terraria.ModLoader;


namespace BigIron {
	public partial class BigIronMod : Mod {
		public static BigIronMod Instance { get; private set; }


		////////////////

		public static string GithubUserName => "hamstar0";
		public static string GithubProjectName => "tml-theolbigiron-mod";



		////////////////

		public BigIronMod() {
			BigIronMod.Instance = this;
		}

		public override void Unload() {
			BigIronMod.Instance = null;
		}


		////////////////

		/*internal float RotRad = 0f;
		internal int RotDeg = 0;
		public override void PostSetupContent() {
			CustomHotkeys.BindActionToKey1( "BigIronRotAngInc", () => {
				this.RotRad += 0.0174533f;
				this.RotDeg += 1;
				Main.NewText( "+1 deg ("+this.RotDeg+")" );
			} );
			CustomHotkeys.BindActionToKey2( "BigIronRotAngDec", () => {
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