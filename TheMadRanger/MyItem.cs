using System;
using Terraria;
using Terraria.ModLoader;
using HamstarHelpers.Helpers.Debug;


namespace TheMadRanger {
	class TMRGlobalItem : GlobalItem {
		public override bool CanUseItem( Item item, Player player ) {
			return !player.GetModPlayer<TMRPlayer>().GunAnim.IsAnimating;
		}
	}
}
