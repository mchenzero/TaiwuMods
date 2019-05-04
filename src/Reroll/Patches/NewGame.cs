using System;

namespace Reroll.Patches
{
    public static class NewGame
    {
        [GeneratorPatch(typeof(global::NewGame), "MakeNewGameMianActor")]
        public static class MakeNewGameMianActorPatch
        {
            public static bool AlreadyInPatch = false;

            public static void PostMoveNext(bool __result)
            {
                if (__result == false && Main.Enabled && !AlreadyInPatch)
                {
                    try
                    {
                        AlreadyInPatch = true;
                        TestAndReroll();
                    }
                    finally
                    {
                        AlreadyInPatch = false;
                    }
                }
            }
        }

        private static void TestAndReroll()
        {
            var result = Taiwu.Test(Taiwu.MainActorId, Main.Settings.NewGame);

            for (var i = 0; i < Main.Settings.MaxRoll; i++)
            {
                if (result.IsMatch)
                {
                    Main.Logger.Log(string.Format("重掷{0}次后找到匹配！", i));
                    break;
                }

                Reroll();

                result = Taiwu.Test(Taiwu.MainActorId, Main.Settings.NewGame, result);
            }

            if (!result.IsMatch)
            {
                result.LogFailure();
            }
        }

        private static void Reroll()
        {
            DateFile.instance.NewDate();
            DateFile.instance.NewGameDate();
            Loading.instance.newGameDate = true;
            var itr = global::NewGame.instance.MakeNewGameMianActor();
            while (itr.MoveNext()) {}
        }
    }
}
