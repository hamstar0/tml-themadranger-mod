using System;
using Terraria;
using Terraria.ModLoader;
using HamstarHelpers.Helpers.Debug;


namespace BigIron {
	class BigIronGlobalItem : GlobalItem {
		public override bool CanUseItem( Item item, Player player ) {
			return !player.GetModPlayer<BigIronPlayer>().GunAnim.IsHolstering;
		}
	}
}
