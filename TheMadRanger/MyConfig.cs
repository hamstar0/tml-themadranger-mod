using System;
using System.ComponentModel;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using HamstarHelpers.Classes.UI.ModConfig;


namespace TheMadRanger {
	class MyFloatInputElement : FloatInputElement { }




	[Label( "The Mad Ranger" )]
	public partial class TMRConfig : ModConfig {
		public static TMRConfig Instance => ModContent.GetInstance<TMRConfig>();



		////////////////

		public override ConfigScope Mode => ConfigScope.ServerSide;
	}
}
