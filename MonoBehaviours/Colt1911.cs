using System;
using System.Collections.Generic;
using System.Text;

namespace pnot0sThings.MonoBehaviours
{
    internal class Colt1911 : GrabbableObject
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
            base.ItemActivate(used, buttonDown);
            playerHeldBy.itemAudio.PlayOneShot(itemProperties.throwSFX);
        }
    }
}
