using System;
using System.Linq;

namespace Reroll
{
    public static class Taiwu
    {
        public static int MainActorId
        {
            get { return DateFile.instance.mianActorId; }
        }

        /// <summary>删除角色数据（仅保证能角色在刚建立时的数据被删除）</summary>
        public static void RemoveActor(int actorId)
        {
            DateFile.instance.actorsDate.Remove(actorId);
            DateFile.instance.actorLife.Remove(actorId);
            DateFile.instance.actorInjuryDate.Remove(actorId);
            DateFile.instance.actorStudyDate.Remove(actorId);
            DateFile.instance.actorGongFas.Remove(actorId);
            DateFile.instance.actorEquipGongFas.Remove(actorId);
            DateFile.instance.actorItemsDate.Remove(actorId);
            DateFile.instance.actorsFeatureCache.Remove(actorId);
        }

        public static Result Test(int actorId, Filters filters, Result result = null)
        {
            if (result == null)
            {
                result = new Result();
            }

            var isMatch = true;

            isMatch &= TestGender(actorId, filters, result);
            isMatch &= TestGoodness(actorId, filters, result);
            isMatch &= TestAge(actorId, filters, result);
            isMatch &= TestMaxLife(actorId, filters, result);
            isMatch &= TestCharm(actorId, filters, result);
            isMatch &= TestFeatures(actorId, filters, result);

            isMatch &= TestAbilityBili(actorId, filters, result);
            isMatch &= TestAbilityTiZhi(actorId, filters, result);
            isMatch &= TestAbilityLingMin(actorId, filters, result);
            isMatch &= TestAbilityGenGu(actorId, filters, result);
            isMatch &= TestAbilityWuXing(actorId, filters, result);
            isMatch &= TestAbilityDingLi(actorId, filters, result);

            isMatch &= TestSkillMaturity(actorId, filters, result);
            isMatch &= TestSkillYinLv(actorId, filters, result);
            isMatch &= TestSkillYiQi(actorId, filters, result);
            isMatch &= TestSkillShiShu(actorId, filters, result);
            isMatch &= TestSkillHuiHua(actorId, filters, result);
            isMatch &= TestSkillShuShu(actorId, filters, result);
            isMatch &= TestSkillPinJian(actorId, filters, result);
            isMatch &= TestSkillDuanZao(actorId, filters, result);
            isMatch &= TestSkillZhiMu(actorId, filters, result);
            isMatch &= TestSkillYiShu(actorId, filters, result);
            isMatch &= TestSkillDuShu(actorId, filters, result);
            isMatch &= TestSkillZhiJin(actorId, filters, result);
            isMatch &= TestSkillQiaoJiang(actorId, filters, result);
            isMatch &= TestSkillDaoFa(actorId, filters, result);
            isMatch &= TestSkillFoXue(actorId, filters, result);
            isMatch &= TestSkillChuYi(actorId, filters, result);
            isMatch &= TestSkillZaXue(actorId, filters, result);

            isMatch &= TestGestMaturity(actorId, filters, result);
            isMatch &= TestGestNeiGong(actorId, filters, result);
            isMatch &= TestGestShenFa(actorId, filters, result);
            isMatch &= TestGestJueJi(actorId, filters, result);
            isMatch &= TestGestQuanZhang(actorId, filters, result);
            isMatch &= TestGestZhiFa(actorId, filters, result);
            isMatch &= TestGestTuiFa(actorId, filters, result);
            isMatch &= TestGestAnQi(actorId, filters, result);
            isMatch &= TestGestJianFa(actorId, filters, result);
            isMatch &= TestGestDaoFa(actorId, filters, result);
            isMatch &= TestGestChangBing(actorId, filters, result);
            isMatch &= TestGestQiMen(actorId, filters, result);
            isMatch &= TestGestRuanBing(actorId, filters, result);
            isMatch &= TestGestYuShe(actorId, filters, result);
            isMatch &= TestGestYueQi(actorId, filters, result);

            result.IsMatch = isMatch;

            return result;
        }

