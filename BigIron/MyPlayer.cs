using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using HamstarHelpers.Helpers.Debug;
using BigIron.Items.Weapons;
using BigIron.Items.Accessories;


namespace BigIron {
	partial class BigIronPlayer : ModPlayer {
		private int LastSlot = -1;


		////////////////

		public GunAnimation GunAnim { get; } = new GunAnimation();

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
			if( prevHeldItem != null && !prevHeldItem.IsAir && prevHeldItem.type == ModContent.ItemType<BigIronItem>() ) {
				this.GunAnim.BeginHolster( this.player );
			}
		}

		private void CheckCurrentHeldItemState() {
			if( BigIronPlayer.IsHoldingGun(this.player) ) {
				this.GunAnim.UpdateEquipped( this.player );
				this.CheckEquippedAimState();
			} else {
				this.GunAnim.UpdateUnequipped( this.player );
				this.CheckUnequippedAimState();
			}
		}


		////////////////

		public override bool Shoot(
					Item item,
					ref Vector2 position,
					ref float speedX,
					ref float speedY,
					ref int type,
					ref int damage,
					ref float knockBack ) {
			if( !BigIronPlayer.IsHoldingGun(this.player) ) {
				return true;
			}

			return this.ShootGun( item, ref speedX, ref speedY, ref damage, ref knockBack );
		}


		////////////////

		public override void SetupStartInventory( IList<Item> items, bool mediumcoreDeath ) {
			if( !mediumcoreDeath ) {
				if( BigIronConfig.Instance.PlayerSpawnsWithGun ) {
					var revolver = new Item();
					revolver.SetDefaults( ModContent.ItemType<BigIronItem>() );

					items.Add( revolver );
				}
				if( BigIronConfig.Instance.PlayerSpawnsWithBandolier ) {
					var bandolier = new Item();
					bandolier.SetDefaults( ModContent.ItemType<BandolierItem>() );

					items.Add( bandolier );
				}
			}
		}


		////////////////
		
		public override void ModifyDrawLayers( List<PlayerLayer> layers ) {
			if( BigIronPlayer.IsHoldingGun(this.player) ) {
				(bool isAimWithinArc, int aimDir) aim = this.AimGun();

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
