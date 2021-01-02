using System;
using System.ComponentModel;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using HamstarHelpers.Classes.UI.ModConfig;


namespace TheMadRanger {
	public partial class TMRConfig : ModConfig {
		[Label( "Percent chance of a bandit to spawn on the surface at day" )]
		[Range( 0f, 1f )]
		[DefaultValue( 0.01f )]
		[CustomModConfigItem( typeof(MyFloatInputElement) )]
		public float BanditSpawnChance { get; set; } = 0.01f;

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
	}
}
