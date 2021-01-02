using System;
using System.ComponentModel;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using HamstarHelpers.Classes.UI.ModConfig;


namespace TheMadRanger {
	public partial class TMRConfig : ModConfig {
		[Label( "Maximum damage of gun while aimed" )]
		[Range( 1, 999999 )]
		[DefaultValue( 60 )]
		[ReloadRequired]
		public int MaximumAimedGunDamage { get; set; } = 60;

		[Label( "Minimum damage of gun while un-aimed" )]
		[Range( 1, 999999 )]
		[DefaultValue( 40 )]	//10?
		[ReloadRequired]
		public int MinimumUnaimedGunDamage { get; set; } = 40;

		[Label( "Maximum damage of gun while un-aimed" )]
		[Range( 1, 999999 )]
		[DefaultValue( 60 )]	//40?
		[ReloadRequired]
		public int MaximumUnaimedGunDamage { get; set; } = 60;

		[Label( "Degrees range of un-aimed shoots" )]
		[Range( 0f, 360f )]
		[DefaultValue( 30f )]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float UnaimedConeDegreesRange { get; set; } = 30f;
	}
}
