using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Rebirth
{
    public static class Taiwu
    {
        public static int MainActorId
        {
            get { return DateFile.instance.mianActorId; }
        }

        public static int MainWorldId
        {
            get { return DateFile.instance.mianWorldId; }
        }

        /// <summary>判断是否已读取存档进入游戏</summary>
        public static bool IsInGame()
        {
            var actorData = DateFile.instance?.actorsDate;
            return actorData != null && actorData.ContainsKey(MainActorId);
        }

        /// <summary>获取太吾姓氏</summary>
        public static string GetSurname()
        {
            return GetSurname(MainActorId);
        }

        /// <summary>获取指定人物姓氏</summary>
        public static string GetSurname(int actorId)
        {
            var surnameText = GetActorData(actorId, 5);

            if (surnameText != "")
            {
                return surnameText;
            }

            var surnameId = int.Parse(GetActorData(actorId, 29));

            return DateFile.instance.actorSurnameDate[surnameId][0];
        }

        /// <summary>设置太吾姓氏</summary>
        public static void SetSurname(string value)
        {
            SetSurname(MainActorId, value);
        }

        /// <summary>设置指定人物姓氏</summary>
        public static void SetSurname(int actorId, string value)
        {
            if (value == "") return;

            var presetSurnameKvp = DateFile.instance.actorSurnameDate.FirstOrDefault(p => p.Value[0] == value);

            if (presetSurnameKvp.Value == null)
            {
                SetActorData(actorId, 5, value, true);
            }
            else
            {
                RemoveActorDataKey(actorId, 5);
                SetActorData(actorId, 29, presetSurnameKvp.Key.ToString(), true);
            }
        }

        /// <summary>获取太吾名字</summary>
        public static string GetForename()
        {
            return GetForename(MainActorId);
        }

        /// <summary>获取指定人物名字</summary>
        public static string GetForename(int actorId)
        {
            return GetActorData(actorId, 0);
        }

        /// <summary>设置太吾名字</summary>
        public static void SetForename(string value)
        {
            SetForename(MainActorId, value);
        }

        /// <summary>设置指定人物名字</summary>
        public static void SetForename(int actorId, string value)
        {
            if (value != "")
            {
                SetActorData(actorId, 0, value, true);
            }
        }

        /// <summary>获取太吾性别</summary>
        public static int GetGender()
        {
            return GetGender(MainActorId);
        }

        /// <summary>获取指定人物性别</summary>
        public static int GetGender(int actorId)
        {
            return (GetActorData(actorId, 14) == "1") ? 1 : 2;
        }

        /// <summary>判断太吾是否为男性</summary>
        public static bool IsMale()
        {
            return IsMale(MainActorId);
        }

        /// <summary>判断指定人物是否为男性</summary>
        public static bool IsMale(int actorId)
        {
            return GetGender(actorId) == 1;
        }

        /// <summary>获取太吾年龄</summary>
        public static int GetAge()
        {
            return GetAge(MainActorId);
        }

        /// <summary>获取指定人物年龄</summary>
        public static int GetAge(int actorId)
        {
            return int.Parse(GetActorData(actorId, 11));
        }

        /// <summary>获取太吾寿命</summary>
        public static int GetMaxAge()
        {
            return GetMaxAge(MainActorId);
        }

        /// <summary>获取指定人物寿命</summary>
        public static int GetMaxAge(int actorId)
        {
            return int.Parse(GetActorData(actorId, 13));
        }

        /// <summary>获取太吾相貌</summary>
        public static string GetAppearance()
        {
            return GetAppearance(MainActorId);
        }

        /// <summary>获取指定人物相貌</summary>
        public static string GetAppearance(int actorId)
        {
            var face = GetActorData(actorId, 995);
            var faceColors = GetActorData(actorId, 996);
            var genderChange = GetActorData(actorId, 17);
            var charm = GetActorData(actorId, 15);
            return string.Format("{0}:{1}:{2}:{3}", face, faceColors, genderChange, charm);
        }

        /// <summary>设置太吾相貌</summary>
        public static void SetAppearance(string value)
        {
            SetAppearance(MainActorId, value);
        }

        /// <summary>设置指定人物相貌</summary>
        public static void SetAppearance(int actorId, string value)
        {
            if (IsAppearanceValueValid(value))
            {
                var parts = value.Split(':');

                SetActorData(actorId, 995, parts[0], true);
                SetActorData(actorId, 996, parts[1], true);

                // 男生女相/女生男性 默认使用preset中的值
                if (parts[2] != GetActorData(actorId, 17))
                {
                    SetActorData(actorId, 17, parts[2], true);
                }

                SetActorData(actorId, 15, parts[3], true);
            }
        }

        /// <summary>检查相貌参数格式是否正确</summary>
        public static bool IsAppearanceValueValid(string value)
        {
            return Regex.IsMatch(value, @"^(\d+\|){7}\d+:(\d+\|){7}\d+:\d:\d+$"); // 仅做格式检查
        }

        /// <summary>获取太吾立场</summary>
        public static int GetGoodness()
        {
            return GetGoodness(MainActorId);
        }

        /// <summary>获取指定人物立场</summary>
        public static int GetGoodness(int actorId)
        {
            return int.Parse(GetActorData(actorId, 16));
        }

        /// <summary>设置太吾立场</summary>
        public static void SetGoodness(int value)
        {
            SetGoodness(MainActorId, value);
        }

        /// <summary>设置指定人物立场</summary>
        public static void SetGoodness(int actorId, int value)
        {
            if (value >= 0 && value <= 1000)
            {
                SetActorData(actorId, 16, value.ToString(), true);
            }
        }

        /// <summary>获取太吾属性(能力+资质)</summary>
        public static string GetAbilities()
        {
            return GetAbilities(MainActorId);
        }

        /// <summary>获取指定人物属性(能力+资质)</summary>
        public static string GetAbilities(int actorId)
        {
            // DateFile.instance.GetActorDate 会对部分人物属性（资质）进行加成后返回
            // 因此需要从 DateFile.instance.actorsDate 直接读取以避免继承时出现错误
            // 这里通过给我们的私有方法 GetActorData 传第三个参数来达到这个目的

            var part0 = "";
            for (var i = 61; i <= 66; i++)
            {
                if (part0 != "") part0 += "|";
                part0 += GetActorData(actorId, i, true);
            }

            var part1 = "";
            for (var i = 501; i <= 516; i++)
            {
                if (part1 != "") part1 += "|";
                part1 += GetActorData(actorId, i, true);
            }

            var part2 = GetActorData(actorId, 551, true);

            var part3 = "";
            for (var i = 601; i <= 614; i++)
            {
                if (part3 != "") part3 += "|";
                part3 += GetActorData(actorId, i, true);
            }

            var part4 = GetActorData(actorId, 651, true);

            return string.Format("{0}:{1}:{2}:{3}:{4}", part0, part1, part2, part3, part4);
        }

        /// <summary>设置太吾属性(能力+资质+成长)</summary>
        public static void SetAbilities(string value)
        {
            SetAbilities(MainActorId, value);
        }

        /// <summary>设置指定人物属性(能力+资质+成长)</summary>
        public static void SetAbilities(int actorId, string value)
        {
            if (IsAbilitiesValueValid(value))
            {
                var parts = value.Split(':');

                var values0 = parts[0].Split('|');
                for (var i = 0; i < values0.Length; i++)
                {
                    SetActorData(actorId, 61 + i, values0[i], true);
                }

                var values1 = parts[1].Split('|');
                for (var i = 0; i < values1.Length; i++)
                {
                    SetActorData(actorId, 501 + i, values1[i], true);
                }

                SetActorData(actorId, 551, parts[2], true);

                var values3 = parts[3].Split('|');
                for (var i = 0; i < values3.Length; i++)
                {
                    SetActorData(actorId, 601 + i, values3[i], true);
                }

                SetActorData(actorId, 651, parts[4], true);
            }
        }

        /// <summary>检查属性(能力+资质)参数格式是否正确</summary>
        public static bool IsAbilitiesValueValid(string value)
        {
            return Regex.IsMatch(value, @"^(\d+\|){5}\d+:(\d+\|){15}\d+:\d:(\d+\|){13}\d+:\d$");
        }

        /// <summary>获取太吾特性</summary>
        public static string GetFeatures()
        {
            return GetFeatures(MainActorId);
        }

        /// <summary>获取指定人物特性</summary>
        public static string GetFeatures(int actorId)
        {
            return GetActorData(actorId, 101);
        }

        /// <summary>设置太吾特性</summary>
        public static void SetFeatures(string value)
        {
            SetFeatures(MainActorId, value);
        }

        /// <summary>设置指定人物特性</summary>
        public static void SetFeatures(int actorId, string value)
        {
            if (IsFeaturesValueValid(value))
            {
                SetActorData(actorId, 101, value, true);
                DateFile.instance.actorsFeatureCache.Remove(actorId);
            }
        }

        /// <summary>检查特性参数格式是否正确</summary>
        public static bool IsFeaturesValueValid(string value)
        {
            return Regex.IsMatch(value, @"^((\d+\|)*\d+)?$");
        }

        /// <summary>获取太吾出生时节</summary>
        public static int GetBornSolarTerm()
        {
            return GetBornSolarTerm(MainActorId);
        }

        /// <summary>获取指定人物出生时节</summary>
        public static int GetBornSolarTerm(int actorId)
        {
            return int.Parse(GetActorData(actorId, 25));
        }

        /// <summary>设置太吾出生时节</summary>
        public static void SetBornSolarTerm(int value)
        {
            SetBornSolarTerm(MainActorId, value);
        }

        /// <summary>设置指定人物出生时节</summary>
        public static void SetBornSolarTerm(int actorId, int value)
        {
            SetActorData(actorId, 25, value.ToString(), true);
        }

        /// <summary>获取所在地人物预设模板ID，创建新人物时使用。女性模板ID在此基础上+1。</summary>
        public static int GetBasePresetActorId()
        {
            return GetBasePresetActorId(MainWorldId);
        }

        /// <summary>获取指定地区人物预设模板ID，创建新人物时使用。女性模板ID在此基础上+1。</summary>
        public static int GetBasePresetActorId(int worldId)
        {
            return int.Parse(DateFile.instance.placeWorldDate[worldId + 1][87]);
        }

        private static string GetActorData(int actorId, int key, bool direct = false)
        {
            if (direct)
            {
                if (DateFile.instance.actorsDate.ContainsKey(actorId) && DateFile.instance.actorsDate[actorId].ContainsKey(key))
                {
                    return DateFile.instance.actorsDate[actorId][key];
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return DateFile.instance.GetActorDate(actorId, key, false);
            }
        }

        private static void SetActorData(int actorId, int key, string value, bool addNewKey = false)
        {
            var actorData = DateFile.instance.actorsDate[actorId];

            if (actorData.ContainsKey(key))
            {
                actorData[key] = value;
            }
            else if (addNewKey)
            {
                actorData.Add(key, value);
            }
        }

        private static void RemoveActorDataKey(int actorId, int key)
        {
            DateFile.instance.actorsDate[actorId].Remove(key);
        }
    }
}
