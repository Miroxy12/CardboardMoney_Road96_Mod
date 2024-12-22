using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using BlueEyes.NarrativeBrain;
using HarmonyLib;
using UnityEngine;

namespace CardboardMoney_Road96_Mod
{
    [BepInEx.BepInPlugin(mod_guid, "CardboardMoney", version)]
    [BepInEx.BepInProcess("Road 96.exe")]
    public class CardboardMoneyMod : BasePlugin
    {
        private const string mod_guid = "miroxy12.cardboardmoney";
        private const string version = "1.0";
        private readonly Harmony harmony = new Harmony(mod_guid);
        internal static new ManualLogSource Log;

        public override void Load()
        {
            Log = base.Log;
            Log.LogInfo(mod_guid + " started, version: " + version);
            harmony.PatchAll(typeof(GiveRewardHook));
            AddComponent<ModMain>();
        }
    }

    public class ModMain : MonoBehaviour
    {
        void Awake()
        {
            CardboardMoneyMod.Log.LogInfo("loading CardboardMoney");
        }
        void OnEnable()
        {
            CardboardMoneyMod.Log.LogInfo("enabled CardboardMoney");
        }
    }

    [HarmonyPatch(typeof(RessourceImpact), "GiveReward", new System.Type[] { typeof(NarrativeContext), typeof(NarrativeSettings), typeof(bool) })]
    public class GiveRewardHook
    {
        static void Postfix(RessourceImpact __instance, NarrativeContext context, NarrativeSettings settings, bool skipFeedback)
        {
            System.Random rdm = new System.Random();
            int moneygained = 0;

            if (__instance.name.ToString() == "RES_SleepOnCardboard") {
                moneygained = rdm.Next(1, 12);
                for (int i = 0; i < moneygained; i++) { // for some weird reasons I need to do that because context.AddMoney(2) or AddMoney(999) doesn't work
                    context.AddMoney(1);
                }
            }
        }
    }
}