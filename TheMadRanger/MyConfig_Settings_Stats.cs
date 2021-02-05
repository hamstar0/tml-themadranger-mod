using System;
using System.ComponentModel;
using Terraria.ModLoader.Config;


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
		[DefaultValue( 40f )]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float UnaimedConeDegreesRange { get; set; } = 40f;

		//

		[Label( "Shot damage added per 32^2 unit of target's area" )]
		[Range( -100, 100 )]
		[DefaultValue( -3 )]
		public int DamagePerTargetVolumeUnitsOf32Sqr { get; set; } = -3;

		[Label( "Shot damage scale against bosses" )]
		[Range( 0f, 10f )]
		[DefaultValue( 1f )]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float DamageScaleAgainstBosses { get; set; } = 1f;
	}
}
