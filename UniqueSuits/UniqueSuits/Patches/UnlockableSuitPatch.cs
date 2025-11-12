using GameNetcodeStuff;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;

namespace UniqueSuits.Patches
{
    [HarmonyPatch(typeof(UnlockableSuit))]
    public class UnlockableSuitPatch
    {
        [HarmonyPatch("SwitchSuitToThis")]
        [HarmonyPrefix]
        static void SwitchSuitToThisPatch(UnlockableSuit __instance, PlayerControllerB playerWhoTriggered = null)
        {
            UniqueSuits.Logger.LogDebug("Player switched suits");
            UniqueSuits.Logger.LogDebug("Player: " + playerWhoTriggered.playerClientId);
            UniqueSuits.Logger.LogDebug("Suit: " + __instance.syncedSuitID.Value);
            if (playerWhoTriggered != null)
            {
                if (UniqueSuits.Instance.PlayerIDSuitIDs.ContainsKey(playerWhoTriggered.playerClientId))
                {
                    UniqueSuits.Instance.PlayerIDSuitIDs[playerWhoTriggered.playerClientId] = __instance.syncedSuitID.Value;
                }
                else
                {
                    UniqueSuits.Instance.PlayerIDSuitIDs.Add(playerWhoTriggered.playerClientId, __instance.syncedSuitID.Value);
                }
            }
            StartOfRound.Instance.PositionSuitsOnRack();
        }

        [HarmonyPatch("SwitchSuitForPlayer")]
        [HarmonyPrefix]
        static void SwitchSuitForPlayerPatch(UnlockableSuit __instance, PlayerControllerB player, int suitID, bool playAudio = true)
        {
            UniqueSuits.Logger.LogDebug("Someone else switched suits");
            UniqueSuits.Logger.LogDebug("Player: " + player.playerClientId);
            UniqueSuits.Logger.LogDebug("Suit: " + suitID);
            if (player != null)
            {
                if (UniqueSuits.Instance.PlayerIDSuitIDs.ContainsKey(player.playerClientId))
                {
                    UniqueSuits.Instance.PlayerIDSuitIDs[player.playerClientId] = suitID;
                }
                else
                {
                    UniqueSuits.Instance.PlayerIDSuitIDs.Add(player.playerClientId, suitID);
                }
            }
            StartOfRound.Instance.PositionSuitsOnRack();
        }
    }
}
