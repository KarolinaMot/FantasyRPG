using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int damage;
    public string weaponName;
    public GameObject player;
    public CombatManager playerCombatManager;

    private void Awake()
    {
        playerCombatManager = player.GetComponent<CombatManager>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.tag == "Enemy" && playerCombatManager.isAttacking)
        {
            StatsManager enemyStats = collider.gameObject.GetComponent<StatsManager>();

            enemyStats.HandleTakenDamage(damage);
        }
    }
}
