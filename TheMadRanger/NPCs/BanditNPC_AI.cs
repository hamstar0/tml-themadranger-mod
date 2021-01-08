using System;
using Terraria;
using Terraria.ModLoader;
using HamstarHelpers.Helpers.Debug;


namespace TheMadRanger.NPCs {
	partial class BanditNPC : ModNPC {
		public override bool PreAI() {
			BanditNPC.IsFiring = true;

			bool isRetreat = this.PreAI_ApplyRetreatEffectsIf();

			this.PreAI_ApplyRetreatEffectsIf();

			return isRetreat;
		}

		public override void AI() {
			//DebugHelpers.Print( "ai", string.Join(", ", this.npc.ai) );
			this.AI_CheckRetreatIf();
		}

		public override void PostAI() {
			BanditNPC.IsFiring = false;

			this.PostAI_ApplyNormalMovementChangesIf();
			this.PostAI_ApplyRetreatMovementChangesIf();
		}


		////////////////

		public bool BeginRetreat() {
			if( this.HasAttemptedRetreat ) {
				return false;
			}

			this.HasAttemptedRetreat = true;
			this.IsRetreatingNow = true;
			return true;
		}
	}
}