        private static bool TestGender(int actorId, Filters filters, Result result)
        {
            var gender = DateFile.instance.GetActorDate(actorId, 14, false);

            var isMatch = false;

            isMatch |= (filters.Gender.Male && gender == "1");
            isMatch |= (filters.Gender.Female && gender == "2");

            if (isMatch)
            {
                result.Gender.EverMatched = true;
            }

            return isMatch;
        }

        private static bool TestGoodness(int actorId, Filters filters, Result result)
        {
            var badness = int.Parse(DateFile.instance.GetActorDate(actorId, 16, false));

            var isMatch = false;

            isMatch |= (filters.Goodness.Upright && badness < 125);
            isMatch |= (filters.Goodness.Kind && badness >= 125 && badness < 375);
            isMatch |= (filters.Goodness.Moderate && badness >= 375 && badness < 625);
            isMatch |= (filters.Goodness.Rebellious && badness >= 625 && badness < 875);
            isMatch |= (filters.Goodness.Selfish && badness >= 875);

            if (isMatch)
            {
                result.Goodness.EverMatched = true;
            }

            return isMatch;
        }

        private static bool TestAge(int actorId, Filters filters, Result result)
        {
            var age = int.Parse(DateFile.instance.GetActorDate(actorId, 11, false));

            var minAgeMatch = (age >= filters.Age.Min);
            var maxAgeMatch = (age <= filters.Age.Max);

            if (minAgeMatch)
            {
                result.Age.Min.EverMatched = true;
            }

            if (maxAgeMatch)
            {
                result.Age.Max.EverMatched = true;
            }

            if (age > result.Age.Min.BestMatch)
            {
                result.Age.Min.BestMatch = age;
            }

            if (age < result.Age.Max.BestMatch)
            {
                result.Age.Max.BestMatch = age;
            }

            return minAgeMatch && maxAgeMatch;
        }

        private static bool TestMaxLife(int actorId, Filters filters, Result result)
        {
            var maxLife = int.Parse(DateFile.instance.GetActorDate(actorId, 13, false));

            var isMatch = (maxLife >= filters.MaxLife);

            if (isMatch)
            {
                result.MaxLife.EverMatched = true;
            }

            if (maxLife > result.MaxLife.BestMatch)
            {
                result.MaxLife.BestMatch = maxLife;
            }

            return isMatch;
        }

        private static bool TestCharm(int actorId, Filters filters, Result result)
        {
            var charm = int.Parse(DateFile.instance.GetActorDate(actorId, 15, false));

            var isMatch = (charm >= filters.Charm);

            if (isMatch)
            {
                result.Charm.EverMatched = true;
            }

            if (charm > result.Charm.BestMatch)
            {
                result.Charm.BestMatch = charm;
            }

            return isMatch;
        }

        private static bool TestFeatures(int actorId, Filters filters, Result result)
        {
            var requiredFeatures = filters.Features.Split('|')
                .Select(name => name.Trim())
                .Where(name => !string.IsNullOrEmpty(name))
                .Distinct()
                .ToList();

            var actualFeatures = DateFile.instance.GetActorDate(actorId, 101, false).Split('|')
                .Select(id => int.Parse(id))
                .Where(id => id != 0 && id != 4001 && id != 4002 && id != 9997)
                .Where(id => DateFile.instance.actorFeaturesDate.ContainsKey(id))
                .Select(id => DateFile.instance.actorFeaturesDate[id][0].Trim())
                .Where(name => !string.IsNullOrEmpty(name))
                .Distinct()
                .ToList();

            var matchedFeatures = actualFeatures.Intersect(requiredFeatures).ToList();

            var isMatch = (matchedFeatures.Count == requiredFeatures.Count || requiredFeatures.Count == 0);

            if (isMatch)
            {
                result.Features.EverMatched = true;
            }

            result.Features.TrackBestMatch(string.Join("|", actualFeatures.ToArray()), matchedFeatures.Count);

            return isMatch;
        }

        private static bool TestAbilityBili(int actorId, Filters filters, Result result)
        {
            var bili = int.Parse(DateFile.instance.actorsDate[actorId][61]); // 绕过 GetActorDate 以读取基础数据

            var isMatch = (bili >= filters.Abilities.BiLi);

            if (isMatch)
            {
                result.Abilities.BiLi.EverMatched = true;
            }

            if (bili > result.Abilities.BiLi.BestMatch)
            {
                result.Abilities.BiLi.BestMatch = bili;
            }

            return isMatch;
        }

