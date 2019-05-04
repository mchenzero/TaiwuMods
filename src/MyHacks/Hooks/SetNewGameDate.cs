using Harmony12;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MyHacks.Hooks
{
    public static class SetNewGameDate
    {
        [GeneratorPatch(typeof(NewGame), "SetNewGameDate")]
        public static class SetNewGameDatePatch
        {
            public static bool AlreadyInPatch = false;

            public static void PostMoveNext(bool __result)
            {
                if (__result == false && Main.Enabled && !AlreadyInPatch)
                {
                    try
                    {
                        AlreadyInPatch = true;
                        RunPostSetNewGameDateHooks();
                    }
                    finally
                    {
                        AlreadyInPatch = false;
                    }
                }
            }
        }

        private static void RunPostSetNewGameDateHooks()
        {
            var methods = Assembly.GetExecutingAssembly().GetTypes()
                            .Where(t => t.Namespace == "MyHacks.Hacks" && t.IsClass)
                            .Select(t => t.GetMethod("PostSetNewGameDate", BindingFlags.Public | BindingFlags.Static))
                            .Where(m => m != null)
                            .ToList();

            foreach (var m in methods)
            {
                try
                {
                    m.Invoke(null, null);
                }
                catch (Exception ex)
                {
                    Main.Logger.Log(ex.ToString());
                }
            }
        }
    }
}
