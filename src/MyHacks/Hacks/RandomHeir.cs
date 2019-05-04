
namespace MyHacks.Hacks
{
    public static class RandomHeir
    {
        public static void PostRandomHeirMakeNewActor(int actorId)
        {
            var actorData = DateFile.instance.actorsDate[actorId];

            actorData[11] = "16"; // 年龄
            actorData[12] = "32"; // 健康
            actorData[13] = "48"; // 寿命

            actorData[551] = "3"; // 技艺早熟
            actorData[651] = "3"; // 功法早熟
        }
    }
}
