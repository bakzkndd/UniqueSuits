using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace UniqueSuits.Patches
{
    public class TooManySuitsPatch
    {
        private static bool isPatched = false;

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        public static void DoIt()
        {
            if (isPatched)
            {
                return;
            }
            UniqueSuits.Harmony.Patch(
                original: AccessTools.Method(typeof(TooManySuits.PaginationController), "DisplayCurrentPage"),
                postfix: new HarmonyMethod(typeof(TooManySuitsPatch), nameof(PositionSuitsOnRackPatch))
            );
            isPatched = true;
        }

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private static void PositionSuitsOnRackPatch()
        {
            if (UniqueSuits.IsTooManySuitsLoaded)
            {
                HashSet<int> suitsInUse = UniqueSuits.PlayerIDSuitIDs.Values.ToHashSet<int>();
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
                        component.positionOffset = new Vector3(-2.45f, -100f, -8.41f);
                        UniqueSuits.Logger.LogDebug("Succesfully put a suit under the ground: " + suit.syncedSuitID.Value);
                        index++;
                    }
                }
            }
        }
    }
}
