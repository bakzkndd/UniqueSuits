using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UniqueSuits.Patches
{
    [HarmonyPatch(typeof(StartOfRound))]
    public class RackPatch
    {
        [HarmonyPatch("PositionSuitsOnRack")]
        [HarmonyPrefix]
        [HarmonyPriority(Priority.High)]
        [HarmonyAfter(UniqueSuits.MoreSuitsGUID)]
        static void PositionSuitsOnRackPatch(ref StartOfRound __instance)
        {
            HashSet<int> suitsInUse = UniqueSuits.Instance.PlayerIDSuitIDs.Values.ToHashSet<int>();
            List<UnlockableSuit> suits = UnityEngine.Object.FindObjectsOfType<UnlockableSuit>().ToList<UnlockableSuit>();
            suits = suits.OrderBy(suit => suit.syncedSuitID.Value).ToList();
            int index = 0;
            foreach (UnlockableSuit suit in suits)
            {
                AutoParentToShip component = suit.gameObject.GetComponent<AutoParentToShip>();
                component.overrideOffset = true;
                UniqueSuits.Logger.LogDebug("Checking suit on rack: " + suit.syncedSuitID.Value);
                if (suitsInUse.Contains(suit.syncedSuitID.Value) && suit.syncedSuitID.Value != 0) //keep the default suit
                {
                    component.positionOffset = new Vector3(-2.45f, -100f, -8.41f) + __instance.rightmostSuitPosition.forward * (float)index;
                    UniqueSuits.Logger.LogDebug("Succesfully put a suit under the ground: " + suit.syncedSuitID.Value);
                    index++;
                }
            }
        }
    }
}
