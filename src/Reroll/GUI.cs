using System;
using UnityEngine;

namespace Reroll
{
    public static class GUI
    {
        public static int SelectedTab = 0;

        public static void Render()
        {
            var infoLabelStyle = new GUIStyle(UnityEngine.GUI.skin.GetStyle("label"));
            infoLabelStyle.normal.textColor = new Color(0.5f, 0.5f, 0.5f, 1f);

            GUILayout.BeginVertical("Box");

            SelectedTab = GUILayout.Toolbar(SelectedTab, new string[] { "创建人物", "龙岛忠仆", "随即继承人" });

            Filters filters;

            switch (SelectedTab)
            {
                case 0:
                    filters = Main.Settings.NewGame;
                    break;
                case 1:
                    filters = Main.Settings.LongDao;
                    break;
                case 2:
                    filters = Main.Settings.RandomHeir;
                    break;
                default:
                    throw new Exception(string.Format("Unknown selected tab {0}", SelectedTab));
            }

            if (SelectedTab == 0)
            {
                UnityEngine.GUI.enabled = false;
                filters.Gender.Male = true;
                filters.Gender.Female = true;
                filters.Goodness.Upright = true;
                filters.Goodness.Kind = true;
                filters.Goodness.Moderate = true;
                filters.Goodness.Rebellious = true;
                filters.Goodness.Selfish = true;
                filters.Age.Min = 0;
                filters.Age.Max = 100;
            }

            GUILayout.BeginHorizontal();
            GUILayout.Label("性别：", GUILayout.Width(40));
            filters.Gender.Male = GUILayout.Toggle(filters.Gender.Male, "男", GUILayout.Width(45));
            filters.Gender.Female = GUILayout.Toggle(filters.Gender.Female, "女", GUILayout.Width(45));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("立场：", GUILayout.Width(40));
            filters.Goodness.Upright = GUILayout.Toggle(filters.Goodness.Upright, "刚正", GUILayout.Width(45));
            filters.Goodness.Kind = GUILayout.Toggle(filters.Goodness.Kind, "仁善", GUILayout.Width(45));
            filters.Goodness.Moderate = GUILayout.Toggle(filters.Goodness.Moderate, "中庸", GUILayout.Width(45));
            filters.Goodness.Rebellious = GUILayout.Toggle(filters.Goodness.Rebellious, "叛逆", GUILayout.Width(45));
            filters.Goodness.Selfish = GUILayout.Toggle(filters.Goodness.Selfish, "唯我", GUILayout.Width(45));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("年龄：", GUILayout.Width(40));
            filters.Age.Min = TryParseInt(GUILayout.TextField(filters.Age.Min.ToString(), GUILayout.Width(30)));
            GUILayout.Space(5);
            GUILayout.Label("-", GUILayout.Width(8));
            filters.Age.Max = TryParseInt(GUILayout.TextField(filters.Age.Max.ToString(), GUILayout.Width(30)), 100);
            GUILayout.EndHorizontal();

            UnityEngine.GUI.enabled = true;

            GUILayout.BeginHorizontal();
            GUILayout.Label("寿命：", GUILayout.Width(40));
            filters.MaxLife = TryParseInt(GUILayout.TextField(filters.MaxLife.ToString(), GUILayout.Width(30)));
            GUILayout.Space(5);
            GUILayout.Label("注：除年龄外其他数值设定均为下限", infoLabelStyle);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("魅力：", GUILayout.Width(40));
            filters.Charm = TryParseInt(GUILayout.TextField(filters.Charm.ToString(), GUILayout.Width(30)));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("特性：", GUILayout.Width(40));
            filters.Features = GUILayout.TextField(filters.Features, GUILayout.Width(200));
            GUILayout.Space(5);
            GUILayout.Label("示例：灵心慧性|沉稳果决（慎用）", infoLabelStyle);
            GUILayout.EndHorizontal();

            GUILayout.Space(10);
            GUILayout.Label("属性");
            GUILayout.BeginVertical("Box");
            GUILayout.BeginHorizontal();
            GUILayout.Label("臂力：", GUILayout.Width(40));
            filters.Abilities.BiLi = TryParseInt(GUILayout.TextField(filters.Abilities.BiLi.ToString(), GUILayout.Width(30)));
            GUILayout.Space(15);
            GUILayout.Label("体质：", GUILayout.Width(40));
            filters.Abilities.TiZhi = TryParseInt(GUILayout.TextField(filters.Abilities.TiZhi.ToString(), GUILayout.Width(30)));
            GUILayout.Space(15);
            GUILayout.Label("灵敏：", GUILayout.Width(40));
            filters.Abilities.LingMin = TryParseInt(GUILayout.TextField(filters.Abilities.LingMin.ToString(), GUILayout.Width(30)));
            GUILayout.Space(15);
            GUILayout.Label("根骨：", GUILayout.Width(40));
            filters.Abilities.GenGu = TryParseInt(GUILayout.TextField(filters.Abilities.GenGu.ToString(), GUILayout.Width(30)));
            GUILayout.Space(15);
            GUILayout.Label("悟性：", GUILayout.Width(40));
            filters.Abilities.WuXing = TryParseInt(GUILayout.TextField(filters.Abilities.WuXing.ToString(), GUILayout.Width(30)));
            GUILayout.Space(15);
            GUILayout.Label("定力：", GUILayout.Width(40));
            filters.Abilities.DingLi = TryParseInt(GUILayout.TextField(filters.Abilities.DingLi.ToString(), GUILayout.Width(30)));
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            GUILayout.Space(10);

            GUILayout.Label("技艺");
            GUILayout.BeginVertical("Box");
            GUILayout.BeginHorizontal();
            GUILayout.Label("成长：", GUILayout.Width(40));
            filters.Skills.Maturity.Early = GUILayout.Toggle(filters.Skills.Maturity.Early, "早熟", GUILayout.Width(45));
            filters.Skills.Maturity.Normal = GUILayout.Toggle(filters.Skills.Maturity.Normal, "均衡", GUILayout.Width(45));
            filters.Skills.Maturity.Late = GUILayout.Toggle(filters.Skills.Maturity.Late, "晚成", GUILayout.Width(45));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("音律：", GUILayout.Width(40));
            filters.Skills.YinLv = TryParseInt(GUILayout.TextField(filters.Skills.YinLv.ToString(), GUILayout.Width(30)));
            GUILayout.Space(15);
            GUILayout.Label("弈棋：", GUILayout.Width(40));
            filters.Skills.YiQi = TryParseInt(GUILayout.TextField(filters.Skills.YiQi.ToString(), GUILayout.Width(30)));
            GUILayout.Space(15);
            GUILayout.Label("诗书：", GUILayout.Width(40));
            filters.Skills.ShiShu = TryParseInt(GUILayout.TextField(filters.Skills.ShiShu.ToString(), GUILayout.Width(30)));
            GUILayout.Space(15);
            GUILayout.Label("绘画：", GUILayout.Width(40));
            filters.Skills.HuiHua = TryParseInt(GUILayout.TextField(filters.Skills.HuiHua.ToString(), GUILayout.Width(30)));
            GUILayout.Space(15);
            GUILayout.Label("术数：", GUILayout.Width(40));
            filters.Skills.ShuShu = TryParseInt(GUILayout.TextField(filters.Skills.ShuShu.ToString(), GUILayout.Width(30)));
            GUILayout.Space(15);
            GUILayout.Label("品鉴：", GUILayout.Width(40));
            filters.Skills.PinJian = TryParseInt(GUILayout.TextField(filters.Skills.PinJian.ToString(), GUILayout.Width(30)));
            GUILayout.Space(15);
            GUILayout.Label("锻造：", GUILayout.Width(40));
            filters.Skills.DuanZao = TryParseInt(GUILayout.TextField(filters.Skills.DuanZao.ToString(), GUILayout.Width(30)));
            GUILayout.Space(15);
            GUILayout.Label("制木：", GUILayout.Width(40));
            filters.Skills.ZhiMu = TryParseInt(GUILayout.TextField(filters.Skills.ZhiMu.ToString(), GUILayout.Width(30)));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("医术：", GUILayout.Width(40));
            filters.Skills.YiShu = TryParseInt(GUILayout.TextField(filters.Skills.YiShu.ToString(), GUILayout.Width(30)));
            GUILayout.Space(15);
            GUILayout.Label("毒术：", GUILayout.Width(40));
            filters.Skills.DuShu = TryParseInt(GUILayout.TextField(filters.Skills.DuShu.ToString(), GUILayout.Width(30)));
            GUILayout.Space(15);
            GUILayout.Label("织锦：", GUILayout.Width(40));
            filters.Skills.ZhiJin = TryParseInt(GUILayout.TextField(filters.Skills.ZhiJin.ToString(), GUILayout.Width(30)));
            GUILayout.Space(15);
            GUILayout.Label("巧匠：", GUILayout.Width(40));
            filters.Skills.QiaoJiang = TryParseInt(GUILayout.TextField(filters.Skills.QiaoJiang.ToString(), GUILayout.Width(30)));
            GUILayout.Space(15);
            GUILayout.Label("道法：", GUILayout.Width(40));
            filters.Skills.DaoFa = TryParseInt(GUILayout.TextField(filters.Skills.DaoFa.ToString(), GUILayout.Width(30)));
            GUILayout.Space(15);
            GUILayout.Label("佛学：", GUILayout.Width(40));
            filters.Skills.FoXue = TryParseInt(GUILayout.TextField(filters.Skills.FoXue.ToString(), GUILayout.Width(30)));
            GUILayout.Space(15);
            GUILayout.Label("厨艺：", GUILayout.Width(40));
            filters.Skills.ChuYi = TryParseInt(GUILayout.TextField(filters.Skills.ChuYi.ToString(), GUILayout.Width(30)));
            GUILayout.Space(15);
            GUILayout.Label("杂学：", GUILayout.Width(40));
            filters.Skills.ZaXue = TryParseInt(GUILayout.TextField(filters.Skills.ZaXue.ToString(), GUILayout.Width(30)));
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            GUILayout.Space(10);

            GUILayout.Label("功法");
            GUILayout.BeginVertical("Box");
            GUILayout.BeginHorizontal();
            GUILayout.Label("成长：", GUILayout.Width(40));
            filters.Gest.Maturity.Early = GUILayout.Toggle(filters.Gest.Maturity.Early, "早熟", GUILayout.Width(45));
            filters.Gest.Maturity.Normal = GUILayout.Toggle(filters.Gest.Maturity.Normal, "均衡", GUILayout.Width(45));
            filters.Gest.Maturity.Late = GUILayout.Toggle(filters.Gest.Maturity.Late, "晚成", GUILayout.Width(45));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("内功：", GUILayout.Width(40));
            filters.Gest.NeiGong = TryParseInt(GUILayout.TextField(filters.Gest.NeiGong.ToString(), GUILayout.Width(30)));
            GUILayout.Space(15);
            GUILayout.Label("身法：", GUILayout.Width(40));
            filters.Gest.ShenFa = TryParseInt(GUILayout.TextField(filters.Gest.ShenFa.ToString(), GUILayout.Width(30)));
            GUILayout.Space(15);
            GUILayout.Label("绝技：", GUILayout.Width(40));
            filters.Gest.JueJi = TryParseInt(GUILayout.TextField(filters.Gest.JueJi.ToString(), GUILayout.Width(30)));
            GUILayout.Space(15);
            GUILayout.Label("拳掌：", GUILayout.Width(40));
            filters.Gest.QuanZhang = TryParseInt(GUILayout.TextField(filters.Gest.QuanZhang.ToString(), GUILayout.Width(30)));
            GUILayout.Space(15);
            GUILayout.Label("指法：", GUILayout.Width(40));
            filters.Gest.ZhiFa = TryParseInt(GUILayout.TextField(filters.Gest.ZhiFa.ToString(), GUILayout.Width(30)));
            GUILayout.Space(15);
            GUILayout.Label("腿法：", GUILayout.Width(40));
            filters.Gest.TuiFa = TryParseInt(GUILayout.TextField(filters.Gest.TuiFa.ToString(), GUILayout.Width(30)));
            GUILayout.Space(15);
            GUILayout.Label("暗器：", GUILayout.Width(40));
            filters.Gest.AnQi = TryParseInt(GUILayout.TextField(filters.Gest.AnQi.ToString(), GUILayout.Width(30)));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("剑法：", GUILayout.Width(40));
            filters.Gest.JianFa = TryParseInt(GUILayout.TextField(filters.Gest.JianFa.ToString(), GUILayout.Width(30)));
            GUILayout.Space(15);
            GUILayout.Label("刀法：", GUILayout.Width(40));
            filters.Gest.DaoFa = TryParseInt(GUILayout.TextField(filters.Gest.DaoFa.ToString(), GUILayout.Width(30)));
            GUILayout.Space(15);
            GUILayout.Label("长兵：", GUILayout.Width(40));
            filters.Gest.ChangBing = TryParseInt(GUILayout.TextField(filters.Gest.ChangBing.ToString(), GUILayout.Width(30)));
            GUILayout.Space(15);
            GUILayout.Label("奇门：", GUILayout.Width(40));
            filters.Gest.QiMen = TryParseInt(GUILayout.TextField(filters.Gest.QiMen.ToString(), GUILayout.Width(30)));
            GUILayout.Space(15);
            GUILayout.Label("软兵：", GUILayout.Width(40));
            filters.Gest.RuanBing = TryParseInt(GUILayout.TextField(filters.Gest.RuanBing.ToString(), GUILayout.Width(30)));
            GUILayout.Space(15);
            GUILayout.Label("御射：", GUILayout.Width(40));
            filters.Gest.YuShe = TryParseInt(GUILayout.TextField(filters.Gest.YuShe.ToString(), GUILayout.Width(30)));
            GUILayout.Space(15);
            GUILayout.Label("乐器：", GUILayout.Width(40));
            filters.Gest.YueQi = TryParseInt(GUILayout.TextField(filters.Gest.YueQi.ToString(), GUILayout.Width(30)));
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            GUILayout.Label("最大重掷次数：", GUILayout.Width(95));
            Main.Settings.MaxRoll = TryParseInt(GUILayout.TextField(Main.Settings.MaxRoll.ToString(), GUILayout.Width(60)), 1000);
            GUILayout.EndHorizontal();
            GUILayout.Label("注：筛选条件勿设置过多，并注意合理范围！超过最大重掷次数仍未满足条件时，可查看Mod日志了解详情。", infoLabelStyle);

            GUILayout.EndVertical();
        }

        private static int TryParseInt(string s, int d = 0, int min = 0, int max = int.MaxValue)
        {
            int i;

            if (int.TryParse(s, out i))
            {
                if (i < min)
                {
                    return min;
                }
                else if (i > max)
                {
                    return max;
                }
                else
                {
                    return i;
                }
            }
            else
            {
                return d;
            }
        }
    }
}