        private static bool TestAbilityTiZhi(int actorId, Filters filters, Result result)
        {
            var tizhi = int.Parse(DateFile.instance.actorsDate[actorId][62]); // 绕过 GetActorDate 以读取基础数据

            var isMatch = (tizhi >= filters.Abilities.TiZhi);

            if (isMatch)
            {
                result.Abilities.TiZhi.EverMatched = true;
            }

            if (tizhi > result.Abilities.TiZhi.BestMatch)
            {
                result.Abilities.TiZhi.BestMatch = tizhi;
            }

            return isMatch;
        }

        private static bool TestAbilityLingMin(int actorId, Filters filters, Result result)
        {
            var lingmin = int.Parse(DateFile.instance.actorsDate[actorId][63]); // 绕过 GetActorDate 以读取基础数据

            var isMatch = (lingmin >= filters.Abilities.LingMin);

            if (isMatch)
            {
                result.Abilities.LingMin.EverMatched = true;
            }

            if (lingmin > result.Abilities.LingMin.BestMatch)
            {
                result.Abilities.LingMin.BestMatch = lingmin;
            }

            return isMatch;
        }

        private static bool TestAbilityGenGu(int actorId, Filters filters, Result result)
        {
            var gengu = int.Parse(DateFile.instance.actorsDate[actorId][64]); // 绕过 GetActorDate 以读取基础数据

            var isMatch = (gengu >= filters.Abilities.GenGu);

            if (isMatch)
            {
                result.Abilities.GenGu.EverMatched = true;
            }

            if (gengu > result.Abilities.GenGu.BestMatch)
            {
                result.Abilities.GenGu.BestMatch = gengu;
            }

            return isMatch;
        }

        private static bool TestAbilityWuXing(int actorId, Filters filters, Result result)
        {
            var wuxing = int.Parse(DateFile.instance.actorsDate[actorId][65]); // 绕过 GetActorDate 以读取基础数据

            var isMatch = (wuxing >= filters.Abilities.WuXing);

            if (isMatch)
            {
                result.Abilities.WuXing.EverMatched = true;
            }

            if (wuxing > result.Abilities.WuXing.BestMatch)
            {
                result.Abilities.WuXing.BestMatch = wuxing;
            }

            return isMatch;
        }

        private static bool TestAbilityDingLi(int actorId, Filters filters, Result result)
        {
            var dingli = int.Parse(DateFile.instance.actorsDate[actorId][66]); // 绕过 GetActorDate 以读取基础数据

            var isMatch = (dingli >= filters.Abilities.DingLi);

            if (isMatch)
            {
                result.Abilities.DingLi.EverMatched = true;
            }

            if (dingli > result.Abilities.DingLi.BestMatch)
            {
                result.Abilities.DingLi.BestMatch = dingli;
            }

            return isMatch;
        }

        private static bool TestSkillMaturity(int actorId, Filters filters, Result result)
        {
            var maturity = DateFile.instance.GetActorDate(actorId, 551, false);

            var isMatch = false;

            isMatch |= (filters.Skills.Maturity.Early && maturity == "3");
            isMatch |= (filters.Skills.Maturity.Normal && maturity == "2");
            isMatch |= (filters.Skills.Maturity.Late && maturity == "4");

            if (isMatch)
            {
                result.Skills.Maturity.EverMatched = true;
            }

            return isMatch;
        }

        private static bool TestSkillYinLv(int actorId, Filters filters, Result result)
        {
            var yinlv = int.Parse(DateFile.instance.actorsDate[actorId][501]); // 绕过 GetActorDate 以读取基础数据

            var isMatch = (yinlv >= filters.Skills.YinLv);

            if (isMatch)
            {
                result.Skills.YinLv.EverMatched = true;
            }

            if (yinlv > result.Skills.YinLv.BestMatch)
            {
                result.Skills.YinLv.BestMatch = yinlv;
            }

            return isMatch;
        }

