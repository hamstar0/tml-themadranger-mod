using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModLibsCore.Libraries.Debug;
using TheMadRanger.Items.Weapons;
using TheMadRanger.NetProtocols;


namespace TheMadRanger.Logic {
	partial class PlayerLogic {
		public static bool IsHoldingGun( Player player ) {
			Item heldItem = player.HeldItem;

			return heldItem?.active == true
				&& heldItem.type == ModContent.ItemType<TheMadRangerItem>();
		}

		public static bool IsUsingHeldGun( Player player ) {
			if( !PlayerLogic.IsHoldingGun(player) ) {
				return false;
			}

			var myplayer = player.GetModPlayer<TMRPlayer>();
			return myplayer.GunHandling.IsAnimating || myplayer.AimMode.IsModeActivating;
		}



		////////////////

		public static void UpdatePreviousHeldGunItemState_If( TMRPlayer myplayer, Item prevHeldItem ) {
			var mygun = prevHeldItem?.modItem as TheMadRangerItem;
			if( mygun == null ) {
				return;
			}

			//

			if( myplayer.HasAttemptedShotSinceEquip ) {
				myplayer.HasAttemptedShotSinceEquip = false;

				myplayer.GunHandling.BeginHolster( myplayer.player, mygun );
			}

			//

			if( Main.netMode == NetmodeID.MultiplayerClient && myplayer.player.whoAmI == Main.myPlayer ) {
				GunAnimationPacket.Broadcast( GunAnimationType.Holster );
			}
		}

		public static bool UpdateCurrentHeldGunItemState( TMRPlayer myplayer, int inventorySlotOfPrevHeldItem ) {
			bool canHoldGun = PlayerLogic.IsHoldingGun( myplayer.player );

			//

			myplayer.AimMode.UpdateAimState( myplayer.player );

			//

			if( canHoldGun ) {
				myplayer.GunHandling.UpdateEquipped( myplayer.player );

				//

				Item prevItem = null;
				if( inventorySlotOfPrevHeldItem != -1 ) {
					prevItem = myplayer.player.inventory[ inventorySlotOfPrevHeldItem ];
				}

				myplayer.AimMode.UpdateEquippedAimState( myplayer.player, prevItem );
			} else {
				myplayer.GunHandling.UpdateUnequipped( myplayer.player );

				myplayer.AimMode.UpdateUnequippedAimState();
			}

			//

			myplayer.GunHandling.UpdateHolsterAnimation( myplayer.player );

			//

			return canHoldGun;
		}
	}
}
