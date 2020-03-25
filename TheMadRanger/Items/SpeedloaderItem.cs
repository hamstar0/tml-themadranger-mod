using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using HamstarHelpers.Services.Timers;
using TheMadRanger.Recipes;
using TheMadRanger.Items.Weapons;
using TheMadRanger.Helpers.Misc;


namespace TheMadRanger.Items {
	partial class SpeedloaderItem : ModItem {
		public static int Width = 10;
		public static int Height = 10;



		////////////////

		public int LoadedRounds { get; private set; } = 0;


		////////////////

		public override bool CloneNewInstances => false;

		public override string Texture {
			get {
				if( this.LoadedRounds > 0 ) {
					return "TheMadRanger/Items/SpeedloaderItem_Loaded";
				}
				return base.Texture;
			}
		}



		////////////////

		public override void SetStaticDefaults() {
			this.DisplayName.SetDefault( ".357 Speedloader" );
			this.Tooltip.SetDefault(
				"Quickly loads a .357 revolver cylinder."
				+"\nRight-click to preload speedloader"
			);
		}

		public override void SetDefaults() {
			this.item.width = SpeedloaderItem.Width;
			this.item.height = SpeedloaderItem.Height;
			this.item.maxStack = 1;
			this.item.value = Item.buyPrice( 0, 1, 0, 0 );
			this.item.rare = 4;
			this.item.scale = 0.75f;
		}


		////////////////

		public override void AddRecipes() {
			var recipe = new SpeedloaderRecipe( this );
			recipe.AddRecipe();
		}


		////////////////

		public override void Load( TagCompound tag ) {
			if( !tag.ContainsKey( "rounds" ) ) {
				return;
			}

			this.LoadedRounds = tag.GetInt( "rounds" );
		}

		public override TagCompound Save() {
			var tag = new TagCompound {
				{ "rounds", this.LoadedRounds }
			};

			return tag;
		}


		////////////////
		
		public override bool PreDrawInInventory(
					SpriteBatch spriteBatch,
					Vector2 position,
					Rectangle frame,
					Color drawColor,
					Color itemColor,
					Vector2 origin,
					float scale ) {
			Texture2D tex = Main.itemTexture[ this.item.type ];
			if( this.LoadedRounds > 0 ) {
				tex = ModContent.GetTexture( this.Texture );
			}

			spriteBatch.Draw( tex, position, frame, drawColor, 0f, origin, scale, SpriteEffects.None, 0f );
			if( this.item.color != Color.Transparent ) {
				spriteBatch.Draw( tex, position, frame, itemColor, 0f, origin, scale, SpriteEffects.None, 0f );
			}

			return false;
		}
		
		public override void PostDrawInWorld(
					SpriteBatch spriteBatch,
					Color lightColor,
					Color alphaColor,
					float rotation,
					float scale,
					int whoAmI ) {
			Texture2D tex = Main.itemTexture[this.item.type];
			if( this.LoadedRounds > 0 ) {
				tex = ModContent.GetTexture( this.Texture );
			}

			float offsetX = this.item.height - tex.Height;
			float offsetY = (this.item.width / 2) - (tex.Width / 2);

			Color alpha = this.item.GetAlpha( lightColor );

			Main.spriteBatch.Draw(
				tex,
				(this.item.position - Main.screenPosition) + new Vector2(
					(float)( tex.Width / 2 ) + offsetY,
					(float)( tex.Height / 2 ) + offsetX + 2f
				),
				new Rectangle( 0, 0, tex.Width, tex.Height ),
				alpha,
				rotation,
				new Vector2( tex.Width / 2, tex.Height / 2 ),
				scale,
				SpriteEffects.None,
				0f
			);
			if( this.item.color != default(Color) ) {
				Main.spriteBatch.Draw(
					tex,
					(this.item.position - Main.screenPosition) + new Vector2(
						(float)( tex.Width / 2 ) + offsetY,
						(float)( tex.Height / 2 ) + offsetX + 2f
					),
					new Rectangle( 0, 0, tex.Width, tex.Height ),
					this.item.GetColor( lightColor ),
					rotation,
					new Vector2( tex.Width / 2, tex.Height / 2 ),
					scale,
					SpriteEffects.None,
					0f
				);
			}
		}


		////////////////

		public override bool CanRightClick() {
			return true;
		}

		public override bool ConsumeItem( Player player ) {
			if( this.LoadedRounds > 0 ) {
				return false;
			}

			this.AttemptReload( player );

			return false;
		}


		////////////////

		public bool AttemptReload( Player player ) {
			int plrWho = player.whoAmI;

			bool Reload() {
				Player plr = Main.player[plrWho];
				if( !TheMadRangerItem.IsAmmoSourceAvailable( plr, true ) ) {
					return false;
				}

				this.LoadedRounds = TheMadRangerItem.CylinderCapacity;
				return true;
			}

			//

			if( !TheMadRangerItem.IsAmmoSourceAvailable(player, true) ) {
				return false;
			}

			var myplayer = player.GetModPlayer<TMRPlayer>();

			if( myplayer.GunHandling.IsAnimating ) {
				return false;
			}

			string timerName = "TheMadRangerSpeedloaderLoad_" + plrWho;

			if( Timers.GetTimerTickDuration(timerName) > 0 ) {
				return false;
			}

			SoundHelpers.PlaySound( "RevolverReloadBegin", player.Center, 0.5f );

			Timers.SetTimer( timerName, TMRConfig.Instance.SpeedloaderLoadTickDuration, false, () => {
				Reload();
				return false;
			} );

			return true;
		}

		////

		public void TransferRoundsOut( Player player ) {
			this.LoadedRounds = 0;

			SoundHelpers.PlaySound( "RevolverReloadRound", player.Center, 0.5f );
		}
	}
}
