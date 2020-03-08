using Terraria.ModLoader;


namespace TheOlBigIron {
	public class TOBIMod : Mod {
		public static TOBIMod Instance { get; private set; }


		////////////////

		public static string GithubUserName => "hamstar0";
		public static string GithubProjectName => "tml-theolbigiron-mod";



		////////////////

		public TOBIMod() {
			TOBIMod.Instance = this;
		}

		public override void Unload() {
			TOBIMod.Instance = null;
		}
	}
}