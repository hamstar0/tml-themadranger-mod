using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria;
using Terraria.ModLoader;
using HamstarHelpers.Classes.Loadable;


namespace TheMadRanger.Helpers.Misc {
	public class SoundHelpers : ILoadable {
		public static void PlaySound( string soundName, Vector2 position, float volume=1f ) {
			LegacySoundStyle sound;
			var sndHelp = ModContent.GetInstance<SoundHelpers>();

			if( sndHelp.Sounds.ContainsKey(soundName) ) {
				sound = sndHelp.Sounds[soundName];
			} else {
				sound = TMRMod.Instance.GetLegacySoundSlot(
					Terraria.ModLoader.SoundType.Custom,
					"Sounds/Custom/" + soundName
				).WithVolume( volume );

				sndHelp.Sounds[soundName] = sound;
			}

			Main.PlaySound( sound, position );
		}



		////////////////

		private IDictionary<string, LegacySoundStyle> Sounds = new Dictionary<string, LegacySoundStyle>();



		////////////////

		void ILoadable.OnModsLoad() { }

		void ILoadable.OnModsUnload() { }

		void ILoadable.OnPostModsLoad() { }
	}
}
