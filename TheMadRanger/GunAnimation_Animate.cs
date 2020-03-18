using System;
using Microsoft.Xna.Framework;
using Terraria;
using HamstarHelpers.Helpers.Debug;
using TheMadRanger.Items.Weapons;


namespace TheMadRanger {
	partial class GunAnimation {
		public float GetAddedRotationDegrees( Player plr ) {
			float degrees;

			if( this.IsReloading ) {
				var myitem = plr.HeldItem.modItem as TheMadRangerItem;

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

		public float GetAddedRotationRadians( Player plr ) {
			return MathHelper.ToRadians( this.GetAddedRotationDegrees( plr ) );
		}


		////////////////

		public void UpdateHolsterAnimation( Player player ) {
			if( this.HolsterDuration > 0 ) {
				this.HolsterDuration--;

				if( player.direction > 0 ) {
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
			if( this.ReloadDuration == 0 ) {
				return;
			}

			if( this.ReloadDuration > 1 ) {
				this.ReloadDuration--;
				return;
			}

			var myitem = (TheMadRangerItem)plr.HeldItem.modItem;
			if( myitem == null ) {
				return;
			}

			if( !this.ReloadingRounds ) {
				if( !myitem.IsCylinderEmpty() ) {
					myitem.UnloadCylinder( plr );   // TODO: Recycle bullets
					this.ReloadDuration = TMRConfig.Instance.ReloadRoundTickDuration;
					this.ReloadingRounds = true;
					return;
				}
			}

			if( myitem.InsertRound( plr ) ) {
				this.ReloadDuration = TMRConfig.Instance.ReloadRoundTickDuration;
				return;
			} else {
				myitem.CloseCylinder( plr );
				this.ReloadDuration = 0;
				this.ReloadingRounds = false;
			}
		}
	}
}
