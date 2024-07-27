using LethalLib.Modules;
using System.Linq;
using System.Threading.Tasks;
using Unity.Netcode;

namespace pnot0sThings.ItemBehaviour
{

    internal class FaultyRPG : GrabbableObject
    {
        public override void Start()
        {
            base.Start();
            grabbable = true;    
            grabbableToEnemies = false;
        }
        public override async void ItemActivate(bool used, bool buttonDown = true)
        {
            base.ItemActivate(used, buttonDown);
            if (playerHeldBy != null && buttonDown)
            {
                playerHeldBy.itemAudio.PlayOneShot(itemProperties.throwSFX);
                await Task.Delay(1100);
            }
            if (IsOwner)
            {
                if (IsHost)
                {
                    RPGExplosionSpawn(playerHeldBy.transform.position, playerHeldBy.transform.rotation);
                }
                else
                {
                    RPGExplosionSpawnServerRpc(playerHeldBy.transform.position, playerHeldBy.transform.rotation);
                }

                var player = StartOfRound.Instance.allPlayerScripts.FirstOrDefault(x => x.OwnerClientId == OwnerClientId);
                player.DespawnHeldObject();
            }
        }


        [ServerRpc(RequireOwnership = false)]
        private void RPGExplosionSpawnServerRpc(UnityEngine.Vector3 playerPosition, UnityEngine.Quaternion playerRotation)
        {
            RPGExplosionSpawn(playerPosition, playerRotation);
        }
        
        private void RPGExplosionSpawn(UnityEngine.Vector3 playerPosition, UnityEngine.Quaternion playerRotation)
        {

            NetworkObject rpgExplosion = Instantiate(itemProperties.spawnPrefab.GetComponent<NetworkObject>(), playerPosition, playerRotation);
            rpgExplosion.GetComponent<NetworkObject>().SpawnWithOwnership(OwnerClientId);
            
            rpgExplosion.GetComponent<RPGExplosion>().Explode();
            rpgExplosion.GetComponent<NetworkObject>().Despawn();

            
        }
    }
}