        private static bool TestSkillYiQi(int actorId, Filters filters, Result result)
        {
            var yiqi = int.Parse(DateFile.instance.actorsDate[actorId][502]); // 绕过 GetActorDate 以读取基础数据

            var isMatch = (yiqi >= filters.Skills.YiQi);

            if (isMatch)
            {
                result.Skills.YiQi.EverMatched = true;
            }

            if (yiqi > result.Skills.YiQi.BestMatch)
            {
                result.Skills.YiQi.BestMatch = yiqi;
            }

            return isMatch;
        }

        private static bool TestSkillShiShu(int actorId, Filters filters, Result result)
        {
            var shishu = int.Parse(DateFile.instance.actorsDate[actorId][503]); // 绕过 GetActorDate 以读取基础数据

            var isMatch = (shishu >= filters.Skills.ShiShu);

            if (isMatch)
            {
                result.Skills.ShiShu.EverMatched = true;
            }

            if (shishu > result.Skills.ShiShu.BestMatch)
            {
                result.Skills.ShiShu.BestMatch = shishu;
            }

            return isMatch;
        }

        private static bool TestSkillHuiHua(int actorId, Filters filters, Result result)
        {
            var huihua = int.Parse(DateFile.instance.actorsDate[actorId][504]); // 绕过 GetActorDate 以读取基础数据

            var isMatch = (huihua >= filters.Skills.HuiHua);

            if (isMatch)
            {
                result.Skills.HuiHua.EverMatched = true;
            }

            if (huihua > result.Skills.HuiHua.BestMatch)
            {
                result.Skills.HuiHua.BestMatch = huihua;
            }

            return isMatch;
        }

        private static bool TestSkillShuShu(int actorId, Filters filters, Result result)
        {
            var shushu = int.Parse(DateFile.instance.actorsDate[actorId][505]); // 绕过 GetActorDate 以读取基础数据

            var isMatch = (shushu >= filters.Skills.ShuShu);

            if (isMatch)
            {
                result.Skills.ShuShu.EverMatched = true;
            }

            if (shushu > result.Skills.ShuShu.BestMatch)
            {
                result.Skills.ShuShu.BestMatch = shushu;
            }

            return isMatch;
        }

        private static bool TestSkillPinJian(int actorId, Filters filters, Result result)
        {
            var pinjian = int.Parse(DateFile.instance.actorsDate[actorId][506]); // 绕过 GetActorDate 以读取基础数据

            var isMatch = (pinjian >= filters.Skills.PinJian);

            if (isMatch)
            {
                result.Skills.PinJian.EverMatched = true;
            }

            if (pinjian > result.Skills.PinJian.BestMatch)
            {
                result.Skills.PinJian.BestMatch = pinjian;
            }

            return isMatch;
        }

        private static bool TestSkillDuanZao(int actorId, Filters filters, Result result)
        {
            var duanzao = int.Parse(DateFile.instance.actorsDate[actorId][507]); // 绕过 GetActorDate 以读取基础数据

            var isMatch = (duanzao >= filters.Skills.DuanZao);

            if (isMatch)
            {
                result.Skills.DuanZao.EverMatched = true;
            }

            if (duanzao > result.Skills.DuanZao.BestMatch)
            {
                result.Skills.DuanZao.BestMatch = duanzao;
            }

            return isMatch;
        }

        private static bool TestSkillZhiMu(int actorId, Filters filters, Result result)
        {
            var zhimu = int.Parse(DateFile.instance.actorsDate[actorId][508]); // 绕过 GetActorDate 以读取基础数据

            var isMatch = (zhimu >= filters.Skills.ZhiMu);

            if (isMatch)
            {
                result.Skills.ZhiMu.EverMatched = true;
            }

            if (zhimu > result.Skills.ZhiMu.BestMatch)
            {
                result.Skills.ZhiMu.BestMatch = zhimu;
            }

            return isMatch;
        }

        private static bool TestSkillYiShu(int actorId, Filters filters, Result result)
        {
            var yishu = int.Parse(DateFile.instance.actorsDate[actorId][509]); // 绕过 GetActorDate 以读取基础数据

            var isMatch = (yishu >= filters.Skills.YiShu);

            if (isMatch)
            {
                result.Skills.YiShu.EverMatched = true;
            }

            if (yishu > result.Skills.YiShu.BestMatch)
            {
                result.Skills.YiShu.BestMatch = yishu;
            }

            return isMatch;
        }

