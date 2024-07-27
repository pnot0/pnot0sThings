using GameNetcodeStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity.Netcode;
using UnityEngine;

namespace pnot0sThings.ItemBehaviour
{
    internal class RPGExplosion : NetworkBehaviour
    {

        public static void CreateExplosion(UnityEngine.Vector3 explosionPosition, bool spawnExplosionEffect = true, int damage = 50, float minDamageRange = 0f, float maxDamageRange = 10f, int enemyHitForce = 6, CauseOfDeath causeOfDeath = CauseOfDeath.Blast, PlayerControllerB attacker = null)
        {

            Transform holder = null;

            if (RoundManager.Instance != null && RoundManager.Instance.mapPropsContainer != null && RoundManager.Instance.mapPropsContainer.transform != null)
            {
                holder = RoundManager.Instance.mapPropsContainer.transform;
            }

            if (spawnExplosionEffect)
            {
                Instantiate(StartOfRound.Instance.explosionPrefab, explosionPosition, UnityEngine.Quaternion.Euler(-90f, 0f, 0f), holder).SetActive(value: true);
            }

            float num = UnityEngine.Vector3.Distance(GameNetworkManager.Instance.localPlayerController.transform.position, explosionPosition);
            if (num < 14f)
            {
                HUDManager.Instance.ShakeCamera(ScreenShakeType.Big);
            }
            else if (num < 25f)
            {
                HUDManager.Instance.ShakeCamera(ScreenShakeType.Small);
            }

            Collider[] array = Physics.OverlapSphere(explosionPosition, maxDamageRange, 2621448, QueryTriggerInteraction.Collide);
            PlayerControllerB playerControllerB = null;
            for (int i = 0; i < array.Length; i++)
            {
                float num2 = UnityEngine.Vector3.Distance(explosionPosition, array[i].transform.position);
                if (num2 > 4f && Physics.Linecast(explosionPosition, array[i].transform.position + UnityEngine.Vector3.up * 0.3f, 256, QueryTriggerInteraction.Ignore))
                {
                    continue;
                }

                if (array[i].gameObject.layer == 3)
                {
                    playerControllerB = array[i].gameObject.GetComponent<PlayerControllerB>();
                    if (playerControllerB != null && playerControllerB.IsOwner)
                    {
                        // calculate damage based on distance, so if player is minDamageRange or closer, they take full damage
                        // if player is maxDamageRange or further, they take no damage
                        // distance is num2
                        float damageMultiplier = 1f - Mathf.Clamp01((num2 - minDamageRange) / (maxDamageRange - minDamageRange));

                        playerControllerB.DamagePlayer((int)(damage * damageMultiplier), causeOfDeath: causeOfDeath);
                    }
                }
                else if (array[i].gameObject.layer == 21)
                {
                    Landmine componentInChildren = array[i].gameObject.GetComponentInChildren<Landmine>();
                    if (componentInChildren != null && !componentInChildren.hasExploded && num2 < 6f)
                    {
                    }
                }
                else if (array[i].gameObject.layer == 19)
                {
                    EnemyAICollisionDetect componentInChildren2 = array[i].gameObject.GetComponentInChildren<EnemyAICollisionDetect>();
                    if (componentInChildren2 != null && componentInChildren2.mainScript.IsOwner && num2 < 4.5f)
                    {
                        componentInChildren2.mainScript.HitEnemyOnLocalClient(enemyHitForce, playerWhoHit: attacker);
                    }
                }
            }

            int num3 = ~LayerMask.GetMask("Room");
            num3 = ~LayerMask.GetMask("Colliders");
            array = Physics.OverlapSphere(explosionPosition, 10f, num3);
            for (int j = 0; j < array.Length; j++)
            {
                Rigidbody component = array[j].GetComponent<Rigidbody>();
                if (component != null)
                {
                    component.AddExplosionForce(70f, explosionPosition, 10f);
                }
            }
        }

        [ServerRpc]
        public void ExplodeRPGServerRpc()
        {
            ExplodeRPG();
            ExplodeRPGClientRpc();
        }

        [ClientRpc]
        public void ExplodeRPGClientRpc()
        {
            ExplodeRPG();
        }

        public void ExplodeRPG()
        {
            CreateExplosion(transform.position, true, 100, 0f, 15f, 20, CauseOfDeath.Blast);
        }


        public void Explode()
        {
            if (IsHost)
            {
                ExplodeRPGClientRpc();
            }
            else
            {
                ExplodeRPGServerRpc();
            }
        }
    }
}
