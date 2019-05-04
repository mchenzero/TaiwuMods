using Harmony12;
using System;
using System.Linq;
using System.Reflection;

namespace MyHacks.Hooks
{
    public static class RandomHeirMakeNewActor
    {
        [HarmonyPatch(typeof(DateFile), "MakeNewActor")]
        public static class MakeNewActorPatch
        {
            public static bool AlreadyInPatch = false;

            public static void Postfix(int __result)
            {
                if (Main.Enabled && IsInRandomHeirEvent() && !AlreadyInPatch)
                {
                    try
                    {
                        AlreadyInPatch = true;
                        RunPostRandomHeirMakeNewActorHooks(__result);
                    }
                    finally
                    {
                        AlreadyInPatch = false;
                    }
                }
            }
        }

        private static bool IsInRandomHeirEvent()
        {
            var eventValue = MassageWindow.instance?.eventValue;
            return eventValue != null && eventValue.Count >= 2 && eventValue[0] == 111 && eventValue[1] == 1;
        }

        private static void RunPostRandomHeirMakeNewActorHooks(int actorId)
        {
            var methods = Assembly.GetExecutingAssembly().GetTypes()
                            .Where(t => t.Namespace == "MyHacks.Hacks" && t.IsClass)
                            .Select(t => t.GetMethod("PostRandomHeirMakeNewActor", BindingFlags.Public | BindingFlags.Static))
                            .Where(m => m != null)
                            .ToList();

            foreach (var m in methods)
            {
                try
                {
                    m.Invoke(null, new object[] { actorId });
                }
                catch (Exception ex)
                {
                    Main.Logger.Log(ex.ToString());
                }
            }
        }
    }
}
