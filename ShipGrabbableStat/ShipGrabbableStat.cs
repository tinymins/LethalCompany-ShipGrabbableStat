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
        internal static ConfigEntry<string> CountItemNames;

        private void Awake()
        {
            Log = Logger;
            CountItemNames = Config.Bind("Settings", "CountItemNames", "Pro-flashlight,Shovel,Stun grenade,Rocket Launcher,Spray paint,TZP-Inhalant", "Item names which will be counted while scan in ship, splitted by \",\".");

            Harmony harmony = new Harmony(GUID);
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}
