using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{

    public GameObject enemy;
    public override void HandleTakenDamage(int damage)
    {
        healthBarWidth = healthBarTransform.sizeDelta.x;
        currentHealth -= damage;
        hitWidth = damage * oneHpWidth;
        healthBarTransform.sizeDelta = new Vector2(healthBarWidth - hitWidth, healthBarTransform.sizeDelta.y);

        if (currentHealth <= 0)
        {
            Destroy(enemy);
        }
    }
}
