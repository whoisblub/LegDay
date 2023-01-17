using System;
using BepInEx;
using EFT;
using Comfort.Common;
using BepInEx.Logging;
using Aki.Reflection.Patching;
using System.Reflection;
using BepInEx.Configuration;

namespace LegDay
{
    [BepInPlugin("com.blub.legday", "LegDay", "1.0.0")]
    internal class Plugin : BaseUnityPlugin
    {
        public static ManualLogSource Log;
        public static ConfigFile Cfg;

        private void Awake()
        {
            Log = base.Logger;
            Cfg = base.Config;

            Prefs.Bind(Config);
            
            Log.LogInfo("tren has been injected");
        }

        private void Start()
        {
            new GameWorldStartedPatch().Enable();
        }
    }

    internal class GameWorldStartedPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return typeof(GameWorld).GetMethod("OnGameStarted", BindingFlags.Public | BindingFlags.Instance);
        }

        [PatchPostfix]
        static void GameWorld_OnGameStarted_PostFix()
        {
            Prefs.FindBuffReferences(Singleton<GameWorld>.Instance.AllPlayers[0].Skills);
            Plugin.Log.LogInfo("Found all Player Skill Buffs");
            
            if (Prefs.buffsEnabled.Value)
            {
                Prefs.ApplyAllBuffs();
            }
        }
    }
}
