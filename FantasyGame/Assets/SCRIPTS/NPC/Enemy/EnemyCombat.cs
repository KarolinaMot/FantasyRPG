using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCombat : MonoBehaviour
{
    CharacterStats characterStats;
    public GameObject lockOnImages;

    // Start is called before the first frame update
    void Start()
    {
        characterStats = GetComponent<CharacterStats>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnableCanvas(bool enable){
        lockOnImages.SetActive(enable);
    }
    public void TakeDamage(float damage)
    {
        characterStats.currentHp -= damage;
    }
}
