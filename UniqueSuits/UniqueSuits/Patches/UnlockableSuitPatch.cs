using GameNetcodeStuff;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using Unity.Services.Authentication.Generated;
using UnityEngine;

namespace UniqueSuits.Patches
{
    [HarmonyPatch(typeof(UnlockableSuit))]
    public class UnlockableSuitPatch
    {
        //possibly useless patch
        //[HarmonyPatch("SwitchSuitToThis")]
        //[HarmonyPrefix]
        //static void SwitchSuitToThisPatch(UnlockableSuit __instance, PlayerControllerB playerWhoTriggered = null)
        //{
        //    UniqueSuits.Logger.LogDebug("Player switched suits");
        //    UniqueSuits.Logger.LogDebug("Player: " + playerWhoTriggered.playerClientId);
        //    UniqueSuits.Logger.LogDebug("Suit: " + __instance.syncedSuitID.Value);
        //    if (playerWhoTriggered != null)
        //    {
        //        if (UniqueSuits.Instance.PlayerIDSuitIDs.ContainsKey(playerWhoTriggered.playerClientId))
        //        {
        //            UniqueSuits.Instance.PlayerIDSuitIDs[playerWhoTriggered.playerClientId] = __instance.syncedSuitID.Value;
        //        }
        //        else
        //        {
        //            UniqueSuits.Instance.PlayerIDSuitIDs.Add(playerWhoTriggered.playerClientId, __instance.syncedSuitID.Value);
        //        }
        //    }
        //    StartOfRound.Instance.PositionSuitsOnRack();
        //}

        [HarmonyPatch("SwitchSuitForPlayer")]
        [HarmonyPrefix]
        static void SwitchSuitForPlayerPatch(UnlockableSuit __instance, PlayerControllerB player, int suitID, bool playAudio = true)
        {
            UniqueSuits.Logger.LogDebug("Someone else switched suits");
            UniqueSuits.Logger.LogDebug("Player: " + player.playerClientId);
            UniqueSuits.Logger.LogDebug("Suit: " + suitID);
            if (player != null)
            {
                if (UniqueSuits.PlayerIDSuitIDs.ContainsKey(player.playerClientId))
                {
                    UniqueSuits.PlayerIDSuitIDs[player.playerClientId] = suitID;
                }
                else
                {
                    UniqueSuits.PlayerIDSuitIDs.Add(player.playerClientId, suitID);
                }
            }
            StartOfRound.Instance.PositionSuitsOnRack();
        }

        [HarmonyPatch("SwitchSuitForAllPlayers")]
        [HarmonyPrefix]
        static void SwitchSuitForAllPlayersPatch(UnlockableSuit __instance, int suitID, bool playAudio = false)
        {
            UniqueSuits.Logger.LogDebug("Everyone switched suits to: " + suitID);
            for (int i = 0; i < StartOfRound.Instance.allPlayerScripts.Length; i++)
            {
                Material material = StartOfRound.Instance.unlockablesList.unlockables[suitID].suitMaterial;
                PlayerControllerB player = StartOfRound.Instance.allPlayerScripts[i];
                if (UniqueSuits.PlayerIDSuitIDs.ContainsKey(player.playerClientId))
                {
                    UniqueSuits.PlayerIDSuitIDs[player.playerClientId] = suitID;
                }
                else
                {
                    UniqueSuits.PlayerIDSuitIDs.Add(player.playerClientId, suitID);
                }
            }
            StartOfRound.Instance.PositionSuitsOnRack();
        }
    }
}
