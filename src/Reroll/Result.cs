using System;
using System.Collections.Generic;

namespace Reroll
{
    public class Result
    {
        public bool IsMatch = false;
        public OptionsMatchResult Gender = new OptionsMatchResult();
        public OptionsMatchResult Goodness = new OptionsMatchResult();
        public AgeMatchResult Age = new AgeMatchResult();
        public MinMatchResult MaxLife = new MinMatchResult();
        public MinMatchResult Charm = new MinMatchResult();
        public FeaturesMatchResult Features = new FeaturesMatchResult();
        public AbilitiesMatchResult Abilities = new AbilitiesMatchResult();
        public SkillsMatchResult Skills = new SkillsMatchResult();
        public GestMatchResult Gest = new GestMatchResult();

        public void LogFailure()
        {
            if (IsMatch) return;

            var logLines = new List<string>();

            if (!Gender.EverMatched)
            {
                logLines.Add("    性别无匹配");
            }

            if (!Goodness.EverMatched)
            {
                logLines.Add("    立场无匹配");
            }

            if (!Age.Min.EverMatched)
            {
                logLines.Add(string.Format("    最小年龄无匹配（最佳：{0}）", Age.Min.BestMatch));
            }

            if (!Age.Max.EverMatched)
            {
                logLines.Add(string.Format("    最大年龄无匹配（最佳：{0}）", Age.Max.BestMatch));
            }

            if (!MaxLife.EverMatched)
            {
                logLines.Add(string.Format("    寿命无匹配（最佳：{0}）", MaxLife.BestMatch));
            }

            if (!Charm.EverMatched)
            {
                logLines.Add(string.Format("    魅力无匹配（最佳：{0}）", Charm.BestMatch));
            }

            if (!Features.EverMatched)
            {
                switch (Features.BestMatches.Count)
                {
                    case 0:
                        logLines.Add("    特性无匹配");
                        break;
                    case 1:
                        logLines.Add(string.Format("    特性无匹配（最佳：{0}）", Features.BestMatches[0].Features));
                        break;
                    default:
                        logLines.Add("    特性无匹配，最佳：");
                        for (int i = 0; i < Features.BestMatches.Count; i++)
                        {
                            if (i < 3)
                            {
                                logLines.Add(string.Format("        {0}", Features.BestMatches[i].Features));
                            }
                            else
                            {
                                logLines.Add("        ......");
                                break;
                            }

                        }
                        break;
                }
            }

            if (!Skills.Maturity.EverMatched)
            {
                logLines.Add("    技艺成长无匹配");
            }

            if (!Skills.YinLv.EverMatched)
            {
                logLines.Add(string.Format("    音律资质无匹配（最佳：{0}）", Skills.YinLv.BestMatch));
            }

            if (!Skills.YiQi.EverMatched)
            {
                logLines.Add(string.Format("    弈棋资质无匹配（最佳：{0}）", Skills.YiQi.BestMatch));
            }

            if (!Skills.ShiShu.EverMatched)
            {
                logLines.Add(string.Format("    诗书资质无匹配（最佳：{0}）", Skills.ShiShu.BestMatch));
            }

            if (!Skills.HuiHua.EverMatched)
            {
                logLines.Add(string.Format("    绘画资质无匹配（最佳：{0}）", Skills.HuiHua.BestMatch));
            }

            if (!Skills.ShuShu.EverMatched)
            {
                logLines.Add(string.Format("    术数资质无匹配（最佳：{0}）", Skills.ShuShu.BestMatch));
            }

            if (!Skills.PinJian.EverMatched)
            {
                logLines.Add(string.Format("    品鉴资质无匹配（最佳：{0}）", Skills.PinJian.BestMatch));
            }

            if (!Skills.DuanZao.EverMatched)
            {
                logLines.Add(string.Format("    锻造资质无匹配（最佳：{0}）", Skills.DuanZao.BestMatch));
            }

            if (!Skills.ZhiMu.EverMatched)
            {
                logLines.Add(string.Format("    制木资质无匹配（最佳：{0}）", Skills.ZhiMu.BestMatch));
            }

            if (!Skills.YiShu.EverMatched)
            {
                logLines.Add(string.Format("    医术资质无匹配（最佳：{0}）", Skills.YiShu.BestMatch));
            }

            if (!Skills.DuShu.EverMatched)
            {
                logLines.Add(string.Format("    毒术资质无匹配（最佳：{0}）", Skills.DuShu.BestMatch));
            }

            if (!Skills.ZhiJin.EverMatched)
            {
                logLines.Add(string.Format("    织锦资质无匹配（最佳：{0}）", Skills.ZhiJin.BestMatch));
            }

            if (!Skills.QiaoJiang.EverMatched)
            {
                logLines.Add(string.Format("    巧匠资质无匹配（最佳：{0}）", Skills.QiaoJiang.BestMatch));
            }

            if (!Skills.DaoFa.EverMatched)
            {
                logLines.Add(string.Format("    道法资质无匹配（最佳：{0}）", Skills.DaoFa.BestMatch));
            }

            if (!Skills.FoXue.EverMatched)
            {
                logLines.Add(string.Format("    佛学资质无匹配（最佳：{0}）", Skills.FoXue.BestMatch));
            }

            if (!Skills.ChuYi.EverMatched)
            {
                logLines.Add(string.Format("    厨艺资质无匹配（最佳：{0}）", Skills.ChuYi.BestMatch));
            }

            if (!Skills.ZaXue.EverMatched)
            {
                logLines.Add(string.Format("    杂学资质无匹配（最佳：{0}）", Skills.ZaXue.BestMatch));
            }

            if (!Gest.Maturity.EverMatched)
            {
                logLines.Add("    功法成长无匹配");
            }

            if (!Gest.NeiGong.EverMatched)
            {
                logLines.Add(string.Format("    内功资质无匹配（最佳：{0}）", Gest.NeiGong.BestMatch));
            }

            if (!Gest.ShenFa.EverMatched)
            {
                logLines.Add(string.Format("    身法资质无匹配（最佳：{0}）", Gest.ShenFa.BestMatch));
            }

            if (!Gest.JueJi.EverMatched)
            {
                logLines.Add(string.Format("    绝技资质无匹配（最佳：{0}）", Gest.JueJi.BestMatch));
            }

            if (!Gest.QuanZhang.EverMatched)
            {
                logLines.Add(string.Format("    拳掌资质无匹配（最佳：{0}）", Gest.QuanZhang.BestMatch));
            }

            if (!Gest.ZhiFa.EverMatched)
            {
                logLines.Add(string.Format("    指法资质无匹配（最佳：{0}）", Gest.ZhiFa.BestMatch));
            }

            if (!Gest.TuiFa.EverMatched)
            {
                logLines.Add(string.Format("    腿法资质无匹配（最佳：{0}）", Gest.TuiFa.BestMatch));
            }

            if (!Gest.AnQi.EverMatched)
            {
                logLines.Add(string.Format("    暗器资质无匹配（最佳：{0}）", Gest.AnQi.BestMatch));
            }

            if (!Gest.JianFa.EverMatched)
            {
                logLines.Add(string.Format("    剑法资质无匹配（最佳：{0}）", Gest.JianFa.BestMatch));
            }

            if (!Gest.DaoFa.EverMatched)
            {
                logLines.Add(string.Format("    刀法资质无匹配（最佳：{0}）", Gest.DaoFa.BestMatch));
            }

            if (!Gest.ChangBing.EverMatched)
            {
                logLines.Add(string.Format("    长兵资质无匹配（最佳：{0}）", Gest.ChangBing.BestMatch));
            }

            if (!Gest.QiMen.EverMatched)
            {
                logLines.Add(string.Format("    奇门资质无匹配（最佳：{0}）", Gest.QiMen.BestMatch));
            }

            if (!Gest.RuanBing.EverMatched)
            {
                logLines.Add(string.Format("    软兵资质无匹配（最佳：{0}）", Gest.RuanBing.BestMatch));
            }

            if (!Gest.YuShe.EverMatched)
            {
                logLines.Add(string.Format("    御射资质无匹配（最佳：{0}）", Gest.YuShe.BestMatch));
            }

            if (!Gest.YueQi.EverMatched)
            {
                logLines.Add(string.Format("    乐器资质无匹配（最佳：{0}）", Gest.YueQi.BestMatch));
            }

            if (logLines.Count == 0)
            {
                Main.Logger.Log("未找到同时满足所有条件的角色！可尝试增加重掷次数。");
            }
            else if (logLines.Count == 1)
            {
                Main.Logger.Log("未找到满足条件的角色！" + logLines[0].Trim());
            }
            else
            {
                Main.Logger.Log("未找到满足条件的角色！\n" + string.Join("\n", logLines.ToArray()));
            }
        }

