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
        public const string VERSION = "1.0.1";

        internal static ManualLogSource Log;
        internal static ConfigEntry<string> StatGrabbables;
        internal static ConfigEntry<bool> HideZeroItems;

        private void Awake()
        {
            Log = Logger;
            StatGrabbables = Config.Bind("Settings", "StatGrabbables", "Pro-flashlight,Shovel,Stun grenade::Stun Grenade,Spray paint::Spray Paint,TZP-Inhalant,Rocket Launcher,Jetpack,Boombox,Lockpicker,Extension ladder::Extension Ladder", "Grabbable names which will be stated while scan in ship, splitted by \",\", use \"::\" to set name alias for display.");
            HideZeroItems = Config.Bind("Settings", "HideZeroItems", true, "Hide grabbable item stat if its count is zero.");

            Harmony harmony = new Harmony(GUID);
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}
