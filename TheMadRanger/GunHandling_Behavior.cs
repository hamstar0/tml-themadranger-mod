using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.DotNET.Reflection;
using TheMadRanger.Gores;
using TheMadRanger.Items.Weapons;


namespace TheMadRanger {
	partial class GunHandling {
		public float GetAddedRotationDegrees( Player plr ) {
			float degrees;

			if( this.IsReloading ) {
				if( plr.direction > 0 ) {
					degrees = this.ReloadingRounds ? 90f : 270f;
				} else {
					degrees = this.ReloadingRounds ? 270f : 90f;
				}
			} else {
				int recoilDeg = this.RecoilDuration <= 15
					? this.RecoilDuration
					: 0;

				degrees = this.HolsterTwirlAddedRotationDegrees
					+ this.MiscAddedRotationDegrees
					+ recoilDeg;
			}

			return degrees % 360;
		}

		public Vector2 GetAddedPositionOffset( Player plr ) {
			if( this.IsReloading && this.ReloadingRounds ) {
				return new Vector2( 0f, 16f );
			}

			return default( Vector2 );
		}

		public float GetAddedRotationRadians( Player plr ) {
			return MathHelper.ToRadians( this.GetAddedRotationDegrees( plr ) );
		}


		////////////////

		public void UpdateHolsterAnimation( Player plr ) {
			if( this.HolsterDuration > 0 ) {
				if( !Main.gamePaused && !plr.dead ) {
					this.HolsterDuration--;
				}

				if( plr.direction > 0 ) {
					this.HolsterTwirlAddedRotationDegrees -= 32f;
					if( this.HolsterTwirlAddedRotationDegrees < 0f ) {
						this.HolsterTwirlAddedRotationDegrees += 360f;
					}
				} else {
					this.HolsterTwirlAddedRotationDegrees += 32f;
					if( this.HolsterTwirlAddedRotationDegrees >= 360f ) {
						this.HolsterTwirlAddedRotationDegrees -= 360f;
					}
				}
			} else {
				this.HolsterTwirlAddedRotationDegrees = 0f;
			}
		}

		////

		public void UpdateEquipped( Player plr ) {
			if( this.RecoilDuration > 0 ) {
				this.RecoilDuration--;
			}

			this.UpdateReloadingSequence( plr );

			if( this.MiscAddedRotationDegrees != 0f ) {
				this.MiscAddedRotationDegrees -= Math.Sign( this.MiscAddedRotationDegrees ) / 3f;

				if( Math.Abs( this.MiscAddedRotationDegrees ) < 1f ) {
					this.MiscAddedRotationDegrees = 0f;
				}
			}
		}

		public void UpdateUnequipped( Player plr ) {
			if( this.RecoilDuration > 0 ) {
				this.RecoilDuration = 0;
			}

			if( this.ReloadDuration > 0 ) {
				this.ReloadDuration = 0;
				this.ReloadingRounds = false;
			}

			if( this.MiscAddedRotationDegrees != 0f ) {
				this.MiscAddedRotationDegrees = 0f;
			}
		}


		////////////////

		private void UpdateReloadingSequence( Player plr ) {
			// Not reloading
			if( this.ReloadDuration == 0 ) {
				return;
			}

			// Reloading timer
			if( this.ReloadDuration > 1 ) {
				this.ReloadDuration--;
				return;
			}

			// No item to reload
			var myitem = plr.HeldItem.modItem as TheMadRangerItem;
			if( myitem == null ) {
				return;
			}

			// Not yet loading rounds
			if( !this.ReloadingRounds ) {
				// Start loading rounds, if cylinder empty
				if( !myitem.IsCylinderEmpty() ) {
					(int Shells, int Rounds) unloadings = myitem.UnloadCylinder( plr );
					this.ProcessUnloadedGunRounds( plr, unloadings.Shells, unloadings.Rounds );
				}
				this.ReloadDuration = TMRConfig.Instance.ReloadRoundTickDuration;
				this.ReloadingRounds = true;
				return;
			}

			// No ammo source; stop reloading
			if( !TheMadRangerItem.IsAmmoSourceAvailable(plr, false, out string result) ) {
				Main.NewText( result, Color.Yellow );
				this.StopReloading( plr );
				return;
			}

			// Reload rounds until not possible
			if( myitem.InsertSpeedloader(plr) || myitem.InsertRound(plr) ) {
				this.ReloadDuration = TMRConfig.Instance.ReloadRoundTickDuration;
				return;
			}
			
			this.StopReloading( plr );
			return;
		}


		////////////////

		private void ProcessUnloadedGunRounds( Player plr, int shells, int rounds ) {
			if( shells > 0 ) {
				this.ProcessUnloadedShells( plr, shells );
			}

			// TODO: Return rounds to Bandolier
		}

		private void ProcessUnloadedShells( Player plr, int shells ) {
			int shellGoreSlot = TMRMod.Instance.GetGoreSlot( "Gores/ShellCasing" );

			Vector2 itemScrPos;
			ReflectionHelpers.RunMethod(
				Main.instance,
				"DrawPlayerItemPos",
				new object[] { plr.gravDir, plr.HeldItem.type },
				out itemScrPos
			);

			Texture2D itemTex = Main.itemTexture[plr.HeldItem.type];
			var itemTexOffset = new Vector2( itemTex.Width / 2, itemScrPos.Y );

			Vector2 itemWldPos = plr.itemLocation + itemTexOffset;

			for( int i = 0; i < shells; i++ ) {
				Gore.NewGore( itemWldPos, ShellCasing.GetVelocity(), shellGoreSlot, ShellCasing.GetScale() );
			}
		}
	}
}
