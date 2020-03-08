using Microsoft.Xna.Framework;
using System;
using Terraria;

namespace TheOlBigIron {
	class GunAnimation {
		public int HolsterDuration { get; private set; } = 0;
		public float AddedRotationDegrees { get; private set; } = 0f;

		////

		public float AddedRotationRadians => MathHelper.ToRadians( this.AddedRotationDegrees );



		////////////////

		public void Update() {
			if( this.HolsterDuration > 0 ) {
				this.HolsterDuration--;

				this.AddedRotationDegrees += 24f;
				if( this.AddedRotationDegrees > 360f ) {
					this.AddedRotationDegrees -= 360f;
				}
			} else {
				this.AddedRotationDegrees = 0f;
			}
		}

		////////////////

		public void BeginHolster() {
			this.HolsterDuration = 60;
		}


		////////////////

		public void DrawGun( Player player ) {
			
		}
	}
}
