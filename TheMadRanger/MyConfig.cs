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

		[DefaultValue( false )]
		public bool DebugModeInfo { get; set; } = false;

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
		[DefaultValue( 35 )]
		public int ReloadRoundTickDuration { get; set; } = 35;

		[Range( 0, 60 * 60 )]
		[DefaultValue( 60 )]
		public int HolsterTwirlTickDuration { get; set; } = 60;

		[Range( 0, 60 * 60 )]
		[DefaultValue( 30 )]
		public int QuickDrawTickDuration { get; set; } = 30;


		[Range( 1, 999999 )]
		[DefaultValue( 50 )]
		[ReloadRequired]
		public int MaximumAimedGunDamage { get; set; } = 50;

		[Range( 1, 999999 )]
		[DefaultValue( 40 )]
		[ReloadRequired]
		public int MaximumUnaimedGunDamage { get; set; } = 40;

		[Range( 0f, 360f )]
		[DefaultValue( 30f )]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float UnaimedConeDegreesRange { get; set; } = 30f;


		[Range( 0f, 30f )]
		[DefaultValue( 1.5f )]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float AimModeDepleteRateWhilePlayerMoving { get; set; } = 1.5f;

		[Range( 0f, 30f )]
		[DefaultValue( 0.5f )]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float AimModeDepleteRateWhileMouseMoving { get; set; } = 0.5f;

		[Range( 0f, 30f )]
		[DefaultValue( 1f )]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float AimModeBuildupRateWhileIdle { get; set; } = 1f;
	}
}
