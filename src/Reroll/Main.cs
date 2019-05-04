using System;
using System.Reflection;
using Harmony12;
using UnityModManagerNet;

namespace Reroll
{
    public static class Main
    {
        public static bool Enabled;
        public static Settings Settings;
        public static UnityModManager.ModEntry.ModLogger Logger;

        public static bool Load(UnityModManager.ModEntry modEntry)
        {
            Logger = modEntry.Logger;

            Enabled = modEntry.Active;
            Settings = UnityModManager.ModSettings.Load<Settings>(modEntry);

            modEntry.OnToggle = OnToggle;
            modEntry.OnGUI = OnGUI;
            modEntry.OnSaveGUI = OnSaveGUI;

            var harmony = HarmonyInstance.Create("TaiwuRerollMod");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            GeneratorPatch.ApplyAll(harmony);

            return true;
        }

        public static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            Enabled = value;
            return true;
        }

        public static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            GUI.Render();
        }

        public static void OnSaveGUI(UnityModManager.ModEntry modEntry)
        {
            Settings.Save(modEntry);
        }
    }
}
