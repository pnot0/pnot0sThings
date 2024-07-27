using System;
using System.Collections.Generic;
using System.Text;

namespace pnot0sThings.MonoBehaviours
{
    internal class DollySoda : GrabbableObject
    {
        public override void Start()
        {
            base.Start();
            grabbable = true;
            grabbableToEnemies = true;
        }
    }
}
