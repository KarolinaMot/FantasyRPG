using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StatsManager : MonoBehaviour
{
    public int maxHealth; //Max health
    int currentHealth;// current health
    int oneHpWidth; //the width of one hp on the health bar
    int hitWidth; //how much does a hit take off the health bar
    
    

    public GameObject healthBar;
    float healthBarWidth;
    RectTransform healthBarTransform;
    
    private void Awake()
    {
      currentHealth = maxHealth;
      healthBarTransform = healthBar.GetComponent<RectTransform>();
      healthBarWidth = healthBarTransform.sizeDelta.x;
      oneHpWidth = (int)healthBarWidth / maxHealth;
      
        
    }

    public void HandleTakenDamage(int damage)
    {
        healthBarWidth = healthBarTransform.sizeDelta.x;
        currentHealth -= damage;
        hitWidth = damage * oneHpWidth;
        healthBarTransform.sizeDelta = new Vector2(healthBarWidth - hitWidth, healthBarTransform.sizeDelta.y);        

        if(currentHealth <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
