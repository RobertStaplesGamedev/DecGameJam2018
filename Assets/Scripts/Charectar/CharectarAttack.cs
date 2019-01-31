using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharectarAttack : MonoBehaviour {
    float timeBtwAttack;
    public float startTimeBtwAttack;

    Transform attackPos;

    int angle = 0;
    int looking = 0;

    Animator AttackAnimator;
    public Weapon weapon;
    public LayerMask whatIsEnemies;

    [Header("Horizontal")]
    public Transform horizontalAttackPos;
    public Animator horizontalAnimator;

    [Header("Up")]
    public Transform upAttackPos;
    public Animator upAnimator;

    [Header("Down")]
    public Transform downAttackPos;
    public Animator downAnimator;

    void Start() {
        horizontalAnimator.runtimeAnimatorController = weapon.weaponAnimation;
        upAnimator.runtimeAnimatorController = weapon.weaponAnimation;
        downAnimator.runtimeAnimatorController = weapon.weaponAnimation;
    }


    void Update() {
        bool isLookingdown = this.GetComponent<CharectarMovement>().isLookingdown;
        bool isLookingup = this.GetComponent<CharectarMovement>().isLookingup;
        bool isRight = this.GetComponent<CharectarMovement>().isRight;
        if (isLookingdown) {
            horizontalAttackPos.gameObject.SetActive(false);
            upAttackPos.gameObject.SetActive(false);
            downAttackPos.gameObject.SetActive(true);
            attackPos = downAttackPos;
            angle = 90;
           looking = 2;
           AttackAnimator = downAnimator;
        } else if (isLookingup)
        {
            horizontalAttackPos.gameObject.SetActive(false);
            upAttackPos.gameObject.SetActive(true);
            downAttackPos.gameObject.SetActive(false);
            attackPos = upAttackPos;
            angle = 90;
            looking = 1;
            AttackAnimator = upAnimator;
        } else if (!isRight){
            horizontalAttackPos.gameObject.SetActive(true);
            upAttackPos.gameObject.SetActive(false);
            downAttackPos.gameObject.SetActive(false);
            attackPos = horizontalAttackPos;
            angle = 0;
            looking = 3;
            AttackAnimator = horizontalAnimator;
        } else {
            horizontalAttackPos.gameObject.SetActive(true);
            upAttackPos.gameObject.SetActive(false);
            downAttackPos.gameObject.SetActive(false);
            attackPos = horizontalAttackPos;
            angle = 0;
            looking = 0;
            AttackAnimator = horizontalAnimator;
        }

        if (timeBtwAttack <=0) {
            if (Input.GetButtonDown("Fire1")) {
                Collider2D[] enemiesToDamage = weapon.DrawAttackShape(attackPos.position, angle, whatIsEnemies, looking);
                AttackAnimator.Play("Attack");
                for (int i = 0; i< enemiesToDamage.Length; i++) {
                    enemiesToDamage[i].GetComponent<Enemy>().TakeDamage(attackPos.gameObject, weapon.thrust, weapon.damage, weapon.knockback);
                }
                timeBtwAttack = startTimeBtwAttack;
            }
        } else {
            timeBtwAttack -= Time.deltaTime;
        }
    }

    void OnDrawGizmos() {
        Transform attackPosTemp;
        Gizmos.color = Color.red;
        if (attackPos) {
            attackPosTemp = attackPos;
        } else {
            attackPosTemp = horizontalAttackPos;
        }
        Vector2 center = weapon.SetAttackShape(attackPosTemp.position, angle, looking);
 
        if (weapon.attackShape == Weapon.AttackShape.Circle)
            Gizmos.DrawWireSphere(center, weapon.attackRange);
        else {
            if (angle == 90) {
                Gizmos.DrawWireCube(center, new Vector3(weapon.verticalRange, weapon.horizontalRange));
            } else {
                Gizmos.DrawWireCube(center, new Vector3(weapon.horizontalRange, weapon.verticalRange));
            }
        }
    }
}