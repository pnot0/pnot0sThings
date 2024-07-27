using BepInEx;
using BepInEx.Logging;
using GameNetcodeStuff;
using HarmonyLib.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using UnityEngine;
using Logger = BepInEx.Logging.Logger;

namespace pnot0sThings.ItemBehaviour
{
    internal class SpeedCoil : GrabbableObject
    {
        public override void Start()
        {
            base.Start();
            grabbable = true;
            grabbableToEnemies = true;
        }

        public override void EquipItem()
        {
            base.EquipItem();
            playerHeldBy.movementSpeed *= 2.5f;
        }
        public override void PocketItem()
        {
            base.PocketItem();
            playerHeldBy.movementSpeed /= 2.5f;
        }
        public override void Update()
        {
            base.Update();
            if (hasBeenHeld)
            {
                if (!isHeld && !isPocketed)
                {
                    var player = StartOfRound.Instance.allPlayerScripts.FirstOrDefault(x => x.OwnerClientId == OwnerClientId);
                    player.movementSpeed /= 2.5f;
                    hasBeenHeld = false;
                }
            }
        }
    }
}
