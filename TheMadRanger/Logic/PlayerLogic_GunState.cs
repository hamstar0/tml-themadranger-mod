using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using HamstarHelpers.Helpers.Debug;
using TheMadRanger.Items.Weapons;
using TheMadRanger.NetProtocols;


namespace TheMadRanger.Logic {
	partial class PlayerLogic {
		public static bool IsHoldingGun( Player player ) {
			Item heldItem = player.HeldItem;
			return heldItem != null && !heldItem.IsAir && heldItem.type == ModContent.ItemType<TheMadRangerItem>();
		}



		////////////////

		public static void CheckPreviousHeldGunItemState( TMRPlayer myplayer, Item prevHeldItem ) {
			var mygun = prevHeldItem?.modItem as TheMadRangerItem;
			if( mygun == null ) {
				return;
			}

			if( myplayer.HasAttemptedShotSinceEquip ) {
				myplayer.HasAttemptedShotSinceEquip = false;
				myplayer.GunHandling.BeginHolster( myplayer.player, mygun );
			}

			if( Main.netMode == NetmodeID.MultiplayerClient && myplayer.player.whoAmI == Main.myPlayer ) {
				GunAnimationProtocol.Broadcast( GunAnimationType.Holster );
			}
		}

		public static void CheckCurrentHeldGunItemState( TMRPlayer myplayer, int inventorySlotOfPrevHeldItem ) {
			myplayer.AimMode.UpdateAimState( myplayer.player );

			if( PlayerLogic.IsHoldingGun(myplayer.player) ) {
				myplayer.GunHandling.UpdateEquipped( myplayer.player );

				Item prevItem = null;
				if( inventorySlotOfPrevHeldItem != -1 ) {
					prevItem = myplayer.player.inventory[ inventorySlotOfPrevHeldItem ];
				}

				myplayer.AimMode.UpdateEquippedAimState( myplayer.player, prevItem );
			} else {
				myplayer.GunHandling.UpdateUnequipped( myplayer.player );
				myplayer.AimMode.CheckUnequippedAimState();
			}

			myplayer.GunHandling.UpdateHolsterAnimation( myplayer.player );
		}
	}
}
