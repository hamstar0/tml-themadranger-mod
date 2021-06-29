using System;
using System.ComponentModel;
using Terraria.ModLoader.Config;


namespace TheMadRanger {
	public partial class TMRConfig : ModConfig {
		[Label( "Tick duration before \"aim mode\" activates" )]
		[Range( 1, 60 * 60 * 60 )]
		[DefaultValue( 90 )]
		public int AimModeActivationTickDuration { get; set; } = 90;

		[Label( "Added tick duration before \"aim mode\" activates" )]
		[Range( 0, 9999 )]
		[DefaultValue( 10 )]
		public int AimModeActivationTickDurationAddedBuffer { get; set; } = 10;


		[Label( "Rate of \"aim mode\" increase while player moves per tick" )]
		[Range( -60f, 60f )]
		[DefaultValue( -0.5f )]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float AimModeOnPlayerMoveBuildupAmount { get; set; } = -0.5f;

		[Label( "Rate of \"aim mode\" increase while mouse moves per tick" )]
		[Range( -60f, 60f )]
		[DefaultValue( 0f )] //-0.2f?
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float AimModeOnMouseMoveBuildupAmount { get; set; } = 0f;

		[Label( "Rate of \"aim mode\" increase while player idle per tick" )]
		[Range( -60f, 60f )]
		[DefaultValue( 1f )]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float AimModeOnIdleBuildupAmount { get; set; } = 1f;

		[Label( "\"Aim mode\" increase from landing shots" )]
		[Range( -999f, 999f )]
		[DefaultValue( 5f )]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float AimModeOnHitBuildupAmount { get; set; } = 5f;

		[Label( "\"Aim mode\" increase from missing shots" )]
		[Range( -999f, 999f )]
		[DefaultValue( -5f )]	//-5f
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float AimModeOnMissBuildupAmount { get; set; } = -5f;


		[Label( "Mouse move distance-per-tick to count as 'moving'" )]
		[Range( 0f, 1000f )]
		[DefaultValue( 1000f )]	//1f
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float AimModeMouseMoveThreshold { get; set; } = 1000f;

		//

		[Label( "Scale to adjust max movement speed while aim mode 'locked'" )]
		[Range( 0f, 5f )]
		[DefaultValue( 0.25f )]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float AimModeLockMoveSpeedScale { get; set; } = 0.25f;
	}
}