        private static bool TestSkillDuShu(int actorId, Filters filters, Result result)
        {
            var dushu = int.Parse(DateFile.instance.actorsDate[actorId][510]); // 绕过 GetActorDate 以读取基础数据

            var isMatch = (dushu >= filters.Skills.DuShu);

            if (isMatch)
            {
                result.Skills.DuShu.EverMatched = true;
            }

            if (dushu > result.Skills.DuShu.BestMatch)
            {
                result.Skills.DuShu.BestMatch = dushu;
            }

            return isMatch;
        }

        private static bool TestSkillZhiJin(int actorId, Filters filters, Result result)
        {
            var zhijin = int.Parse(DateFile.instance.actorsDate[actorId][511]); // 绕过 GetActorDate 以读取基础数据

            var isMatch = (zhijin >= filters.Skills.ZhiJin);

            if (isMatch)
            {
                result.Skills.ZhiJin.EverMatched = true;
            }

            if (zhijin > result.Skills.ZhiJin.BestMatch)
            {
                result.Skills.ZhiJin.BestMatch = zhijin;
            }

            return isMatch;
        }

        private static bool TestSkillQiaoJiang(int actorId, Filters filters, Result result)
        {
            var qiaojiang = int.Parse(DateFile.instance.actorsDate[actorId][512]); // 绕过 GetActorDate 以读取基础数据

            var isMatch = (qiaojiang >= filters.Skills.QiaoJiang);

            if (isMatch)
            {
                result.Skills.QiaoJiang.EverMatched = true;
            }

            if (qiaojiang > result.Skills.QiaoJiang.BestMatch)
            {
                result.Skills.QiaoJiang.BestMatch = qiaojiang;
            }

            return isMatch;
        }

        private static bool TestSkillDaoFa(int actorId, Filters filters, Result result)
        {
            var daofa = int.Parse(DateFile.instance.actorsDate[actorId][513]); // 绕过 GetActorDate 以读取基础数据

            var isMatch = (daofa >= filters.Skills.DaoFa);

            if (isMatch)
            {
                result.Skills.DaoFa.EverMatched = true;
            }

            if (daofa > result.Skills.DaoFa.BestMatch)
            {
                result.Skills.DaoFa.BestMatch = daofa;
            }

            return isMatch;
        }

        private static bool TestSkillFoXue(int actorId, Filters filters, Result result)
        {
            var foxue = int.Parse(DateFile.instance.actorsDate[actorId][514]); // 绕过 GetActorDate 以读取基础数据

            var isMatch = (foxue >= filters.Skills.FoXue);

            if (isMatch)
            {
                result.Skills.FoXue.EverMatched = true;
            }

            if (foxue > result.Skills.FoXue.BestMatch)
            {
                result.Skills.FoXue.BestMatch = foxue;
            }

            return isMatch;
        }

        private static bool TestSkillChuYi(int actorId, Filters filters, Result result)
        {
            var chuyi = int.Parse(DateFile.instance.actorsDate[actorId][515]); // 绕过 GetActorDate 以读取基础数据

            var isMatch = (chuyi >= filters.Skills.ChuYi);

            if (isMatch)
            {
                result.Skills.ChuYi.EverMatched = true;
            }

            if (chuyi > result.Skills.ChuYi.BestMatch)
            {
                result.Skills.ChuYi.BestMatch = chuyi;
            }

            return isMatch;
        }

        private static bool TestSkillZaXue(int actorId, Filters filters, Result result)
        {
            var zaxue = int.Parse(DateFile.instance.actorsDate[actorId][516]); // 绕过 GetActorDate 以读取基础数据

            var isMatch = (zaxue >= filters.Skills.ZaXue);

            if (isMatch)
            {
                result.Skills.ZaXue.EverMatched = true;
            }

            if (zaxue > result.Skills.ZaXue.BestMatch)
            {
                result.Skills.ZaXue.BestMatch = zaxue;
            }

            return isMatch;
        }

