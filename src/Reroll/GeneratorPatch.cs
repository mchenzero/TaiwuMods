using System;
using System.Linq;
using System.Reflection;
using Harmony12;

namespace Reroll
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class GeneratorPatch : Attribute
    {
        public Type Type;
        public string Method;

        public GeneratorPatch(Type type, string method)
        {
            Type = type;
            Method = method;
        }

        public static void ApplyAll(HarmonyInstance harmony)
        {
            foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
            {
                var attributes = type.GetCustomAttributes(true)
                    .Where(obj => obj is GeneratorPatch);

                foreach (GeneratorPatch attr in attributes)
                {
                    var patch = type;
                    var enumerator = attr.Type.GetNestedTypes(BindingFlags.NonPublic)
                        .FirstOrDefault(t => t.Name.Contains($"<{attr.Method}>"));

                    if (enumerator == null) {
                        throw new Exception($"Patch failed. Enumerator type not found for {attr.Type.Name}.{attr.Method}");
                    }

                    var method = AccessTools.Method(enumerator, "MoveNext");
                    var postfix = AccessTools.Method(patch, "PostMoveNext");

                    if (method != null && postfix != null)
                    {
                        harmony.Patch(method, null, new HarmonyMethod(postfix));
                    }
                }
            }
        }
    }
}
