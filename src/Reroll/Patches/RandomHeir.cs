using Harmony12;
using UnityEngine;

namespace Reroll.Patches
{
    public static class RandomHeir
    {
        [HarmonyPatch(typeof(DateFile), "MakeNewActor")]
        public static class MakeNewActorPatch
        {
            public static bool AlreadyInPatch = false;

            public static void Postfix(int __result)
            {
                var newActorId = __result;

                if (Main.Enabled && IsInRandomHeirEvent() && !AlreadyInPatch)
                {
                    try
                    {
                        AlreadyInPatch = true;
                        TestAndReroll(newActorId);
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

        private static void TestAndReroll(int actorId)
        {
            var result = Taiwu.Test(actorId, Main.Settings.RandomHeir);

            for (var i = 0; i < Main.Settings.MaxRoll; i++)
            {
                if (result.IsMatch)
                {
                    Main.Logger.Log(string.Format("重掷{0}次后找到匹配！", i));
                    break;
                }

                var success = Reroll(actorId);

                if (!success)
                {
                    Main.Logger.Log("游戏更新或与其他Mod冲突导致重掷失败！");
                    return;
                }

                result = Taiwu.Test(actorId, Main.Settings.RandomHeir, result);
            }

            if (!result.IsMatch)
            {
                result.LogFailure();
            }
        }

        private static bool Reroll(int actorId)
        {
            try
            {
                // 简单起见，我们通过删除角色数据并回撤newActorId的方式来实现重掷。为此，先确保actorId与newActorId一致。
                if (actorId != DateFile.instance.newActorId)
                {
                    Main.Logger.Log("当前重掷角色不是系统最新角色！");
                    return false;
                }

                Taiwu.RemoveActor(actorId);
                DateFile.instance.newActorId--;

                // 来源：MassageWindow.EndEvent11_1
                int actorId2 = DateFile.instance.MakeNewActor(int.Parse(DateFile.instance.placeWorldDate[DateFile.instance.mianWorldId + 1][87]) + Random.Range(0, 2), true, 0, Random.Range(12, 15), -1, (string[])null, (string[])null, (string[])null, (string[])null, 20);

                if (actorId2 != actorId)
                {
                    Main.Logger.Log("新老角色ID不一致！");
                    return false;
                }

                return true;
            }
            catch (System.Exception ex)
            {
                Main.Logger.Log(ex.Message);
                return false;
            }
        }
    }
}
