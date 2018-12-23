using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharectarAttack : MonoBehaviour {
    float timeBtwAttack;
    public float startTimeBtwAttack;

    public Transform attackPosHorizontal;
    Transform attackPos;
    public Transform attackPosUp;
    public Transform attackPosDown;

    int angle = 0;
    int looking = 0;

    public Animator AttackAnimator;
    public Weapon weapon;
    public LayerMask whatIsEnemies;

    void Update() {
        bool isLookingdown = this.GetComponent<CharectarMovement>().isLookingdown;
        bool isLookingup = this.GetComponent<CharectarMovement>().isLookingup;
        bool isRight = this.GetComponent<CharectarMovement>().isRight;
        if (isLookingdown) {
            attackPosHorizontal.gameObject.SetActive(false);
            attackPosUp.gameObject.SetActive(false);
            attackPosDown.gameObject.SetActive(true);
            attackPos = attackPosDown;
            angle = 90;
           looking = 2;
        } else if (isLookingup)
        {
            attackPosHorizontal.gameObject.SetActive(false);
            attackPosUp.gameObject.SetActive(true);
            attackPosDown.gameObject.SetActive(false);
            attackPos = attackPosUp;
            angle = 90;
            looking = 1;
        } else if (!isRight){
            attackPosHorizontal.gameObject.SetActive(true);
            attackPosUp.gameObject.SetActive(false);
            attackPosDown.gameObject.SetActive(false);
            attackPos = attackPosHorizontal;
            angle = 0;
            looking = 3;
        } else {
            attackPosHorizontal.gameObject.SetActive(true);
            attackPosUp.gameObject.SetActive(false);
            attackPosDown.gameObject.SetActive(false);
            attackPos = attackPosHorizontal;
            angle = 0;
            looking = 0;
        }

        if (timeBtwAttack <=0) {
            if (Input.GetButtonDown("Fire1")) {
                Collider2D[] enemiesToDamage = weapon.DrawAttackShape(attackPos.position, angle, whatIsEnemies, looking);
                AttackAnimator.Play("Attack");
                for (int i = 0; i< enemiesToDamage.Length; i++) {
                    enemiesToDamage[i].GetComponent<Enemy>().TakeDamage(attackPos.gameObject, 100, weapon.damage, weapon.knockback);
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
            attackPosTemp = attackPosHorizontal;
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