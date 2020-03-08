using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using HamstarHelpers.Helpers.Debug;
using BigIron.Items.Weapons;


namespace BigIron {
	partial class BigIronPlayer : ModPlayer {
		private int Recoil = 0;
		private bool IsFacingWrongWay = false;
		private GunAnimation GunAnim = new GunAnimation();

		private int LastSlot = -1;


		////////////////

		public override bool CloneNewInstances => false;



		////////////////

		public override void PreUpdate() {
			if( this.LastSlot != this.player.selectedItem ) {
				this.CheckHeldItemState();
			}

			if( this.Recoil > 0 ) {
				this.Recoil--;
			}

			this.GunAnim.Update();
		}


		private void CheckHeldItemState() {
			if( this.LastSlot != -1 ) {
				Item prevHeldItem = this.player.inventory[this.LastSlot];

				if( !prevHeldItem.IsAir && prevHeldItem.type == ModContent.ItemType<BigIronItem>() ) {
					this.GunAnim.BeginHolster();
				}
			}

			this.LastSlot = this.player.selectedItem;
		}


		////////////////

		public override bool Shoot( Item item, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack ) {
			if( BigIronPlayer.IsHoldingGun(this.player) ) {
				if( this.GunAnim.HolsterDuration > 0 ) {
					return false;
				}

				this.Recoil = 17;
			}

			return base.Shoot( item, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack );
		}


		////////////////
		
		public override void ModifyDrawLayers( List<PlayerLayer> layers ) {
			if( BigIronPlayer.IsHoldingGun(this.player) ) {
				this.AimGun();

				if( !this.IsFacingWrongWay || this.Recoil == 0 ) {
					if( this.ModifyDrawLayersForGun(layers) ) {
						this.ModifyDrawLayerForTorsoWithGun( layers );
					}

//					layers.Add( layer, this.GunAnim.DrawLayer );
					
					this.player.headPosition.Y += 1;
				}
			}
		}
	}
}
