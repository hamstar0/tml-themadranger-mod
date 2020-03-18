using System;
using Microsoft.Xna.Framework;
using Terraria;
using HamstarHelpers.Helpers.Debug;
using TheMadRanger.Items.Weapons;


namespace TheMadRanger {
	partial class GunAnimation {
		public float GetAddedRotationDegrees( Player plr ) {
			if( this.IsReloading ) {
				var myitem = plr.HeldItem.modItem as TheMadRangerItem;
				return myitem?.IsCylinderOpen == true
					? 270
					: 90;
			}

			int recoilDeg = this.RecoilDuration <= 15
				? this.RecoilDuration
				: 0;

			float degrees = this.HolsterTwirlAddedRotationDegrees
				+ this.MiscAddedRotationDegrees
				+ recoilDeg;

			return degrees % 360;
		}

		public float GetAddedRotationRadians( Player plr ) {
			return MathHelper.ToRadians( this.GetAddedRotationDegrees(plr) );
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

			if( this.ReloadDuration > 0 ) {
				if( this.ReloadDuration > 1 ) {
					this.ReloadDuration--;
				} else {
					if( TMRPlayer.AttemptGunReloadRound(plr) ) {
						this.ReloadDuration = TMRConfig.Instance.ReloadRoundTickDuration;
					} else {
						TMRPlayer.AttemptGunReloadEnd( plr );
					}
				}
			}

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
			}

			if( this.MiscAddedRotationDegrees != 0f ) {
				this.MiscAddedRotationDegrees = 0f;
			}
		}
	}
}
