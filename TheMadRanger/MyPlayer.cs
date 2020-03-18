using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using HamstarHelpers.Helpers.Debug;
using TheMadRanger.Items.Weapons;
using TheMadRanger.Items.Accessories;


namespace TheMadRanger {
	partial class TMRPlayer : ModPlayer {
		private int LastSlot = -1;


		////////////////

		public GunAnimation GunAnim { get; } = new GunAnimation();
		public PlayerAimMode AimMode { get; } = new PlayerAimMode();

		////////////////

		public override bool CloneNewInstances => false;



		////////////////

		public override void PreUpdate() {
			if( this.LastSlot != this.player.selectedItem ) {
				if( this.LastSlot != -1 ) {
					this.CheckPreviousHeldItemState( this.player.inventory[this.LastSlot] );
				}
				this.LastSlot = this.player.selectedItem;
			}

			this.CheckCurrentHeldItemState();

			this.GunAnim.Update( this.player );
		}


		private void CheckPreviousHeldItemState( Item prevHeldItem ) {
			if( prevHeldItem != null && !prevHeldItem.IsAir && prevHeldItem.type == ModContent.ItemType<TheMadRangerItem>() ) {
				this.GunAnim.BeginHolster( this.player );
			}
		}

		private void CheckCurrentHeldItemState() {
			if( TMRPlayer.IsHoldingGun(this.player) ) {
				this.GunAnim.UpdateEquipped( this.player );
				this.AimMode.CheckEquippedAimState( this.player );
			} else {
				this.GunAnim.UpdateUnequipped( this.player );
				this.AimMode.CheckUnequippedAimState();
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


		////////////////

		public override void ModifyDrawLayers( List<PlayerLayer> layers ) {
			if( TMRPlayer.IsHoldingGun(this.player) ) {
				(bool isAimWithinArc, int aimDir) aim = this.ApplyGunAim();

				if( !this.GunAnim.IsAnimating ) {
					if( aim.aimDir == this.player.direction || this.GunAnim.RecoilDuration == 0 ) {
						if( this.ModifyDrawLayersForGun( layers, true ) ) {
							this.ModifyDrawLayerForTorsoWithGun( layers, true );
						}

						this.player.headPosition.Y += 1;
					}
				}
			}

			this.GunAnim.ModifyDrawLayers( this.player, layers );
		}
	}
}
