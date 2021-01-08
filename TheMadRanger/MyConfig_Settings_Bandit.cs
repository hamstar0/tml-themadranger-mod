using System;
using System.ComponentModel;
using Terraria.ModLoader.Config;


namespace TheMadRanger {
	public partial class TMRConfig : ModConfig {
		[Label( "Percent chance of a bandit to spawn on the surface at day" )]
		[Range( 0f, 1f )]
		[DefaultValue( 0.03f )]
		[CustomModConfigItem( typeof(MyFloatInputElement) )]
		public float BanditSpawnChance { get; set; } = 0.03f;

		[Label( "Percent chance of a bandit to spawn as a combo" )]
		[Range( 0f, 1f )]
		[DefaultValue( 0.25f )]
		[CustomModConfigItem( typeof(MyFloatInputElement) )]
		public float BanditComboSpawnChance { get; set; } = 0.25f;

		[Label( "Percent chance of a bandit to chain its spawn combo" )]
		[Range( 0f, 1f )]
		[DefaultValue( 0.7f )]
		[CustomModConfigItem( typeof(MyFloatInputElement) )]
		public float BanditComboChainSpawnChance { get; set; } = 0.7f;

		//

		[Range( 0f, 1f )]
		[DefaultValue( 0.05f )]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float BanditLootGunDropPercentChance { get; set; } = 0.05f;

		[Range( 0f, 1f )]
		[DefaultValue( 0.02f )]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float BanditLootSpeedloaderDropPercentChance { get; set; } = 0.02f;

		[Range( 0f, 1f )]
		[DefaultValue( 0.05f )]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float BanditLootBandolierDropPercentChance { get; set; } = 0.05f;

		//

		[Range( 0f, 1f )]
		[DefaultValue( 0.15f )]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float BanditTotalDamageSkittishnessPercent { get; set; } = 0.15f;
	}
}
