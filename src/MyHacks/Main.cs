using Harmony12;
using System.Reflection;
using UnityModManagerNet;

namespace MyHacks
{
    public static class Main
    {
        public static bool Enabled;
        public static UnityModManager.ModEntry.ModLogger Logger;

        public static bool Load(UnityModManager.ModEntry modEntry)
        {
            Logger = modEntry.Logger;
            Enabled = modEntry.Active;

            modEntry.OnToggle = OnToggle;
            modEntry.OnGUI = OnGUI;
            modEntry.OnSaveGUI = OnSaveGUI;

            var harmony = HarmonyInstance.Create("TaiwuMyHacksMod");
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

        }

        public static void OnSaveGUI(UnityModManager.ModEntry modEntry)
        {

        }
    }
}
