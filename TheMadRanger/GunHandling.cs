﻿using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;
using HamstarHelpers.Helpers.Debug;
using TheMadRanger.Helpers.Misc;
using TheMadRanger.Items.Weapons;


namespace TheMadRanger {
	partial class GunHandling {
		/*public static Vector2 GetGunTipPosition( Player plr ) {
			var myplayer = plr.GetModPlayer<TMRPlayer>();
			Texture2D tex = Main.itemTexture[ ModContent.ItemType<TheMadRangerItem>() ];
			float rot = plr.itemRotation;//+ myplayer.GunAnim.GetAddedRotationRadians(plr);

			var aim = new Vector2( (float)Math.Cos(rot), (float)Math.Sin(rot) )
				* plr.direction;

			return plr.MountedCenter + (aim * tex.Width * TheMadRangerItem.Scale);
		} <- Seems to slide off tip at some angles? */



		////////////////

		private Rectangle BodyFrameShifted;


		////////////////

		public int RecoilDuration { get; private set; } = 0;

		////

		public int HolsterDuration { get; private set; } = 0;

		////

		public int ReloadDuration { get; private set; } = 0;

		public bool ReloadingRounds { get; private set; } = false;



		////

		public bool IsHolstering => this.HolsterDuration > 0;

		public bool IsReloading => this.ReloadDuration > 0;

		public bool IsAnimating => this.IsHolstering || this.IsReloading;


		////

		public float HolsterTwirlAddedRotationDegrees { get; private set; } = 0f;

		public float MiscAddedRotationDegrees { get; private set; } = 0f;


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

		public GunHandling() {
			this.GunDrawLayer = new PlayerLayer( "TheMadRanger", "Custom Gun Animation", (plrDrawInfo) => {
				Main.playerDrawData.Add( this.GetGunDrawData(plrDrawInfo) );

				DrawData? drawData = this.GetReloadDrawData( plrDrawInfo );
				if( drawData.HasValue ) {
					Main.playerDrawData.Add( drawData.Value );
				}
			} );

			var unshiftedBodyFrame = default(Rectangle);

			Action<PlayerDrawInfo> shiftAction = ( plrDrawInfo ) => {
				unshiftedBodyFrame = plrDrawInfo.drawPlayer.bodyFrame;
				plrDrawInfo.drawPlayer.bodyFrame = this.BodyFrameShifted;
			};
			Action<PlayerDrawInfo> unshiftAction = ( plrDrawInfo ) => {
				plrDrawInfo.drawPlayer.bodyFrame = unshiftedBodyFrame;
			};

			this.ArmsShiftLayer = new PlayerLayer( "TheMadRanger", "Gun Holster Arms Shift Reframe", shiftAction );
			this.ArmsUnshiftLayer = new PlayerLayer( "TheMadRanger", "Gun Holster Arms Unshift Reframe", unshiftAction );
			this.HandShiftLayer = new PlayerLayer( "TheMadRanger", "Gun Holster Arm Shift Reframe", shiftAction );
			this.HandUnshiftLayer = new PlayerLayer( "TheMadRanger", "Gun Holster Arm Unshift Reframe", unshiftAction );
			this.BodyShiftLayer = new PlayerLayer( "TheMadRanger", "Gun Holster Torso Shift Reframe", shiftAction );
			this.BodyUnshiftLayer = new PlayerLayer( "TheMadRanger", "Gun Holster Torso Unshift Reframe", unshiftAction );
			this.SkinShiftLayer = new PlayerLayer( "TheMadRanger", "Gun Holster Torso Skin Shift Reframe", shiftAction );
			this.SkinUnshiftLayer = new PlayerLayer( "TheMadRanger", "Gun Holster Torso Skin Unshift Reframe", unshiftAction );
		}


		////////////////

		public void Update( Player player ) {
			this.UpdateHolsterAnimation( player );
		}


		////////////////

		public void BeginRecoil( float addedRotationDegrees ) {
			this.MiscAddedRotationDegrees = addedRotationDegrees;
			this.RecoilDuration = 17;
		}

		public bool BeginReload( Player plr ) {
			if( this.IsReloading ) { return false; }

			var myitem = plr.HeldItem.modItem as TheMadRangerItem;
			if( myitem?.IsCylinderFull() ?? true ) { return false; }

			myitem.OpenCylinder( plr );
			this.ReloadDuration = TMRConfig.Instance.ReloadInitTickDuration;

			return true;
		}

		public void BeginHolster( Player plr ) {
			this.HolsterDuration = TMRConfig.Instance.HolsterTwirlTickDuration;
			if( this.HolsterDuration == 0 ) {
				return;
			}

			SoundHelpers.PlaySound( "RevolverTwirl", plr.Center, 0.65f );
		}


		////

		public void StopReloading( Player plr ) {
			var myitem = (TheMadRangerItem)plr.HeldItem.modItem;
			myitem.CloseCylinder( plr );

			this.ReloadDuration = 0;
			this.ReloadingRounds = false;
		}
	}
}