        public class OptionsMatchResult
        {
            public bool EverMatched = false;
        }

        public class MinMatchResult
        {
            public bool EverMatched = false;
            public int BestMatch = 0;
        }

        public class MaxMatchResult
        {
            public bool EverMatched = false;
            public int BestMatch = int.MaxValue;
        }

        public class AgeMatchResult
        {
            public MinMatchResult Min = new MinMatchResult();
            public MaxMatchResult Max = new MaxMatchResult();
        }

        public class FeaturesMatchResult
        {
            public bool EverMatched = false;
            public List<SingleMatch> BestMatches = new List<SingleMatch>();

            public void TrackBestMatch(string features, int matchCount)
            {
                if (matchCount > 0)
                {
                    BestMatches.Add(new SingleMatch() { Features = features, MatchCount = matchCount });
                    SortBestMatches();
                }
            }

            public void SortBestMatches()
            {
                BestMatches.Sort((m1, m2) => m2.MatchCount.CompareTo(m1.MatchCount));
            }

            public class SingleMatch
            {
                public string Features = "";
                public int MatchCount = 0;
            }
        }

        public class AbilitiesMatchResult
        {
            public MinMatchResult BiLi = new MinMatchResult();
            public MinMatchResult TiZhi = new MinMatchResult();
            public MinMatchResult LingMin = new MinMatchResult();
            public MinMatchResult GenGu = new MinMatchResult();
            public MinMatchResult WuXing = new MinMatchResult();
            public MinMatchResult DingLi = new MinMatchResult();
        }

