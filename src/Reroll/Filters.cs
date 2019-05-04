using System;

namespace Reroll
{
    public class Filters
    {
        public GenderOptions Gender = new GenderOptions();
        public GoodnessOptions Goodness = new GoodnessOptions();
        public AgeRange Age = new AgeRange();
        public int MaxLife = 0;
        public int Charm = 0;
        public string Features = "";
        public AbilityOptions Abilities = new AbilityOptions();
        public SkillOptions Skills = new SkillOptions();
        public GestOptions Gest = new GestOptions();

        public class GenderOptions
        {
            public bool Male = true;
            public bool Female = true;
        }

        public class GoodnessOptions
        {
            public bool Upright = true;
            public bool Kind = true;
            public bool Moderate = true;
            public bool Rebellious = true;
            public bool Selfish = true;
        }

        public class AgeRange
        {
            public int Min = 0;
            public int Max = 100;
        }

        public class AbilityOptions
        {
            public int BiLi = 0;
            public int TiZhi = 0;
            public int LingMin = 0;
            public int GenGu = 0;
            public int WuXing = 0;
            public int DingLi = 0;
        }

        public class SkillOptions
        {
            public MaturityOptions Maturity = new MaturityOptions();
            public int YinLv = 0;
            public int YiQi = 0;
            public int ShiShu = 0;
            public int HuiHua = 0;
            public int ShuShu = 0;
            public int PinJian = 0;
            public int DuanZao = 0;
            public int ZhiMu = 0;
            public int YiShu = 0;
            public int DuShu = 0;
            public int ZhiJin = 0;
            public int QiaoJiang = 0;
            public int DaoFa = 0;
            public int FoXue = 0;
            public int ChuYi = 0;
            public int ZaXue = 0;
        }

        public class GestOptions
        {
            public MaturityOptions Maturity = new MaturityOptions();
            public int NeiGong = 0;
            public int ShenFa = 0;
            public int JueJi = 0;
            public int QuanZhang = 0;
            public int ZhiFa = 0;
            public int TuiFa = 0;
            public int AnQi = 0;
            public int JianFa = 0;
            public int DaoFa = 0;
            public int ChangBing = 0;
            public int QiMen = 0;
            public int RuanBing = 0;
            public int YuShe = 0;
            public int YueQi = 0;
        }

        public class MaturityOptions
        {
            public bool Early = true;
            public bool Normal = true;
            public bool Late = true;
        }
    }
}
