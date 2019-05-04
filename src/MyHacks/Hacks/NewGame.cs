using System.Linq;

namespace MyHacks.Hacks
{
    public static class NewGame
    {
        public static void PostMakeNewGameMianActor()
        {
            SetMainActorProperties();
        }

        public static void PostSetNewGameDate()
        {
            LogBirthPlace();
        }

        private static void SetMainActorProperties()
        {
            var mainActorId = DateFile.instance.MianActorID();
            var mainActorData = DateFile.instance.actorsDate[mainActorId];

            mainActorData[13] = "48"; // 寿命
            mainActorData[12] = (48 - int.Parse(mainActorData[11])).ToString(); // 健康

            mainActorData[15] = "850"; // 魅力

            mainActorData[551] = "3"; // 技艺早熟
            mainActorData[651] = "3"; // 功法早熟

            mainActorData[101] = "0|4001|9997|3003|2008|26|74|37|19"; // 特性：无|真阳纯阴|神魂俱安|生于小雪|一块玉佩|灵心慧性|沉稳果决|良材美玉|骨骼清奇
        }

        private static void LogBirthPlace()
        {
            var worldId = DateFile.instance.newActorBirthPlaceId - 1;
            var worldData = DateFile.instance.baseWorldDate[worldId];

            var partId = worldData.Keys.OrderBy(k => k).ToList()[2];
            var partData = DateFile.instance.partWorldMapDate[partId];

            Main.Logger.Log(string.Format("出生地为{0}", partData[0]));
        }
    }
}
