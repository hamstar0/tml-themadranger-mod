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

		internal GunAnimation GunAnim { get; } = new GunAnimation();

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
				this.CheckAimState();
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

			if( this.GunAnim.IsHolstering ) {
				return false;
			}

			float shakeAddedRads = BigIronPlayer.GetAimShakeAddedRadians();

			Vector2 randSpeed = new Vector2( speedX, speedY )
				.RotatedBy(shakeAddedRads);
			speedX = randSpeed.X;
			speedY = randSpeed.Y;

			this.GunAnim.BeginRecoil( MathHelper.ToDegrees(shakeAddedRads) * -player.direction );
			
			damage = this.GetAimStateShakeDamage( damage );

			return true;
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

				if( (aim.aimDir == this.player.direction || this.GunAnim.RecoilDuration == 0) && !this.GunAnim.IsHolstering ) {
					if( this.ModifyDrawLayersForGun(layers, true) ) {
						this.ModifyDrawLayerForTorsoWithGun( layers, true );
					}

					this.player.headPosition.Y += 1;
				}
			}

			this.GunAnim.ModifyDrawLayers( this.player, layers );
		}
	}
}
