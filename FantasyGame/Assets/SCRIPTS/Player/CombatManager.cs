using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CombatManager : MonoBehaviour
{
    private Animator playerAnimator;
    private StarterAssetsInputs inputs;
    private ThirdPersonController thirdPersonController;
    private CharacterStats characterStats;
    private CharacterController ch;
    private SoundManager soundManager;

    private Collider nearestEnemy;
    private Collider lastNearestEnemy;

    [Header ("Enemy lock on")]
    public bool enemyDetected = false;
    public float enemyDetectionRadius = 10;
    public bool enemyChosen = false;
    		

    [Header ("Attack Animation Stuff")]
    public bool isAttacking = false;
    public bool checkForAttack = false;



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

        playerAnimator.SetBool("IsAttacking", isAttacking);
        CheckWeapon();
    }

    // Update is called once per frame
    void Update()
    {
            AttackAnimation();

            SearchNearbyEnemy();
    }

    private void AttackAnimation()
    {
        isAttacking = playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack1") || playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack2") || playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack3") || playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack4");
        if(!checkForAttack)
            playerAnimator.SetBool("NextAttackAllowed", false);
        if(inputs.attack)
        {
            inputs.attack = false;
            if(checkForAttack)
                playerAnimator.SetBool("NextAttackAllowed", true);
            if(!isAttacking)
            playerAnimator.SetTrigger("AttackInput");
        }
    }

    public void SearchNearbyEnemy(){
        Collider[] nearbyEnemies = Physics.OverlapSphere(transform.position, enemyDetectionRadius, enemyLayer);
        float nearestDistance = float.MaxValue;
        lastNearestEnemy = nearestEnemy;
        

        if(nearbyEnemies.Length>0){
            if(!enemyChosen){
                enemyDetected = true;
                inputs.disabledlockOn = false;
                if(inputs.lockOn && !thirdPersonController.isDashing && !thirdPersonController.isSprinting){
                    for(int i=0; i<nearbyEnemies.Length; i++){
                        float distance = (nearbyEnemies[i].transform.position - transform.position).sqrMagnitude;
                        if(distance < nearestDistance) {
                            nearestEnemy = nearbyEnemies[i];
                            nearestDistance = distance;
                        }
                    }
                    nearestEnemy.GetComponent<EnemyCombat>().EnableCanvas(true);
                    enemyChosen = true;
                }
                else{
                    if(nearestEnemy != null)
                        nearestEnemy.GetComponent<EnemyCombat>().EnableCanvas(false);
                }
            }
            else{
                if(inputs.lockOn && !thirdPersonController.isDashing && !thirdPersonController.isSprinting){
                    transform.LookAt(nearestEnemy.transform.position);
                    nearestEnemy.GetComponent<EnemyCombat>().EnableCanvas(true);
                    if(nearestEnemy.GetComponent<CharacterStats>().currentHp <= 0){
                        enemyChosen = false;
                        Destroy(nearestEnemy.gameObject);
                        inputs.lockOn = false;
                    }
                }
                else{
                    enemyChosen = false;
                    nearestEnemy.GetComponent<EnemyCombat>().EnableCanvas(false);
                }
            }
        }
        else{
            enemyDetected = false;
            enemyChosen = false;
            inputs.disabledlockOn = true;
            nearestEnemy.GetComponent<EnemyCombat>().EnableCanvas(false);
        }
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



    private void CheckHitBox(){
        Collider[] hitColliders = Physics.OverlapSphere(weapon.transform.position, characterStats.attackSize, enemyLayer);

        foreach(Collider col in hitColliders){
            ApplyDamage(col.gameObject);
        }
    }
    
    public void ApplyDamage(GameObject enemy)
    {
        if(!soundManager.hit.isPlaying){
          soundManager.hit.Play();
        } 
        else{
            soundManager.hit.Stop();
            soundManager.hit.Play();
        }
        CharacterStats enemyStats = enemy.GetComponent<CharacterStats>();
        enemyStats.currentHp -= characterStats.attack;
    }

    public void LookForNextAttack(){
        checkForAttack = true;
    }

    public void StopLookForAttack(){
        checkForAttack = false;
    }

}
