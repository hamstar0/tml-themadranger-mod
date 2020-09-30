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

		////

		public bool HasAttemptedShotSinceEquip { get; internal set; } = false;

		////////////////

		public override bool CloneNewInstances => false;



		////////////////

		public override void PreUpdate() {
			if( !Main.gamePaused && !this.player.dead ) {
				this.PreUpdateActive();
			}
		}

		private void PreUpdateActive() {
			if( this.InventorySlotOfPreviousHeldItem != this.player.selectedItem ) {
				if( this.InventorySlotOfPreviousHeldItem != -1 ) {
					this.CheckPreviousHeldItemState( this.player.inventory[this.InventorySlotOfPreviousHeldItem] );
				}
			}

			this.CheckCurrentHeldItemState();

			this.GunHandling.UpdateHolsterAnimation( this.player );

			if( this.InventorySlotOfPreviousHeldItem != this.player.selectedItem ) {
				this.InventorySlotOfPreviousHeldItem = this.player.selectedItem;
			}

			if( this.AimMode.IsLocked ) {
				var config = TMRConfig.Instance;
				float aimLockMoveSpeed = config.Get<float>( nameof(TMRConfig.AimModeLockMoveSpeedScale) );

				this.player.maxRunSpeed *= aimLockMoveSpeed;
				this.player.accRunSpeed = player.maxRunSpeed;
				this.player.moveSpeed *= aimLockMoveSpeed;
			}
		}

		////

		public override void UpdateDead() {
			this.GunHandling.UpdateUnequipped( this.player );
			this.AimMode.CheckUnequippedAimState();
		}


		////////////////

		private void CheckPreviousHeldItemState( Item prevHeldItem ) {
			if( prevHeldItem != null && !prevHeldItem.IsAir && prevHeldItem.type == ModContent.ItemType<TheMadRangerItem>() ) {
				if( this.HasAttemptedShotSinceEquip ) {
					this.HasAttemptedShotSinceEquip = false;
					this.GunHandling.BeginHolster( this.player );
				}

				if( Main.netMode == 1 && this.player.whoAmI == Main.myPlayer ) {
					GunAnimationProtocol.Broadcast( GunAnimationType.Holster );
				}
			}
		}

		private void CheckCurrentHeldItemState() {
			this.AimMode.UpdateAimState( this.player );

			if( TMRPlayer.IsHoldingGun(this.player) ) {
				this.GunHandling.UpdateEquipped( this.player );

				Item prevItem = null;
				if( this.InventorySlotOfPreviousHeldItem != -1 ) {
					prevItem = this.player.inventory[ this.InventorySlotOfPreviousHeldItem ];
				}

				this.AimMode.UpdateEquippedAimState( this.player, prevItem );
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
				if( !Main.gamePaused && !this.player.dead ) {
					if( this.GunHandling.BeginReload( this.player ) ) {
						if( Main.netMode == 1 && this.player.whoAmI == Main.myPlayer ) {
							GunAnimationProtocol.Broadcast( GunAnimationType.Reload );
						}
					}
				}
			}
		}


		////////////////

		public override void SetupStartInventory( IList<Item> items, bool mediumcoreDeath ) {
			var config = TMRConfig.Instance;

			if( !mediumcoreDeath ) {
				if( config.Get<bool>( nameof(TMRConfig.PlayerSpawnsWithGun) ) ) {
					var revolver = new Item();
					revolver.SetDefaults( ModContent.ItemType<TheMadRangerItem>() );

					items.Add( revolver );
				}

				if( config.Get<bool>( nameof(TMRConfig.PlayerSpawnsWithBandolier) ) ) {
					var bandolier = new Item();
					bandolier.SetDefaults( ModContent.ItemType<BandolierItem>() );

					items.Add( bandolier );
				}
			}
		}
	}
}
