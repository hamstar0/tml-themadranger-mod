using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.GameInput;
using HamstarHelpers.Helpers.Debug;
using TheMadRanger.Items.Weapons;
using TheMadRanger.Items.Accessories;
using TheMadRanger.NetProtocols;


namespace TheMadRanger {
	partial class TMRPlayer : ModPlayer {
		private int InventorySlotOfPreviousHeldItem = -1;


		////////////////

		public GunHandling GunHandling { get; } = new GunHandling();
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

			this.GunHandling.Update( this.player );

			if( this.InventorySlotOfPreviousHeldItem != this.player.selectedItem ) {
				this.InventorySlotOfPreviousHeldItem = this.player.selectedItem;
			}
		}


		////

		private void CheckPreviousHeldItemState( Item prevHeldItem ) {
			if( prevHeldItem != null && !prevHeldItem.IsAir && prevHeldItem.type == ModContent.ItemType<TheMadRangerItem>() ) {
				this.GunHandling.BeginHolster( this.player );

				if( Main.netMode == 1 && this.player.whoAmI == Main.myPlayer ) {
					GunAnimationProtocol.Broadcast( GunAnimationType.Holster );
				}
			}
		}

		private void CheckCurrentHeldItemState() {
			this.AimMode.CheckAimState( this.player );

			if( TMRPlayer.IsHoldingGun(this.player) ) {
				this.GunHandling.UpdateEquipped( this.player );

				Item prevItem = null;
				if( this.InventorySlotOfPreviousHeldItem != -1 ) {
					prevItem = this.player.inventory[ this.InventorySlotOfPreviousHeldItem ];
				}

				this.AimMode.CheckEquippedAimState( this.player, prevItem );
			} else {
				this.GunHandling.UpdateUnequipped( this.player );
				this.AimMode.CheckUnequippedAimState();
			}
		}


		////////////////

		public override bool PreItemCheck() {
			return !this.GunHandling.IsAnimating;
		}

		////////////////

		public override void ProcessTriggers( TriggersSet triggersSet ) {
			if( TMRMod.Instance.ReloadKey.JustPressed ) {
				this.GunHandling.BeginReload( this.player );

				if( Main.netMode == 1 && this.player.whoAmI == Main.myPlayer ) {
					GunAnimationProtocol.Broadcast( GunAnimationType.Reload );
				}
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
