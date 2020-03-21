using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.GameInput;
using HamstarHelpers.Helpers.Debug;
using TheMadRanger.Items.Weapons;
using TheMadRanger.Items.Accessories;


namespace TheMadRanger {
	partial class TMRPlayer : ModPlayer {
		private int InventorySlotOfPreviousHeldItem = -1;


		////////////////

		public GunAnimation GunAnim { get; } = new GunAnimation();
		public PlayerAimMode AimMode { get; } = new PlayerAimMode();

		////////////////

		public override bool CloneNewInstances => false;



		////////////////

		public override void PreUpdate() {
			if( this.InventorySlotOfPreviousHeldItem != this.player.selectedItem ) {
				if( this.InventorySlotOfPreviousHeldItem != -1 ) {
					this.CheckPreviousHeldItemState( this.player.inventory[this.InventorySlotOfPreviousHeldItem] );
				}
			}

			this.CheckCurrentHeldItemState();

			this.GunAnim.Update( this.player );

			if( this.InventorySlotOfPreviousHeldItem != this.player.selectedItem ) {
				this.InventorySlotOfPreviousHeldItem = this.player.selectedItem;
			}
		}


		private void CheckPreviousHeldItemState( Item prevHeldItem ) {
			if( prevHeldItem != null && !prevHeldItem.IsAir && prevHeldItem.type == ModContent.ItemType<TheMadRangerItem>() ) {
				this.GunAnim.BeginHolster( this.player );
			}
		}

		private void CheckCurrentHeldItemState() {
			this.AimMode.CheckAimState( this.player );

			if( TMRPlayer.IsHoldingGun(this.player) ) {
				this.GunAnim.UpdateEquipped( this.player );

				Item prevItem = null;
				if( this.InventorySlotOfPreviousHeldItem != -1 ) {
					prevItem = this.player.inventory[ this.InventorySlotOfPreviousHeldItem ];
				}

				this.AimMode.CheckEquippedAimState( this.player, prevItem );
			} else {
				this.GunAnim.UpdateUnequipped( this.player );
				this.AimMode.CheckUnequippedAimState();
			}
		}


		////////////////

		public override void ProcessTriggers( TriggersSet triggersSet ) {
			if( TMRMod.Instance.ReloadKey.JustPressed ) {
				this.GunAnim.BeginReload( this.player );
			}
		}


		////////////////

		public override void SetupStartInventory( IList<Item> items, bool mediumcoreDeath ) {
			if( !mediumcoreDeath ) {
				if( TMRConfig.Instance.PlayerSpawnsWithGun ) {
					var revolver = new Item();
					revolver.SetDefaults( ModContent.ItemType<TheMadRangerItem>() );

					items.Add( revolver );
				}
				if( TMRConfig.Instance.PlayerSpawnsWithBandolier ) {
					var bandolier = new Item();
					bandolier.SetDefaults( ModContent.ItemType<BandolierItem>() );

					items.Add( bandolier );
				}
			}
		}
	}
}
