using System;
using UnityModManagerNet;

namespace Reroll
{
    public class Settings : UnityModManager.ModSettings
    {
        public Filters NewGame = new Filters();
        public Filters LongDao = new Filters();
        public Filters RandomHeir = new Filters();

        public int MaxRoll = 1000;

        public override void Save(UnityModManager.ModEntry modEntry)
        {
            UnityModManager.ModSettings.Save<Settings>(this, modEntry);
        }
    }
}
