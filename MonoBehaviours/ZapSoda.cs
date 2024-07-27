using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace pnot0sThings.MonoBehaviours
{
    internal class ZapSoda : GrabbableObject
    {
        public override void Start()
        {
            base.Start();
            grabbable = true;
            grabbableToEnemies = true;
            itemProperties.allowDroppingAheadOfPlayer = false;
        }
        public override async void ItemActivate(bool used, bool buttonDown = true)
        {
            base.ItemActivate(used, buttonDown);
            playerHeldBy.drunkness = 10f;
            playerHeldBy.itemAudio.PlayOneShot(itemProperties.throwSFX);
            await Task.Delay(1000);
        }
    }
}
