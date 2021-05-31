using System;
using Terraria;
using Terraria.ModLoader;
using ModLibsCore.Libraries.Debug;
using TheMadRanger.Items;


namespace TheMadRanger.Logic {
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

		public PlayerLayer GunAnimDrawLayer { get; private set; }



		////////////////

		public GunHandling() {
			this.InitGunAnimDrawLayers();
		}


		////////////////

		public bool CanAttemptToShootGun( Player player ) {
			return !this.IsHolstering
				&& ( !this.IsReloading || this.ReloadingRounds )
				&& !SpeedloaderItem.IsReloading( player.whoAmI );
		}
	}
}