        private static bool TestGestMaturity(int actorId, Filters filters, Result result)
        {
            var maturity = DateFile.instance.GetActorDate(actorId, 651, false);

            var isMatch = false;

            isMatch |= (filters.Gest.Maturity.Early && maturity == "3");
            isMatch |= (filters.Gest.Maturity.Normal && maturity == "2");
            isMatch |= (filters.Gest.Maturity.Late && maturity == "4");

            if (isMatch)
            {
                result.Gest.Maturity.EverMatched = true;
            }

            return isMatch;
        }

        private static bool TestGestNeiGong(int actorId, Filters filters, Result result)
        {
            var neigong = int.Parse(DateFile.instance.actorsDate[actorId][601]); // 绕过 GetActorDate 以读取基础数据

            var isMatch = (neigong >= filters.Gest.NeiGong);

            if (isMatch)
            {
                result.Gest.NeiGong.EverMatched = true;
            }

            if (neigong > result.Gest.NeiGong.BestMatch)
            {
                result.Gest.NeiGong.BestMatch = neigong;
            }

            return isMatch;
        }

        private static bool TestGestShenFa(int actorId, Filters filters, Result result)
        {
            var shenfa = int.Parse(DateFile.instance.actorsDate[actorId][602]); // 绕过 GetActorDate 以读取基础数据

            var isMatch = (shenfa >= filters.Gest.ShenFa);

            if (isMatch)
            {
                result.Gest.ShenFa.EverMatched = true;
            }

            if (shenfa > result.Gest.ShenFa.BestMatch)
            {
                result.Gest.ShenFa.BestMatch = shenfa;
            }

            return isMatch;
        }

        private static bool TestGestJueJi(int actorId, Filters filters, Result result)
        {
            var jueji = int.Parse(DateFile.instance.actorsDate[actorId][603]); // 绕过 GetActorDate 以读取基础数据

            var isMatch = (jueji >= filters.Gest.JueJi);

            if (isMatch)
            {
                result.Gest.JueJi.EverMatched = true;
            }

            if (jueji > result.Gest.JueJi.BestMatch)
            {
                result.Gest.JueJi.BestMatch = jueji;
            }

            return isMatch;
        }

        private static bool TestGestQuanZhang(int actorId, Filters filters, Result result)
        {
            var quanzhang = int.Parse(DateFile.instance.actorsDate[actorId][604]); // 绕过 GetActorDate 以读取基础数据

            var isMatch = (quanzhang >= filters.Gest.QuanZhang);

            if (isMatch)
            {
                result.Gest.QuanZhang.EverMatched = true;
            }

            if (quanzhang > result.Gest.QuanZhang.BestMatch)
            {
                result.Gest.QuanZhang.BestMatch = quanzhang;
            }

            return isMatch;
        }

        private static bool TestGestZhiFa(int actorId, Filters filters, Result result)
        {
            var zhifa = int.Parse(DateFile.instance.actorsDate[actorId][605]); // 绕过 GetActorDate 以读取基础数据

            var isMatch = (zhifa >= filters.Gest.ZhiFa);

            if (isMatch)
            {
                result.Gest.ZhiFa.EverMatched = true;
            }

            if (zhifa > result.Gest.ZhiFa.BestMatch)
            {
                result.Gest.ZhiFa.BestMatch = zhifa;
            }

            return isMatch;
        }

        private static bool TestGestTuiFa(int actorId, Filters filters, Result result)
        {
            var tuifa = int.Parse(DateFile.instance.actorsDate[actorId][606]); // 绕过 GetActorDate 以读取基础数据

            var isMatch = (tuifa >= filters.Gest.TuiFa);

            if (isMatch)
            {
                result.Gest.TuiFa.EverMatched = true;
            }

            if (tuifa > result.Gest.TuiFa.BestMatch)
            {
                result.Gest.TuiFa.BestMatch = tuifa;
            }

            return isMatch;
        }

