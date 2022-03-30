using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStats : MonoBehaviour
{
    
    private StarterAssetsInputs inputs;
    private ThirdPersonController thirdPersonController;

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

    [Header("HP Stuff")]
    public float maxHp = 1000;
    public float currentHp;
    public Image hpBarImage;
    public bool canTakeDamage = true;

    [Header("Combat stuff")]
    public float attack = 200;
    public float deffense;
    public float critRate;
    public float attackSize;

    // Start is called before the first frame update
    void Start()
    {
        inputs = GetComponent<StarterAssetsInputs>();
        thirdPersonController = GetComponent<ThirdPersonController>();
        stamina = maxStamina;
        currentHp = maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        ManageStamina();
        ManageHp();
    }

    void ManageStamina()
    {
        if(staminaBarImage != null)
        {
            if (inputs.dash && stamina > 0 && thirdPersonController.targetSpeed != 0)
            {
                stamina -= Time.deltaTime * 0.1f;
                if(thirdPersonController.isDashing)
                {
                    stamina -= Time.deltaTime * 0.8f;
                }
                staminaBarImage.fillAmount = 1 - stamina;
            }

            if (stamina <= 0)
            {
                slowedLocomotion = true;
                slowedLocomotionTimer++;

                if (slowedLocomotionTimer == slowedLocomotionTimerMax)
                {
                    slowedLocomotionTimer = 0;
                    slowedLocomotion = false;
                }
            }

            if (stamina < maxStamina && !inputs.dash && !slowedLocomotion)
            {
                stamina += Time.deltaTime * 0.1f;
                staminaBarImage.fillAmount = 1 - stamina;
            }
        }
    }

    void ManageHp()
    {
        if(hpBarImage != null)
        {
            hpBarImage.fillAmount = currentHp / maxHp; 
        }
    }

}
