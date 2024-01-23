using HarmonyLib;
using System;
using System.Collections;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using Object = UnityEngine.Object;

namespace ShipGrabbableStat.Patches
{
    [HarmonyPatch]
    internal class HUDManagerPatcher
    {
        private static GameObject _shipGrabbableCounter;
        private static TextMeshProUGUI _textMesh;
        private static float _displayTimeLeft;
        private const float DisplayTime = 5f;

        [HarmonyPrefix]
        [HarmonyPatch(typeof(HUDManager), nameof(HUDManager.PingScan_performed))]
        private static void OnScan(HUDManager __instance, InputAction.CallbackContext context)
        {
            if (GameNetworkManager.Instance.localPlayerController == null)
                return;
            if (!context.performed || !__instance.CanPlayerScan() || __instance.playerPingingScan > -0.5f)
                return;
            // Only allow this special scan to work while inside the ship.
            if (!StartOfRound.Instance.inShipPhase && !GameNetworkManager.Instance.localPlayerController.isInHangarShipRoom)
                return;

            if (!_shipGrabbableCounter)
                CopyValueCounter();

            _textMesh.text = CalculateGrabbableStatText();
            _displayTimeLeft = DisplayTime;
            if (!_shipGrabbableCounter.activeSelf)
                GameNetworkManager.Instance.StartCoroutine(ShipGrabbableStatCoroutine());
        }

        private static IEnumerator ShipGrabbableStatCoroutine()
        {
            _shipGrabbableCounter.SetActive(true);
            while (_displayTimeLeft > 0f)
            {
                float time = _displayTimeLeft;
                _displayTimeLeft = 0f;
                yield return new WaitForSeconds(time);
            }
            _shipGrabbableCounter.SetActive(false);
        }

        /// <summary>
        /// Calculate the count text of all grabbable in the ship.
        /// </summary>
        /// <returns>The total grabbable stat text.</returns>
        private static string CalculateGrabbableStatText()
        {
            GameObject ship = GameObject.Find("/Environment/HangarShip");
            var allGrabbables = ship.GetComponentsInChildren<GrabbableObject>().ToList();

            var sb = new StringBuilder();
            var statGrabbables = ShipGrabbableStat.StatGrabbables.Value.Split(',')
                .Select(i => i.Trim().Split(new string[] { "::" }, StringSplitOptions.None));
            foreach (var statGrabbable in statGrabbables)
            {
                var itemName = statGrabbable[0];
                var itemText = statGrabbable.Length > 1 ? statGrabbable[1] : statGrabbable[0];
                int itemCount = allGrabbables.Count(item => item.itemProperties.itemName == itemName);
                if (itemCount != 0 || !ShipGrabbableStat.HideZeroItems.Value)
                {
                    sb.AppendLine($"{itemText} x{itemCount}");
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Copy an existing object loaded by the game for the display of ship loot and put it in the right position.
        /// </summary>
        private static void CopyValueCounter()
        {
            var allItemNames = string.Join(", ", StartOfRound.Instance.allItemsList.itemsList.Select(item => $"{item.itemName}").ToList());
            ShipGrabbableStat.Log.LogDebug($"List all items: {allItemNames}");

            GameObject valueCounter = GameObject.Find("/Systems/UI/Canvas/IngamePlayerHUD/BottomMiddle/ValueCounter");
            if (!valueCounter)
                ShipGrabbableStat.Log.LogError("Failed to find ValueCounter object to copy!");
            GameObject topRightCorner = GameObject.Find("/Systems/UI/Canvas/IngamePlayerHUD/TopRightCorner");
            if (!topRightCorner)
                ShipGrabbableStat.Log.LogError("Failed to find TopRightCorner object to paste!");

            _shipGrabbableCounter = Object.Instantiate(valueCounter.gameObject, topRightCorner.transform, false);
            Vector3 pos = _shipGrabbableCounter.transform.localPosition;
            _shipGrabbableCounter.transform.localPosition = new Vector3(pos.x - 25f, -15f, pos.z);

            _textMesh = _shipGrabbableCounter.GetComponentInChildren<TextMeshProUGUI>();
            _textMesh.fontSize = 18;
            _textMesh.alignment = TextAlignmentOptions.TopRight;

            _shipGrabbableCounter.GetComponentInChildren<UnityEngine.UI.Image>()?.gameObject.SetActive(false);
        }
    }
}
