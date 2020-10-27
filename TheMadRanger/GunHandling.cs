using System;
using Terraria;
using Terraria.ModLoader;
using HamstarHelpers.Helpers.Audio;
using HamstarHelpers.Helpers.Debug;
using TheMadRanger.Items.Weapons;
using TheMadRanger.Items;


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

		public bool IsQuickDrawReady { get; internal set; } = true;


		////

		public float HolsterTwirlAddedRotationDegrees { get; private set; } = 0f;

		public float MiscAddedRotationDegrees { get; private set; } = 0f;


		////

		public PlayerLayer GunDrawLayer { get; private set; }



		////////////////

		public GunHandling() {
			this.InitDrawLayers();
		}


		////////////////

		public void BeginRecoil( float addedRotationDegrees ) {
			this.MiscAddedRotationDegrees = addedRotationDegrees;
			this.RecoilDuration = 17;
		}

		public bool BeginReload( Player plr ) {
			if( this.IsReloading ) {
				return false;
			}

			if( SpeedloaderItem.IsReloading(plr.whoAmI) ) {
				return false;
			}

			var myitem = plr.HeldItem.modItem as TheMadRangerItem;
			if( myitem?.IsCylinderFull() ?? true ) {
				return false;
			}

			myitem.OpenCylinder( plr );
			this.ReloadDuration = TMRConfig.Instance.Get<int>( nameof(TMRConfig.ReloadInitTickDuration) );

			this.IsQuickDrawReady = true;

			return true;
		}

		public void BeginHolster( Player plr ) {
			this.HolsterDuration = TMRConfig.Instance.Get<int>( nameof(TMRConfig.HolsterTwirlTickDuration) );

			this.IsQuickDrawReady = true;

			if( this.HolsterDuration == 0 ) {
				return;
			}

			SoundHelpers.PlaySound( TMRMod.Instance, "RevolverTwirl", plr.Center, 0.65f );
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
