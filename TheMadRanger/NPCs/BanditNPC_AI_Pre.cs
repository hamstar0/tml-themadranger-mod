using System;
using Terraria;
using Terraria.ModLoader;
using HamstarHelpers.Helpers.Debug;


namespace TheMadRanger.NPCs {
	partial class BanditNPC : ModNPC {
		private bool PreAI_ApplyRetreatEffectsIf() {
			if( !this.IsRetreatingNow ) {
				return true;
			}

			this.RetreatTimer--;

			// Done retreating; don't relapse
			if( this.RetreatTimer <= 0 ) {
				this.IsRetreatingNow = false;	// redundant

				return true;
			}

			this.PreAI_ApplyRetreatIf_ApplyDirectionAndAI();
			this.PreAI_ApplyRetreatIf_ApplyRun();

			return false;
		}

		private void PreAI_ApplyRetreatIf_ApplyDirectionAndAI() {
			this.npc.ai[0] = 1f;
			this.npc.ai[1] = 0f;
			this.npc.ai[2] = 0f;
			this.npc.ai[3] = 0f;

			Player player = Main.player[ this.npc.target ];

			float dir = this.npc.oldDirection;
			if( player?.active == true ) {
				dir = (this.npc.Center.X - player.Center.X) < 0f
						? -1f : 1f;
			}

			this.npc.direction = (int)dir;
		}

		private void PreAI_ApplyRetreatIf_ApplyRun() {
			//bool isAimedInVelocityDir = (this.npc.direction > 1 && this.npc.velocity.X > 0f)
			//	|| (this.npc.direction < 1 && this.npc.velocity.X < 0f);
			if( Math.Abs(this.npc.velocity.X) < BanditNPC.MaxRetreatSpeed ) {
				this.npc.velocity.X += (float)this.npc.direction * 0.1f;
			}
		}
	}
}
