using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Harmony12;

namespace Rebirth
{
    public static class Patch
    {
        public static void LoadAll()
        {
            HarmonyInstance.Create("TaiwuRebirthMod").PatchAll(Assembly.GetExecutingAssembly());
        }
    }

    [HarmonyPatch(typeof(DateFile), "RemoveActor")]
    public static class RemoveActorPatch
    {
        // 人物死亡时部分数据会被删除，这里做一下备份
        public static int TaiwuMaxAgeBackup = 0;
        public static string TaiwuAbilitiesBackup = ""; // 能力和资质不会被删除，但 早熟/晚熟 设定会被删除
        public static string TaiwuFeaturesBackup = "";

        public static void Prefix(List<int> actorId)
        {
            for (int i = 0; i < actorId.Count; i++)
            {
                if (actorId[i] == Taiwu.MainActorId)
                {
                    TaiwuMaxAgeBackup = Taiwu.GetMaxAge();
                    TaiwuAbilitiesBackup = Taiwu.GetAbilities();
                    TaiwuFeaturesBackup = Taiwu.GetFeatures();
                }
            }
        }
    }

    [HarmonyPatch(typeof(DateFile), "MakeNewActor")]
    public static class MakeNewActorPatch
    {
        public static void Prefix(ref int baseActorId)
        {
            if (Main.Enabled && IsInRandomHeirEvent() && IsTaiwuOldEnough())
            {
                // 人物性别存储在 presetActorDate 中，强行在 actorsDate 中设置性别会有副作用（死亡后性别恢复）。
                // 因此这里通过在创建人物时人为选择 baseActorId (presetActorId) 来实现性别修改。
                if (Main.Settings.Gender != Settings.Enums.Gender.Random)
                {
                    var gender = Taiwu.GetGender(); // 男: 1, 女: 2

                    if (Main.Settings.Gender == Settings.Enums.Gender.Invert)
                    {
                        gender = 3 - gender;
                    }

                    baseActorId = Taiwu.GetBasePresetActorId() + gender - 1;
                }
            }
        }

        public static void Postfix(int __result)
        {
            if (Main.Enabled && IsInRandomHeirEvent() && IsTaiwuOldEnough())
            {
                int newActorId = __result;
                UpdateName(newActorId);
                UpdateAppearance(newActorId);
                UpdateGoodness(newActorId);
                UpdateAbilities(newActorId);
                UpdateFeatures(newActorId);
                UpdateSamsara(newActorId);
            }
        }

        private static bool IsInRandomHeirEvent()
        {
            var eventValue = MassageWindow.instance?.eventValue;
            return eventValue != null && eventValue.Count >= 2 && eventValue[0] == 111 && eventValue[1] == 1;
        }

        private static bool IsTaiwuOldEnough()
        {
            return Taiwu.GetAge() >= Math.Min(30, RemoveActorPatch.TaiwuMaxAgeBackup - 1);
        }

        private static void UpdateName(int newActorId)
        {
            switch (Main.Settings.Name)
            {
                case Settings.Enums.Name.Keep:
                    Taiwu.SetSurname(newActorId, Taiwu.GetSurname());
                    Taiwu.SetForename(newActorId, Taiwu.GetForename());
                    break;
                case Settings.Enums.Name.Change:
                    Taiwu.SetSurname(newActorId, Taiwu.IsMale(newActorId) ? Main.Settings.SurnameMale : Main.Settings.SurnameFemale);
                    Taiwu.SetForename(newActorId, Taiwu.IsMale(newActorId) ? Main.Settings.ForenameMale : Main.Settings.ForenameFemale);
                    break;
            }
        }

        private static void UpdateAppearance(int newActorId)
        {
            switch (Main.Settings.Appearance)
            {
                case Settings.Enums.Appearance.Keep:
                    Taiwu.SetAppearance(newActorId, Taiwu.GetAppearance());
                    break;
                case Settings.Enums.Appearance.Change:
                    Taiwu.SetAppearance(newActorId, Taiwu.IsMale(newActorId) ? Main.Settings.AppearanceMale : Main.Settings.AppearanceFemale);
                    break;
            }
        }

        private static void UpdateGoodness(int newActorId)
        {
            if (Main.Settings.Goodness == Settings.Enums.Goodness.Keep)
            {
                Taiwu.SetGoodness(newActorId, Taiwu.GetGoodness());
            }
        }

        private static void UpdateAbilities(int newActorId)
        {
            if (Main.Settings.Abilities == Settings.Enums.Abilities.Inherit)
            {
                Taiwu.SetAbilities(newActorId, RemoveActorPatch.TaiwuAbilitiesBackup);
            }
        }

        private static void UpdateFeatures(int newActorId)
        {
            if (Main.Settings.Features == Settings.Enums.Features.Inherit)
            {
                var features = RemoveActorPatch.TaiwuFeaturesBackup;

                features = Regex.Replace(features, @"\b(9998|9999)\b", "9997"); // 相枢入邪、相枢化魔 替换为 神魂俱安
                features = Regex.Replace(features, @"\b(4002|4003)\b", "4001"); // 杂阳毁阴、身怀六甲 替换为 真阳纯阴

                features = String.Join("|", features.Split('|').Distinct().ToArray()); // 保险起见，去重

                Taiwu.SetFeatures(newActorId, features);
                Taiwu.SetBornSolarTerm(newActorId, Taiwu.GetBornSolarTerm());
            }
        }

        private static void UpdateSamsara(int newActorId)
        {
            var mainActorId = Taiwu.MainActorId;
            var actorLifeData = DateFile.instance.actorLife;

            var newActorSamsara = new List<int>(DateFile.instance.GetLifeDateList(mainActorId, 801, false)) { mainActorId };

            if (!actorLifeData[newActorId].ContainsKey(801))
            {
                actorLifeData[newActorId].Add(801, newActorSamsara);
            }
            else
            {
                actorLifeData[newActorId][801] = newActorSamsara;
            }

            DateFile.instance.deadActors.Remove(mainActorId);
        }
    }
}
