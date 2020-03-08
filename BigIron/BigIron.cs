using System.Collections.Generic;
using Terraria.ModLoader;

namespace BigIron {
	public class BigIron : Mod { public BigIron() { } }

	class BigIronPlayer : ModPlayer {
		/*public override void ModifyDrawInfo( ref PlayerDrawInfo drawInfo ) {
			drawInfo.drawAltHair = false;
			drawInfo.drawArms = false;
			drawInfo.drawHair = false;
			drawInfo.drawHands = false;
			drawInfo.drawHeldProjInFrontOfHeldItemAndBody = false;
		}*/

		public override void ModifyDrawLayers( List<PlayerLayer> layers ) {
			//layers.Remove( PlayerLayer.HairBack );

			//PlayerLayer.HairBack.visible = false;
			//PlayerLayer.MiscEffectsFront.visible = false;
			//PlayerLayer.FrontAcc.visible = false;
			//PlayerLayer.HeldProjFront.visible = false;
			PlayerLayer.HandOnAcc.visible = false;
			PlayerLayer.Arms.visible = false;
			PlayerLayer.HeldItem.visible = false;
			//PlayerLayer.HeldProjBack.visible = false;
			//PlayerLayer.SolarShield.visible = false;
			//PlayerLayer.ShieldAcc.visible = false;
			//PlayerLayer.MountFront.visible = false;
			//PlayerLayer.FaceAcc.visible = false;
			//PlayerLayer.Head.visible = false;
			//PlayerLayer.Hair.visible = false;
			//PlayerLayer.NeckAcc.visible = false;
			//PlayerLayer.WaistAcc.visible = false;
			//PlayerLayer.HandOffAcc.visible = false;
			//PlayerLayer.Body.visible = false;
			//PlayerLayer.Legs.visible = false;
			//PlayerLayer.BalloonAcc.visible = false;
			//PlayerLayer.Wings.visible = false;
			//PlayerLayer.BackAcc.visible = false;
			//PlayerLayer.MiscEffectsBack.visible = false;
			//PlayerLayer.MountBack.visible = false;
			//PlayerLayer.Face.visible = false;
			//PlayerLayer.Skin.visible = false;
		}
	}
}