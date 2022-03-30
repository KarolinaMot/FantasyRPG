using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    private Animator playerAnimator;
    private StarterAssetsInputs inputs;
    private ThirdPersonController thirdPersonController;
    private CharacterStats characterStats;
    private CharacterController ch;
    private SoundManager soundManager;

    		

    [Header ("Attack Animation Stuff")]
    public AnimationClip lastAnimation;
    [SerializeField]
    private float maxTimeBetweenAttacks = 0;
    private float timer = 0;
    public bool ticking = false;
    [HideInInspector]
    public bool isAttacking = false;


    [Header("Weapon Stuff")]
    private WeaponStats weaponStats;
    public GameObject weaponSlot;
    public bool hasWeapon;
    private GameObject weapon;

    public LayerMask enemyLayer;


    // Start is called before the first frame update
    void Awake()
    {
        playerAnimator = GetComponent<Animator>(); 
        thirdPersonController = GetComponent<ThirdPersonController>();
        inputs = GetComponent<StarterAssetsInputs>();
        characterStats = GetComponent<CharacterStats>();
        ch = GetComponent<CharacterController>();
        soundManager = GetComponent<SoundManager>();

        playerAnimator.SetBool("IsAttacking", ticking);
        CheckWeapon();
    }

    // Update is called once per frame
    void Update()
    {
            AttackAnimation();
            Tick();
            if(thirdPersonController.moving)
                ticking=false;
            playerAnimator.SetBool("IsAttacking", ticking);

    }

    private void AttackAnimation()
    {
        isAttacking = playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack1") || playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack2") || playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack3") || playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack4");
        if (playerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.6f && ticking || timer>=maxTimeBetweenAttacks)
        {
            timer = 0;
            ticking = false;
            return;
        }

        if (playerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.3f && inputs.attack || playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle Walk Run Blend") && inputs.attack) {
            ticking = true;
            timer = 0;
        }

        if(inputs.attack)
          inputs.attack = false;
    }

    private void MoveOnAnimation(float speed)
    {
        ch.Move(transform.forward * speed); 
    }

    private void CheckWeapon()
    {
        if(weaponSlot.transform.childCount > 0)
            hasWeapon = true;

        if (hasWeapon)
        {
            weaponStats = weaponSlot.GetComponentInChildren<WeaponStats>();
            characterStats.attack += weaponStats.weaponAttack;
            weapon = weaponStats.transform.gameObject;
        }
    }

    private void Tick()
    {
        if(ticking)
         timer += Time.deltaTime;
    }

    private void CheckHitBox(){
        Collider[] hitColliders = Physics.OverlapSphere(weapon.transform.position, characterStats.attackSize, enemyLayer);

        foreach(Collider col in hitColliders){
            ApplyDamage(col.gameObject);
            if(!soundManager.hit.isPlaying){
                soundManager.hit.Play();
            }
        }
    }
    public void ApplyDamage(GameObject enemy)
    {
        CharacterStats enemyStats = enemy.GetComponent<CharacterStats>();
        enemyStats.currentHp -= characterStats.attack;

    }
}
