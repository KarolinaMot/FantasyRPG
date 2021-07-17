using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int damage;
    public string weaponName;
    public GameObject player;
    public CombatManager playerCombatManager;
    public bool isGettingAttacked;

    private void Awake()
    {
        playerCombatManager = player.GetComponent<CombatManager>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.tag == "Enemy" && playerCombatManager.isAttacking)
        {
            CharacterStats colliderStats = collider.gameObject.GetComponent<CharacterStats>(); //the stats of the object that got hit

            colliderStats.HandleTakenDamage(damage);
        }
    }
}