        private static bool TestGestAnQi(int actorId, Filters filters, Result result)
        {
            var anqi = int.Parse(DateFile.instance.actorsDate[actorId][607]); // 绕过 GetActorDate 以读取基础数据

            var isMatch = (anqi >= filters.Gest.AnQi);

            if (isMatch)
            {
                result.Gest.AnQi.EverMatched = true;
            }

            if (anqi > result.Gest.AnQi.BestMatch)
            {
                result.Gest.AnQi.BestMatch = anqi;
            }

            return isMatch;
        }

        private static bool TestGestJianFa(int actorId, Filters filters, Result result)
        {
            var jianfa = int.Parse(DateFile.instance.actorsDate[actorId][608]); // 绕过 GetActorDate 以读取基础数据

            var isMatch = (jianfa >= filters.Gest.JianFa);

            if (isMatch)
            {
                result.Gest.JianFa.EverMatched = true;
            }

            if (jianfa > result.Gest.JianFa.BestMatch)
            {
                result.Gest.JianFa.BestMatch = jianfa;
            }

            return isMatch;
        }

        private static bool TestGestDaoFa(int actorId, Filters filters, Result result)
        {
            var daofa = int.Parse(DateFile.instance.actorsDate[actorId][609]); // 绕过 GetActorDate 以读取基础数据

            var isMatch = (daofa >= filters.Gest.DaoFa);

            if (isMatch)
            {
                result.Gest.DaoFa.EverMatched = true;
            }

            if (daofa > result.Gest.DaoFa.BestMatch)
            {
                result.Gest.DaoFa.BestMatch = daofa;
            }

            return isMatch;
        }

        private static bool TestGestChangBing(int actorId, Filters filters, Result result)
        {
            var changbing = int.Parse(DateFile.instance.actorsDate[actorId][610]); // 绕过 GetActorDate 以读取基础数据

            var isMatch = (changbing >= filters.Gest.ChangBing);

            if (isMatch)
            {
                result.Gest.ChangBing.EverMatched = true;
            }

            if (changbing > result.Gest.ChangBing.BestMatch)
            {
                result.Gest.ChangBing.BestMatch = changbing;
            }

            return isMatch;
        }

        private static bool TestGestQiMen(int actorId, Filters filters, Result result)
        {
            var qimen = int.Parse(DateFile.instance.actorsDate[actorId][611]); // 绕过 GetActorDate 以读取基础数据

            var isMatch = (qimen >= filters.Gest.QiMen);

            if (isMatch)
            {
                result.Gest.QiMen.EverMatched = true;
            }

            if (qimen > result.Gest.QiMen.BestMatch)
            {
                result.Gest.QiMen.BestMatch = qimen;
            }

            return isMatch;
        }

        private static bool TestGestRuanBing(int actorId, Filters filters, Result result)
        {
            var ruanbing = int.Parse(DateFile.instance.actorsDate[actorId][612]); // 绕过 GetActorDate 以读取基础数据

            var isMatch = (ruanbing >= filters.Gest.RuanBing);

            if (isMatch)
            {
                result.Gest.RuanBing.EverMatched = true;
            }

            if (ruanbing > result.Gest.RuanBing.BestMatch)
            {
                result.Gest.RuanBing.BestMatch = ruanbing;
            }

            return isMatch;
        }

        private static bool TestGestYuShe(int actorId, Filters filters, Result result)
        {
            var yushe = int.Parse(DateFile.instance.actorsDate[actorId][613]); // 绕过 GetActorDate 以读取基础数据

            var isMatch = (yushe >= filters.Gest.YuShe);

            if (isMatch)
            {
                result.Gest.YuShe.EverMatched = true;
            }

            if (yushe > result.Gest.YuShe.BestMatch)
            {
                result.Gest.YuShe.BestMatch = yushe;
            }

            return isMatch;
        }

        private static bool TestGestYueQi(int actorId, Filters filters, Result result)
        {
            var yueqi = int.Parse(DateFile.instance.actorsDate[actorId][614]); // 绕过 GetActorDate 以读取基础数据

            var isMatch = (yueqi >= filters.Gest.YueQi);

            if (isMatch)
            {
                result.Gest.YueQi.EverMatched = true;
            }

            if (yueqi > result.Gest.YueQi.BestMatch)
            {
                result.Gest.YueQi.BestMatch = yueqi;
            }

            return isMatch;
        }
    }
}
