using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.Audio;
using HamstarHelpers.Helpers.Debug;


namespace TheMadRanger {
	partial class GunAnimation {
		private Rectangle BodyFrameShifted;


		////////////////

		public int RecoilDuration { get; private set; } = 0;

		////

		public int HolsterDuration { get; private set; } = 0;

		////

		public int ReloadDuration { get; private set; } = 0;



		////

		public bool IsHolstering => this.HolsterDuration > 0;

		public bool IsReloading => this.ReloadDuration > 0;

		public bool IsAnimating => this.IsHolstering || this.IsReloading;


		////

		public float HolsterTwirlAddedRotationDegrees { get; private set; } = 0f;

		public float MiscAddedRotationDegrees { get; private set; } = 0f;


		////

		public SoundStyle TwirlSound { get; private set; } = null;


		////

		public PlayerLayer GunDrawLayer { get; }
		public PlayerLayer ArmsShiftLayer { get; }
		public PlayerLayer ArmsUnshiftLayer { get; }
		public PlayerLayer HandShiftLayer { get; }
		public PlayerLayer HandUnshiftLayer { get; }
		public PlayerLayer BodyShiftLayer { get; }
		public PlayerLayer BodyUnshiftLayer { get; }
		public PlayerLayer SkinShiftLayer { get; }
		public PlayerLayer SkinUnshiftLayer { get; }



		////////////////
		
		public GunAnimation() {
			this.GunDrawLayer = new PlayerLayer( "BigIron", "Custom Gun Animation", (plrDrawInfo) => {
				Main.playerDrawData.Add( this.DrawGun(plrDrawInfo) );
			} );

			Rectangle unshiftedBodyFrame = default(Rectangle);

			Action<PlayerDrawInfo> shiftAction = ( plrDrawInfo ) => {
				unshiftedBodyFrame = plrDrawInfo.drawPlayer.bodyFrame;
				plrDrawInfo.drawPlayer.bodyFrame = this.BodyFrameShifted;
			};
			Action<PlayerDrawInfo> unshiftAction = ( plrDrawInfo ) => {
				plrDrawInfo.drawPlayer.bodyFrame = unshiftedBodyFrame;
			};
			
			this.ArmsShiftLayer = new PlayerLayer( "BigIron", "Gun Holster Arms Shift Reframe", shiftAction );
			this.ArmsUnshiftLayer = new PlayerLayer( "BigIron", "Gun Holster Arms Unshift Reframe", unshiftAction );
			this.HandShiftLayer = new PlayerLayer( "BigIron", "Gun Holster Arm Shift Reframe", shiftAction );
			this.HandUnshiftLayer = new PlayerLayer( "BigIron", "Gun Holster Arm Unshift Reframe", unshiftAction );
			this.BodyShiftLayer = new PlayerLayer( "BigIron", "Gun Holster Torso Shift Reframe", shiftAction );
			this.BodyUnshiftLayer = new PlayerLayer( "BigIron", "Gun Holster Torso Unshift Reframe", unshiftAction );
			this.SkinShiftLayer = new PlayerLayer( "BigIron", "Gun Holster Torso Skin Shift Reframe", shiftAction );
			this.SkinUnshiftLayer = new PlayerLayer( "BigIron", "Gun Holster Torso Skin Unshift Reframe", unshiftAction );
		}


		////////////////

		public float GetAddedRotationDegrees() {
			int recoilDeg = this.RecoilDuration <= 15
				? this.RecoilDuration
				: 0;

			float degrees = this.HolsterTwirlAddedRotationDegrees
				+ this.MiscAddedRotationDegrees
				+ recoilDeg;
			return degrees % 360;
		}

		public float GetAddedRotationRadians() {
			return MathHelper.ToRadians( this.GetAddedRotationDegrees() );
		}


		////////////////

		public void Update( Player player ) {
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
						this.ReloadDuration = BigIronConfig.Instance.ReloadRoundTickDuration;
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


		////////////////

		public void BeginRecoil( float addedRotationDegrees ) {
			this.MiscAddedRotationDegrees = addedRotationDegrees;
			this.RecoilDuration = 17;
		}

		public void BeginReload( Player plr ) {
			if( TMRPlayer.AttemptGunReloadBegin(plr) ) {
				this.ReloadDuration = BigIronConfig.Instance.ReloadInitTickDuration;
			}
		}

		public void BeginHolster( Player plr ) {
			this.HolsterDuration = BigIronConfig.Instance.HolsterTwirlTickDuration;
			if( this.HolsterDuration == 0 ) {
				return;
			}

			if( this.TwirlSound == null ) {
				this.TwirlSound = TMRMod.Instance.GetLegacySoundSlot(
					Terraria.ModLoader.SoundType.Custom,
					"Sounds/Custom/RevolverTwirl"
				).WithVolume( 0.65f );
			}

			Main.PlaySound( (LegacySoundStyle)this.TwirlSound, plr.Center );
		}
	}
}
