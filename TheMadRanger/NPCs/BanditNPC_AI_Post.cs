using System;
using Terraria;
using Terraria.ModLoader;
using ModLibsCore.Libraries.Debug;


namespace TheMadRanger.NPCs {
	partial class BanditNPC : ModNPC {
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

			var config = TMRConfig.Instance;
			float maxChaseSpeed = config.Get<float>( nameof(config.BanditMaxChaseSpeed) );

			if( Math.Abs(this.npc.velocity.X) < maxChaseSpeed ) {
				this.npc.velocity.X += (float)this.npc.direction * 0.3f;
			}
		}

		private void PostAI_ApplyRetreatMovementChangesIf() {
			if( !this.IsRetreatingNow ) {
				return;
			}

			if( this.npc.velocity.LengthSquared() < 1f ) {
				this.RetreatStuckElapsed++;

				this.npc.velocity.Y -= 8f;
			} else {
				this.RetreatStuckElapsed = 0;
			}

			if( this.RetreatStuckElapsed > (60 * 5) ) {
				this.RetreatStuckElapsed = 0;

				this.IsRetreatingNow = false;
			}
		}
	}
}
