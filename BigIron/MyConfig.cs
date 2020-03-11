using HamstarHelpers.Classes.UI.ModConfig;
using HamstarHelpers.Services.Configs;
using System;
using System.ComponentModel;
using Terraria.ModLoader.Config;


namespace BigIron {
	class MyFloatInputElement : FloatInputElement { }




	public partial class BigIronConfig : StackableModConfig {
		public static BigIronConfig Instance => ModConfigStack.GetMergedConfigs<BigIronConfig>();



		////////////////

		public override ConfigScope Mode => ConfigScope.ServerSide;


		////////////////

		[DefaultValue( true )]
		public bool RecipeEnabled { get; set; } = true;


		[DefaultValue( true )]
		public bool PlayerSpawnsWithGun { get; set; } = true;

		[DefaultValue( true )]
		public bool PlayerSpawnsWithBandolier { get; set; } = true;


		[DefaultValue( 40 )]
		public int MaximumGunDamage { get; set; } = 40;
	}
}
