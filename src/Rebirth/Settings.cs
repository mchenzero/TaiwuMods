using System;
using UnityModManagerNet;

namespace Rebirth
{
    public class Settings : UnityModManager.ModSettings
    {
        public class Enums
        {
            // 性别: 保持 | 反转 | 随机
            public enum Gender
            {
                Keep = 0,
                Random = 1,
                Invert = 2
            }

            // 姓名: 保持 | 修改 | 随机
            public enum Name
            {
                Keep = 0,
                Random = 1,
                Change = 2
            }

            // 相貌: 保持 | 修改 | 随机
            public enum Appearance
            {
                Keep = 0,
                Random = 1,
                Change = 2
            }

            // 立场: 保持 | 随机
            public enum Goodness
            {
                Keep = 0,
                Random = 1
            }

            // 属性: 继承 | 随机
            public enum Abilities
            {
                Inherit = 0,
                Random = 1
            }

            // 特性: 继承 | 随机
            public enum Features
            {
                Inherit = 0,
                Random = 1
            }
        }

        public Enums.Gender Gender = Enums.Gender.Keep;
        public Enums.Name Name = Enums.Name.Keep;
        public string SurnameMale = "";
        public string ForenameMale = "";
        public string SurnameFemale = "";
        public string ForenameFemale = "";
        public Enums.Appearance Appearance = Enums.Appearance.Keep;
        public string AppearanceMale = "";
        public string AppearanceFemale = "";
        public Enums.Goodness Goodness = Enums.Goodness.Keep;
        public Enums.Abilities Abilities = Enums.Abilities.Inherit;
        public Enums.Features Features = Enums.Features.Inherit;

        public override void Save(UnityModManager.ModEntry modEntry)
        {
            UnityModManager.ModSettings.Save<Settings>(this, modEntry);
        }
    }
}
