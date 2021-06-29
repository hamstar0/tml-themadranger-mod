using System;
using System.ComponentModel;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using ModLibsCore.Classes.UI.ModConfig;


namespace TheMadRanger {
	public partial class TMRConfig : ModConfig {
		[Label( "Tick duration of reload sequence start" )]
		[Range( 0, 60 * 60 )]
		[DefaultValue( 60 )]
		public int ReloadInitTickDuration { get; set; } = 60;

		[Label( "Tick duration of reload for each round" )]
		[Range( 0, 60 * 60 )]
		[DefaultValue( 28 )]	//35?
		public int ReloadRoundTickDuration { get; set; } = 28;

		[Label( "Tick duration of holster twirl animation" )]
		[Range( 0, 60 * 60 )]
		[DefaultValue( 50 )]
		public int HolsterTwirlTickDuration { get; set; } = 50;

		//

		[Label( "Tick duration of \"quick draw\" mode" )]
		[Range( 0, 60 * 60 )]
		[DefaultValue( 30 )]
		public int QuickDrawTickDuration { get; set; } = 30;


		//

		[Label( "Tick duration of speedloader reloads" )]
		[Range( 0, 60 * 60 )]
		[DefaultValue( 180 )]
		public int SpeedloaderLoadTickDuration { get; set; } = 180;
	}
}
