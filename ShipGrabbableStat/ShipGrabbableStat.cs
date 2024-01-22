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
        public const string GUID = "com.github.tinymins.ShipGrabbableStat";
        public const string NAME = "ShipGrabbableStat";
        public const string VERSION = "1.0";

        internal static ManualLogSource Log;
        internal static ConfigEntry<string> CountItems;

        private void Awake()
        {
            Log = Logger;
            CountItems = Config.Bind("Settings", "CountItems", "Pro-flashlight::Pro Flashlight,Shovel,Stun grenade::Stun Grenade,Rocket Launcher,Spray paint::Spray Paint,TZP-Inhalant", "Item names which will be counted while scan in ship, splitted by \",\", use \"::\" to set item name alias for display.");

            Harmony harmony = new Harmony(GUID);
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}
