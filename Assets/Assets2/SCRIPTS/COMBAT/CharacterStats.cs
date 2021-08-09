using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterStats : MonoBehaviour
{
    public int maxHealth; //Max health
    public int currentHealth;// current health
    public int oneHpWidth; //the width of one hp on the health bar
    public int hitWidth; //how much does a hit take off the health bar    

    public GameObject healthBar;
    public float healthBarWidth;
    public RectTransform healthBarTransform;

    private void Awake()
    {
      currentHealth = maxHealth;
      healthBarTransform = healthBar.GetComponent<RectTransform>();
      healthBarWidth = healthBarTransform.sizeDelta.x;
      oneHpWidth = (int)healthBarWidth / maxHealth;

    }

    public virtual void HandleTakenDamage(int damage)
    {

    }

  
}
