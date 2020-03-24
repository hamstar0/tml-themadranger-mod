using HamstarHelpers.Classes.UI.ModConfig;
using HamstarHelpers.Services.Configs;
using System;
using System.ComponentModel;
using Terraria.ModLoader.Config;


namespace TheMadRanger {
	class MyFloatInputElement : FloatInputElement { }




	[Label( "The Mad Ranger" )]
	public partial class TMRConfig : StackableModConfig {
		public static TMRConfig Instance => ModConfigStack.GetMergedConfigs<TMRConfig>();



		////////////////

		public override ConfigScope Mode => ConfigScope.ServerSide;


		////////////////

		[DefaultValue( false )]
		public bool DebugModeInfo { get; set; } = false;

		//

		[DefaultValue( false )]
		public bool InfiniteAmmoCheat { get; set; } = false;


		//

		//[DefaultValue( true )]
		//public bool RecipeEnabled { get; set; } = true;


		[DefaultValue( true )]
		public bool PlayerSpawnsWithGun { get; set; } = true;

		[DefaultValue( true )]
		public bool PlayerSpawnsWithBandolier { get; set; } = true;

		[DefaultValue( true )]
		public bool BandolierNeededToReload { get; set; } = true;


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
		[DefaultValue( 60 )]
		[ReloadRequired]
		public int MaximumAimedGunDamage { get; set; } = 60;

		[Range( 1, 999999 )]
		[DefaultValue( 10 )]
		[ReloadRequired]
		public int MinimumUnaimedGunDamage { get; set; } = 10;

		[Range( 1, 999999 )]
		[DefaultValue( 40 )]
		[ReloadRequired]
		public int MaximumUnaimedGunDamage { get; set; } = 40;

		[Range( 0f, 360f )]
		[DefaultValue( 30f )]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float UnaimedConeDegreesRange { get; set; } = 30f;


		[Range( 1, 60 * 60 * 60 )]
		[DefaultValue( 90 )]
		public int AimModeActivationThreshold { get; set; } = 90;


		[Range( 0f, 60f )]
		[DefaultValue( 1f )]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float AimModeDepleteRateWhilePlayerMoving { get; set; } = 1f;

		[Range( 0f, 60f )]
		[DefaultValue( 0.1f )]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float AimModeDepleteRateWhileMouseMoving { get; set; } = 0.1f;

		[Range( 0f, 60f )]
		[DefaultValue( 1f )]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float AimModeBuildupRateWhileIdle { get; set; } = 1f;

		[Range( 0f, 9999f )]
		[DefaultValue( 10f )]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float AimModeBufferAddedThreshold { get; set; } = 10f;

		[Range( 0f, 9999f )]
		[DefaultValue( 5f )]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float AimModeOnHitBuildupAmount { get; set; } = 5f;

		[Range( 0f, 9999f )]
		[DefaultValue( 5f )]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float AimModeOnMissLossAmount { get; set; } = 5f;
	}
}
