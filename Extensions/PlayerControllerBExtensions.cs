using GameNetcodeStuff;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestItem.Extensions
{
    public static class PlayerControllerBExtensions
    {
        private static Dictionary<PlayerControllerB, bool> heldByCustomFlags = new Dictionary<PlayerControllerB, bool>();

        public static bool CatItemGetWasItemUsed(this PlayerControllerB player)
        {
            if (heldByCustomFlags.ContainsKey(player))
            {
                return heldByCustomFlags[player];
            }
            else
            {
                // Default value if not set yet
                return false;
            }
        }

        public static void CatItemWasItemUsed(this PlayerControllerB player, bool value)
        {
            if (heldByCustomFlags.ContainsKey(player))
            {
                heldByCustomFlags[player] = value;
            }
            else
            {
                heldByCustomFlags.Add(player, value);
            }
        }
    }
}
