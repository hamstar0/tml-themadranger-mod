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

				this.IsBraveNow = true;

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


		////

		private void AI_CheckRetreatIf() {
			if( !this.npc.HasPlayerTarget || this.npc.target < 0 ) {
				return;
			}

			Player player = Main.player[ this.npc.target ];
			if( player?.active != true || player.dead ) {
				return;
			}

			this.BraveryTimer--;

			if( !this.IsBraveNow ) {
				this.AI_CheckRetreatIf_Unbrave( player );
			}
		}

		private void AI_CheckRetreatIf_Unbrave( Player targetPlayer ) {
			int retreatDist = BanditNPC.RetreatTileDistance * 16;
			int retreatDistSqr = retreatDist * retreatDist;

			if( (targetPlayer.Center - npc.Center).LengthSquared() < retreatDistSqr ) {
				this.IsRetreatingNow = true;
			}
		}


		////

		private void PostAI_ApplyNormalMovementChangesIf() {
			if( this.IsRetreatingNow ) {
				return;
			}
			if( this.npc.velocity.X == 0 ) {
				return;
			}
			if( this.npc.ai[2] != 0f ) {
				return;
			}

			if( Math.Abs(this.npc.velocity.X) < BanditNPC.MaxChaseSpeed ) {
				this.npc.velocity.X += (float)this.npc.direction * 0.3f;
			}
		}

		private void PostAI_ApplyRetreatMovementChangesIf() {
			if( !this.IsRetreatingNow ) {
				return;
			}

			if( this.npc.position.X == 0 && this.npc.velocity.Y == 0f ) {
				this.RetreatStuckElapsed++;

				this.npc.velocity.Y -= 8f;
			} else {
				this.RetreatStuckElapsed = 0;
			}

			if( this.RetreatStuckElapsed > (60 * 5) ) {
				this.RetreatStuckElapsed = 0;

				this.IsBraveNow = true;
				this.IsRetreatingNow = false;
			}
		}
	}
}
