using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameInput;
using Terraria.ModLoader.IO;
using ModLibsCore.Libraries.Debug;
using TheMadRanger.NetProtocols;
using TheMadRanger.Logic;
using TheMadRanger.Items.Weapons;
using TheMadRanger.Items.Accessories;


namespace TheMadRanger {
	partial class TMRPlayer : ModPlayer {
		private int InventorySlotOfPreviousHeldItem = -1;


		////////////////

		public GunHandling GunHandling { get; } = new GunHandling();

		public PlayerAimMode AimMode { get; } = new PlayerAimMode();

		////

		public bool HasAttemptedShotSinceEquip { get; internal set; } = false;

		public Vector2 AmmoDisplayOffset { get; internal set; } = default;


		////////////////

		public override bool CloneNewInstances => false;



		////////////////

		public override void Initialize() {
			this.AmmoDisplayOffset = default;
		}


		////////////////

		public override void Load( TagCompound tag ) {
			this.AmmoDisplayOffset = default;

			if( tag.ContainsKey("ammo_display_x") ) {
				this.AmmoDisplayOffset = new Vector2(
					tag.GetInt( "ammo_display_x" ),
					tag.GetInt( "ammo_display_y" )
				);
			}
		}

		public override TagCompound Save() {
			return new TagCompound {
				{ "ammo_display_x", (int)this.AmmoDisplayOffset.X },
				{ "ammo_display_y", (int)this.AmmoDisplayOffset.Y }
			};
		}


		////////////////

		public override void PreUpdate() {
			if( !Main.gamePaused && !this.player.dead ) {
				this.PreUpdateActive();
			}
		}

		private void PreUpdateActive() {
			if( this.InventorySlotOfPreviousHeldItem != this.player.selectedItem ) {
				if( this.InventorySlotOfPreviousHeldItem != -1 ) {
					Item prevItem = this.player.inventory[ this.InventorySlotOfPreviousHeldItem ];
					PlayerLogic.UpdatePreviousHeldGunItemState( this, prevItem );
				}
			}

			if( PlayerLogic.UpdateCurrentHeldGunItemState(this, this.InventorySlotOfPreviousHeldItem) ) {
				PlayerLogic.UpdatePlayerStateForAimMode( this );
			}

			if( this.InventorySlotOfPreviousHeldItem != this.player.selectedItem ) {
				this.InventorySlotOfPreviousHeldItem = this.player.selectedItem;
			}
		}

		////

		public override void UpdateDead() {
			this.GunHandling.UpdateUnequipped( this.player );
			this.AimMode.UpdateUnequippedAimState();
		}

		////////////////

		public override bool PreItemCheck() {
			return true;
			//return !this.GunHandling.IsAnimating;
		}


		////////////////

		public override void ProcessTriggers( TriggersSet triggersSet ) {
			void handleReload() {
				Item gun = this.player.HeldItem;
				if( gun?.active != true ) {
					return;
				}

				var mygun = gun.modItem as TheMadRangerItem;
				if( mygun == null ) {
					return;
				}

				if( this.GunHandling.BeginReload(this.player, mygun) ) {
					if( Main.netMode == NetmodeID.MultiplayerClient && this.player.whoAmI == Main.myPlayer ) {
						GunAnimationPacket.Broadcast( GunAnimationType.Reload );
					}
				}
			}

			//

			if( TMRMod.Instance.ReloadKey.JustPressed ) {
				if( !Main.gamePaused && !this.player.dead ) {
					handleReload();
				}
			}
		}


		////////////////

		public override void SetupStartInventory( IList<Item> items, bool mediumcoreDeath ) {
			var config = TMRConfig.Instance;

			if( !mediumcoreDeath ) {
				if( config.Get<bool>( nameof(config.PlayerSpawnsWithGun) ) ) {
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

		////

		public override void OnRespawn( Player player ) {
			if( PlayerLogic.IsHoldingGun(this.player) ) {
				((TheMadRangerItem)player.HeldItem.modItem).InsertAllOnRespawn( player );
			}
		}
	}
}
