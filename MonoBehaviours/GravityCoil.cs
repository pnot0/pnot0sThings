using BepInEx.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pnot0sThings.ItemBehaviour
{
    internal class GravityCoil : GrabbableObject
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
            playerHeldBy.jumpForce *= 1.8f;
        }
        public override void PocketItem()
        {
            base.PocketItem();
            playerHeldBy.jumpForce /= 1.8f;
        }
        public override void Update()
        {
            base.Update();
            if (hasBeenHeld)
            {
                if (!isHeld && !isPocketed)
                {
                    var player = StartOfRound.Instance.allPlayerScripts.FirstOrDefault(x => x.OwnerClientId == OwnerClientId);
                    player.jumpForce /= 1.8f;
                    hasBeenHeld = false;
                }
            }
        }
    }
}
