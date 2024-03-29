﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using ModLibsCore.Libraries.Debug;
using ModLibsNet.Services.Network;


namespace TheMadRanger.Logic {
	partial class PlayerLogic {
		public static (bool IsAimWithinArc, int AimDir) ApplyGunAimFromCursorData( TMRPlayer myplayer ) {
			if( myplayer.player.whoAmI == Main.myPlayer ) {
				return PlayerLogic.ApplyGunAim( myplayer, Main.mouseX, Main.mouseY );
			} else {
				(int x, int y) cursor;
				if( ClientCursorData.LastKnownCursorPositions.ContainsKey( myplayer.player.whoAmI ) ) {
					cursor = ClientCursorData.LastKnownCursorPositions[myplayer.player.whoAmI];
				} else {
					cursor = ((int)myplayer.player.MountedCenter.X, (int)myplayer.player.MountedCenter.Y);
					cursor.x += ( myplayer.player.direction * 256 ) + 1;
				}

				return PlayerLogic.ApplyGunAim( myplayer, cursor.x, cursor.y );
			}
		}

		////

		public static (bool IsAimWithinArc, int AimDir) ApplyGunAim(
					TMRPlayer myplayer,
					int screenX,
					int screenY ) {
			Player plr = myplayer.player;
			Texture2D itemTex = Main.itemTexture[plr.HeldItem.type];

			Vector2 plrCenter = plr.RotatedRelativePoint( plr.MountedCenter, true );

			float aimX = (float)screenX + Main.screenPosition.X - plrCenter.X;
			float aimY = (float)screenY + Main.screenPosition.Y - plrCenter.Y;
			if( plr.gravDir == -1f ) {
				aimY = Main.screenPosition.Y + (float)Main.screenHeight - (float)screenY - plrCenter.Y;
			}

			//

			plr.itemRotation = (float)Math.Atan2(
				(double)( aimY * (float)plr.direction ),
				(double)( aimX * (float)plr.direction )
			) - plr.fullRotation;

			bool isAimWithinArc = true;

			if( plr.direction > 0 ) {
				float rot1 = 1.2f;
				float rot2 = -1.5f;

				if( plr.itemRotation > rot1 || plr.itemRotation < rot2 ) {
					float upTest = Math.Abs( plr.itemRotation - rot2 );
					float downTest = Math.Abs( plr.itemRotation - rot1 );

					if( upTest < downTest ) {
						plr.itemRotation = rot2;
					} else {
						plr.itemRotation = rot1;
					}
					isAimWithinArc = false;
				}
			} else {
				float rot1 = -1.2f;
				float rot2 = 1.5f;

				if( plr.itemRotation < rot1 || plr.itemRotation > rot2 ) {
					float upTest = Math.Abs( plr.itemRotation - rot2 );
					float downTest = Math.Abs( plr.itemRotation - rot1 );

					if( upTest < downTest ) {
						plr.itemRotation = rot2;
					} else {
						plr.itemRotation = rot1;
					}
					isAimWithinArc = false;
				}
			}

			float addedRotDeg = myplayer.GunHandling.GetAddedRotationDegrees( plr );
			if( addedRotDeg != 0f ) {
				plr.itemRotation = MathHelper.ToDegrees( plr.itemRotation ) - (float)( plr.direction * addedRotDeg );
				plr.itemRotation = MathHelper.ToRadians( plr.itemRotation );
			} else {
				plr.itemRotation += myplayer.AimMode.GetAimStateShakeRadiansOffset( true );
			}

			//

			plr.itemLocation.X = plr.position.X
				+ ( (float)plr.width * 0.5f )
				- ( itemTex.Width * 0.5f )
				- plr.direction * 2;
			plr.itemLocation.Y = plr.MountedCenter.Y
				- ( (float)itemTex.Height * 0.5f );

			//

			ItemLoader.UseStyle( plr.HeldItem, plr );

			//

			//NetMessage.SendData( MessageID.PlayerControls, -1, -1, null, plr.whoAmI, 0f, 0f, 0f, 0, 0, 0 );
			//NetMessage.SendData( MessageID.ItemAnimation, -1, -1, null, plr.whoAmI, 0f, 0f, 0f, 0, 0, 0 );

			return ( isAimWithinArc, Math.Sign(aimX) );
		}
	}
}
