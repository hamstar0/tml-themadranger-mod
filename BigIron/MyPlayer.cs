using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using HamstarHelpers.Helpers.Debug;
using BigIron.Items.Weapons;


namespace BigIron {
	partial class BigIronPlayer : ModPlayer {
		private bool IsFacingWrongWay = false;
		private int LastSlot = -1;

		private GunAnimation GunAnim = new GunAnimation();


		////////////////

		public override bool CloneNewInstances => false;



		////////////////

		public override void PreUpdate() {
			if( this.LastSlot != this.player.selectedItem ) {
				if( this.LastSlot != -1 ) {
					this.CheckHeldItemState( this.player.inventory[this.LastSlot] );
				}
				this.LastSlot = this.player.selectedItem;
			}

			this.GunAnim.Update();
		}


		private void CheckHeldItemState( Item prevHeldItem ) {
			if( !(prevHeldItem?.IsAir == true) && prevHeldItem.type == ModContent.ItemType<BigIronItem>() ) {
				this.GunAnim.BeginHolster();
			}
		}


		////////////////

		public override bool Shoot( Item item, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack ) {
			if( BigIronPlayer.IsHoldingGun(this.player) ) {
				if( this.GunAnim.HolsterDuration > 0 ) {
					return false;
				}

				this.GunAnim.BeginRecoil();
			}

			return base.Shoot( item, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack );
		}


		////////////////
		
		public override void ModifyDrawLayers( List<PlayerLayer> layers ) {
			if( BigIronPlayer.IsHoldingGun( this.player ) ) {
				this.AimGun();

				if( (!this.IsFacingWrongWay || this.GunAnim.Recoil == 0) && !this.GunAnim.IsAnimating ) {
					if( this.ModifyDrawLayersForGun( layers ) ) {
						this.ModifyDrawLayerForTorsoWithGun( layers );
					}

					this.player.headPosition.Y += 1;
				}
			}

			this.GunAnim.ModifyDrawLayers( this.player, layers );
		}
	}
}