        public class SkillsMatchResult
        {
            public OptionsMatchResult Maturity = new OptionsMatchResult();
            public MinMatchResult YinLv = new MinMatchResult();
            public MinMatchResult YiQi = new MinMatchResult();
            public MinMatchResult ShiShu = new MinMatchResult();
            public MinMatchResult HuiHua = new MinMatchResult();
            public MinMatchResult ShuShu = new MinMatchResult();
            public MinMatchResult PinJian = new MinMatchResult();
            public MinMatchResult DuanZao = new MinMatchResult();
            public MinMatchResult ZhiMu = new MinMatchResult();
            public MinMatchResult YiShu = new MinMatchResult();
            public MinMatchResult DuShu = new MinMatchResult();
            public MinMatchResult ZhiJin = new MinMatchResult();
            public MinMatchResult QiaoJiang = new MinMatchResult();
            public MinMatchResult DaoFa = new MinMatchResult();
            public MinMatchResult FoXue = new MinMatchResult();
            public MinMatchResult ZaXue = new MinMatchResult();
            public MinMatchResult ChuYi = new MinMatchResult();
        }

        public class GestMatchResult
        {
            public OptionsMatchResult Maturity = new OptionsMatchResult();
            public MinMatchResult AnQi = new MinMatchResult();
            public MinMatchResult JianFa = new MinMatchResult();
            public MinMatchResult DaoFa = new MinMatchResult();
            public MinMatchResult ChangBing = new MinMatchResult();
            public MinMatchResult QiMen = new MinMatchResult();
            public MinMatchResult RuanBing = new MinMatchResult();
            public MinMatchResult YuShe = new MinMatchResult();
            public MinMatchResult YueQi = new MinMatchResult();
            public MinMatchResult TuiFa = new MinMatchResult();
            public MinMatchResult ZhiFa = new MinMatchResult();
            public MinMatchResult QuanZhang = new MinMatchResult();
            public MinMatchResult NeiGong = new MinMatchResult();
            public MinMatchResult ShenFa = new MinMatchResult();
            public MinMatchResult JueJi = new MinMatchResult();
        }
    }
}
