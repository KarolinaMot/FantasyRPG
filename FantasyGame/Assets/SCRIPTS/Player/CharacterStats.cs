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
    [SerializeField]
    private Image staminaBarImage;

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
            if (inputs.dash && stamina > 0 && thirdPersonController.moving)
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
                inputs.sprintDisabled = true;
                inputs.dash = false;
            }

            if (stamina < maxStamina && (!inputs.dash || inputs.sprintDisabled))
            {
                stamina += Time.deltaTime * 0.1f;
                staminaBarImage.fillAmount = 1 - stamina;
            }
            if(inputs.sprintDisabled && stamina>=1){
                inputs.sprintDisabled = false;
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
