using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;

namespace ShipGrabbableStat
{
    [BepInPlugin(GUID, NAME, VERSION)]
    internal class ShipGrabbableStat : BaseUnityPlugin
    {
        public const string GUID = "com.zhaiyiming.github.tinymins.ShipGrabbableStat";
        public const string NAME = "ShipGrabbableStat";
        public const string VERSION = "1.0.5";

        internal static ManualLogSource Log;
        internal static ConfigEntry<string> StatGrabbables;
        internal static ConfigEntry<bool> HideZeroItems;

        private void Awake()
        {
            Log = Logger;
            StatGrabbables = Config.Bind("Settings", "StatGrabbables", "Walkie-talkie,Flashlight,Shovel,Lockpicker,Pro-flashlight,Stun grenade::Stun Grenade,Boombox,TZP-Inhalant,Zap gun,Jetpack,Extension ladder::Extension Ladder,Radar-booster,Spray paint::Spray Paint,Rocket Launcher,Flaregun,Emergency Flare (ammo),Toy Hammer,Remote Radar,Utility Belt,Hacking Tool,Pinger,Portable Tele,Advanced Portable Tele,Night Vision Goggles,Medkit,Peeper,Helmet,Diving Kit,Wheelbarrow,Ouija Board,Shells,Key,Fire Axe", "Grabbable names which will be stated while scan in ship, splitted by \",\", use \"::\" to set name alias for display.");
            HideZeroItems = Config.Bind("Settings", "HideZeroItems", true, "Hide grabbable item stat if its count is zero.");

            Harmony harmony = new Harmony(GUID);
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}
