using Terraria.ModLoader;


namespace BigIron {
	public class BigIronMod : Mod {
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
	}
}