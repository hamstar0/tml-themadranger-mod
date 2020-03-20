using HamstarHelpers.Classes.UI.ModConfig;
using HamstarHelpers.Services.Configs;
using System;
using System.ComponentModel;
using Terraria.ModLoader.Config;


namespace TheMadRanger {
	class MyFloatInputElement : FloatInputElement { }




	public partial class TMRConfig : StackableModConfig {
		public static TMRConfig Instance => ModConfigStack.GetMergedConfigs<TMRConfig>();



		////////////////

		public override ConfigScope Mode => ConfigScope.ServerSide;


		////////////////

		//[DefaultValue( true )]
		//public bool RecipeEnabled { get; set; } = true;


		[DefaultValue( true )]
		public bool PlayerSpawnsWithGun { get; set; } = true;

		[DefaultValue( true )]
		public bool PlayerSpawnsWithBandolier { get; set; } = true;


		[Range( 1, 60 * 60 * 60 )]
		[DefaultValue( 90 )]
		public int AimModeActivationTickDurationWhileIdling { get; set; } = 90;

		[Range( 0, 60 * 60 )]
		[DefaultValue( 60 )]
		public int ReloadInitTickDuration { get; set; } = 60;

		[Range( 0, 60 * 60 )]
		[DefaultValue( 45 )]
		public int ReloadRoundTickDuration { get; set; } = 45;

		[Range( 0, 60 * 60 )]
		[DefaultValue( 60 )]
		public int HolsterTwirlTickDuration { get; set; } = 60;

		[Range( 0, 60 * 60 )]
		[DefaultValue( 15 )]
		public int QuickDrawTickDuration { get; set; } = 15;


		[Range( 1, 1000 )]
		[DefaultValue( 40 )]
		[ReloadRequired]
		public int MaximumGunDamage { get; set; } = 40;

		[Range( 0f, 360f )]
		[DefaultValue( 30f )]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float UnaimedConeDegreesRange { get; set; } = 30f;
	}
}
