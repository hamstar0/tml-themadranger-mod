using System;
using System.ComponentModel;
using Terraria.ModLoader.Config;


namespace TheMadRanger {
	public partial class TMRConfig : ModConfig {
		public bool DebugModeGunInfo { get; set; } = false;

		public bool DebugModeBanditInfo { get; set; } = false;

		//

		[DefaultValue( false )]
		public bool InfiniteAmmoCheat { get; set; } = false;


		//

		[Label("Color intensity of aim reticule")]
		[Range( 0f, 1f )]
		[DefaultValue( 0.8f )]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float ReticuleIntensityPercent { get; } = 0.8f;


		//

		[DefaultValue( false )]
		public bool RecipeAvailableForTheMadRanger { get; set; } = false;

		[DefaultValue( false )]
		public bool RecipeAvailableForBandolier { get; set; } = false;

		[DefaultValue( true )]
		public bool RecipeAvailableForSpeedloader { get; set; } = true;

		//

		[DefaultValue( true )]
		public bool PlayerSpawnsWithGun { get; set; } = true;

		[DefaultValue( true )]
		public bool PlayerSpawnsWithBandolier { get; set; } = true;

		[DefaultValue( true )]
		public bool BandolierNeededToReload { get; set; } = true;


		//

		[Range( -4096f, 4096f )]
		[DefaultValue( -96f )]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float AmmoHUDPositionX { get; set; } = -96f;

		[Range( -2048, 2048 )]
		[DefaultValue( -128 )]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float AmmoHUDPositionY { get; set; } = -128;
	}
}
