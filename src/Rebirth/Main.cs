using System;
using UnityEngine;
using UnityModManagerNet;

namespace Rebirth
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

            Patch.LoadAll();

            return true;
        }

        public static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            Enabled = value;
            return true;
        }

        public static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            var infoLabelStyle = new GUIStyle(GUI.skin.GetStyle("label"));
            infoLabelStyle.normal.textColor = new Color(0.5f, 0.5f, 0.5f, 1f);

            var errorLabelStyle = new GUIStyle(GUI.skin.GetStyle("label"));
            errorLabelStyle.normal.textColor = new Color(1f, 0.25f, 0.25f, 1f);

            GUILayout.BeginVertical("Box");

            GUILayout.BeginHorizontal();
            GUILayout.Label("性别：", GUILayout.Width(40));
            Settings.Gender = (Settings.Enums.Gender)GUILayout.SelectionGrid((int)Settings.Gender, new string[] { "保持", "随机", "反转" }, 3, GUI.skin.toggle, GUILayout.Width(150));
            GUILayout.Space(10);
            switch (Settings.Gender)
            {
                case Settings.Enums.Gender.Keep:
                    GUILayout.Label("重生时性别不变", infoLabelStyle);
                    break;
                case Settings.Enums.Gender.Random:
                    GUILayout.Label("重生时性别随机", infoLabelStyle);
                    break;
                case Settings.Enums.Gender.Invert:
                    GUILayout.Label("重生时转为异性", infoLabelStyle);
                    break;
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("姓名：", GUILayout.Width(40));
            Settings.Name = (Settings.Enums.Name)GUILayout.SelectionGrid((int)Settings.Name, new string[] { "保持", "随机", "修改" }, 3, GUI.skin.toggle, GUILayout.Width(150));
            GUILayout.Space(10);
            switch (Settings.Name)
            {
                case Settings.Enums.Name.Keep:
                    GUILayout.Label("重生时姓名不变", infoLabelStyle);
                    break;
                case Settings.Enums.Name.Random:
                    GUILayout.Label("重生时姓名随机", infoLabelStyle);
                    break;
                case Settings.Enums.Name.Change:
                    GUILayout.Label("重生时修改姓名", infoLabelStyle);
                    break;
            }
            GUILayout.EndHorizontal();
            if (Settings.Name == Settings.Enums.Name.Change)
            {
                GUILayout.BeginHorizontal("Box");
                GUILayout.Label("男性：", GUILayout.Width(40));
                Settings.SurnameMale = GUILayout.TextField(Settings.SurnameMale, GUILayout.Width(46));
                Settings.ForenameMale = GUILayout.TextField(Settings.ForenameMale, GUILayout.Width(46));
                GUILayout.Space(10);
                GUILayout.Label("女性：", GUILayout.Width(40));
                Settings.SurnameFemale = GUILayout.TextField(Settings.SurnameFemale, GUILayout.Width(46));
                Settings.ForenameFemale = GUILayout.TextField(Settings.ForenameFemale, GUILayout.Width(46));
                GUILayout.EndHorizontal();
            }

            GUILayout.BeginHorizontal();
            GUILayout.Label("相貌：", GUILayout.Width(40));
            Settings.Appearance = (Settings.Enums.Appearance)GUILayout.SelectionGrid((int)Settings.Appearance, new string[] { "保持", "随机", "修改" }, 3, GUI.skin.toggle, GUILayout.Width(150));
            GUILayout.Space(10);
            switch (Settings.Appearance)
            {
                case Settings.Enums.Appearance.Keep:
                    GUILayout.Label("重生时相貌不变", infoLabelStyle);
                    break;
                case Settings.Enums.Appearance.Random:
                    GUILayout.Label("重生时相貌随机", infoLabelStyle);
                    break;
                case Settings.Enums.Appearance.Change:
                    GUILayout.Label("重生时修改相貌", infoLabelStyle);
                    break;
            }
            GUILayout.EndHorizontal();
            if (Settings.Appearance == Settings.Enums.Appearance.Change)
            {
                GUILayout.BeginVertical("Box");
                GUILayout.BeginHorizontal();
                GUILayout.Label("男性：", GUILayout.Width(40));
                Settings.AppearanceMale = GUILayout.TextField(Settings.AppearanceMale, GUILayout.Width(250));
                GUILayout.Space(10);
                if (GUILayout.Button("获取当前", GUILayout.Width(70)) && Taiwu.IsInGame())
                {
                    Settings.AppearanceMale = Taiwu.GetAppearance();
                }
                if (Settings.AppearanceMale != "" && !Taiwu.IsAppearanceValueValid(Settings.AppearanceMale))
                {
                    GUILayout.Space(9);
                    GUILayout.Label("格式错误", errorLabelStyle, GUILayout.Width(52));
                }
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                GUILayout.Label("女性：", GUILayout.Width(40));
                Settings.AppearanceFemale = GUILayout.TextField(Settings.AppearanceFemale, GUILayout.Width(250));
                GUILayout.Space(10);
                if (GUILayout.Button("获取当前", GUILayout.Width(70)) && Taiwu.IsInGame())
                {
                    Settings.AppearanceFemale = Taiwu.GetAppearance();
                }
                if (Settings.AppearanceFemale != "" && !Taiwu.IsAppearanceValueValid(Settings.AppearanceFemale))
                {
                    GUILayout.Space(9);
                    GUILayout.Label("格式错误", errorLabelStyle, GUILayout.Width(52));
                }
                GUILayout.EndHorizontal();
                GUILayout.EndVertical();
            }

            GUILayout.BeginHorizontal();
            GUILayout.Label("立场：", GUILayout.Width(40));
            Settings.Goodness = (Settings.Enums.Goodness)GUILayout.SelectionGrid((int)Settings.Goodness, new string[] { "保持", "随机" }, 3, GUI.skin.toggle, GUILayout.Width(150));
            GUILayout.Space(10);
            switch (Settings.Goodness)
            {
                case Settings.Enums.Goodness.Keep:
                    GUILayout.Label("重生时立场不变", infoLabelStyle);
                    break;
                case Settings.Enums.Goodness.Random:
                    GUILayout.Label("重生时立场随机", infoLabelStyle);
                    break;
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("属性：", GUILayout.Width(40));
            Settings.Abilities = (Settings.Enums.Abilities)GUILayout.SelectionGrid((int)Settings.Abilities, new string[] { "继承", "随机" }, 3, GUI.skin.toggle, GUILayout.Width(150));
            GUILayout.Space(10);
            switch (Settings.Abilities)
            {
                case Settings.Enums.Abilities.Inherit:
                    GUILayout.Label("重生时继承能力和资质", infoLabelStyle);
                    break;
                case Settings.Enums.Abilities.Random:
                    GUILayout.Label("重生时能力和资质随机", infoLabelStyle);
                    break;
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("特性：", GUILayout.Width(40));
            Settings.Features = (Settings.Enums.Features)GUILayout.SelectionGrid((int)Settings.Features, new string[] { "继承", "随机" }, 3, GUI.skin.toggle, GUILayout.Width(150));
            GUILayout.Space(10);
            switch (Settings.Features)
            {
                case Settings.Enums.Features.Inherit:
                    GUILayout.Label("重生时继承特性", infoLabelStyle);
                    break;
                case Settings.Enums.Features.Random:
                    GUILayout.Label("重生时特性随机", infoLabelStyle);
                    break;
            }
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
        }

        public static void OnSaveGUI(UnityModManager.ModEntry modEntry)
        {
            Settings.Save(modEntry);
        }
    }
}
