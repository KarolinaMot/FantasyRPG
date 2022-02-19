using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    
    private StarterAssetsInputs inputs;

    [Header("Stamina Stuff")]
    public float stamina;
    private float maxStamina = 1;
    public bool slowedLocomotion = false;
    [SerializeField]
    private Image staminaBarImage;
    [SerializeField]
    private float slowedLocomotionTimerMax = 100;
    [SerializeField]
    private int slowedLocomotionTimer = 0;

    // Start is called before the first frame update
    void Start()
    {
        inputs = GetComponent<StarterAssetsInputs>();
        stamina = maxStamina;
    }

    // Update is called once per frame
    void Update()
    {
        ManageStamina();
    }

    void ManageStamina()
    {
        if (inputs.sprint && stamina >0)
        {
            stamina -= Time.deltaTime * 0.1f;
            staminaBarImage.fillAmount = 1 - stamina;
        }

        if(stamina <=0)
        {
            slowedLocomotion = true;
            slowedLocomotionTimer++;

            if(slowedLocomotionTimer == slowedLocomotionTimerMax)
            {
                slowedLocomotionTimer = 0; 
                slowedLocomotion = false;
            }  
        }

        if (stamina < maxStamina && !inputs.sprint && !slowedLocomotion)
        {
            stamina += Time.deltaTime * 0.1f;
            staminaBarImage.fillAmount = 1 - stamina;
        }

    }


}
