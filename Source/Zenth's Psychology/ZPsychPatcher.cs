using HarmonyLib;
using Verse;

namespace ZPsych
{
    [StaticConstructorOnStartup]
    public static class ZPsychPatcher
    {
        static ZPsychPatcher()
        {
            var harmony = new Harmony("ZenthWolf.Psych");
            Log.Message("Hello, Psych.");
        }
    }
}
