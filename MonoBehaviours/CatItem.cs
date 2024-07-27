using GameNetcodeStuff;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Xml.Linq;
using UnityEngine;
using static UnityEngine.UIElements.UxmlAttributeDescription;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib.Tools;
using System.Timers;
using System.Threading;
using System.Threading.Tasks;
using LethalLib.Modules;
using static LethalLib.Modules.Enemies;
using UnityEngine.UI;
using TestItem.Extensions;
using System.IO;
using System.Reflection;

namespace pnot0sThings.Behaviour
{
    internal class CatItem : GrabbableObject
    {
        public override void Start()
        {
            base.Start();
            grabbable = true;
            grabbableToEnemies = true;
            itemProperties.allowDroppingAheadOfPlayer = false;
        }
        public override void ItemActivate(bool used, bool buttonDown = true)
        {

            if (!StartOfRound.Instance.inShipPhase && !playerHeldBy.CatItemGetWasItemUsed())
            {
                base.ItemActivate(used, buttonDown);
                if (playerHeldBy != null && buttonDown)
                {
                    playerHeldBy.movementSpeed *= 2.5f;
                    playerHeldBy.jumpForce *= 1.8f;

                    playerHeldBy.DamagePlayer(50);

                    //play audio when used
                    playerHeldBy.itemAudio.PlayOneShot(itemProperties.throwSFX);

                    playerHeldBy.CatItemWasItemUsed(true);
                    ItemTimer(playerHeldBy);
                    playerHeldBy.DespawnHeldObject();
                }
            }
        }
        public async void ItemTimer(PlayerControllerB playerHeldBy)
        {
            while (!StartOfRound.Instance.inShipPhase)
            {
                //Do nothing, just wait
                
                await Task.Delay(2000);
            }
            if (StartOfRound.Instance.inShipPhase && base.playerHeldBy.CatItemGetWasItemUsed())
            {


                playerHeldBy.CatItemWasItemUsed(false);
                playerHeldBy.movementSpeed /= 2.5f;
                base.playerHeldBy.jumpForce /= 1.8f;
            }
        }
    }
}
