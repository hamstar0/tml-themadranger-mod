using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using HamstarHelpers.Helpers.Debug;


namespace BigIron {
	partial class GunAnimation {
		private Rectangle BodyFrameShifted;


		////////////////

		public int Recoil { get; private set; } = 0;

		public int HolsterDuration { get; private set; } = 0;

		public int HolsterDurationMax { get; private set; } = 0;

		public float AddedRotationDegrees { get; private set; } = 0f;


		////

		public bool IsHolstering => this.HolsterDuration > 0;

		public float AddedRotationRadians => MathHelper.ToRadians( this.AddedRotationDegrees );

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

		public void Update() {
			if( this.HolsterDuration > 0 ) {
				this.HolsterDuration--;

				this.AddedRotationDegrees += 32f;
				if( this.AddedRotationDegrees > 360f ) {
					this.AddedRotationDegrees -= 360f;
				}
			} else {
				this.AddedRotationDegrees = 0f;
			}

			if( this.Recoil > 0 ) {
				this.Recoil--;
			}
		}

		////////////////

		public void BeginRecoil() {
			this.Recoil = 17;
		}

		public void BeginHolster() {
			this.HolsterDuration = 60;
			this.HolsterDurationMax = 60;
		}
	}
}
