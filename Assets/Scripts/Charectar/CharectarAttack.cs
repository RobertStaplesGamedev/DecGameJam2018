using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharectarAttack : MonoBehaviour {
    float timeBtwAttack;
    public float startTimeBtwAttack;

    public Transform attackPos;
    public Animator AttackAnimator;

    public Weapon weapon;

    public LayerMask whatIsEnemies;

    void Update() {

        if (timeBtwAttack <=0) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                Collider2D[] enemiesToDamage = weapon.DrawAttackShape(attackPos.position, whatIsEnemies);
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
        Gizmos.color = Color.red;
        Vector2 center = new Vector2(attackPos.position.x + weapon.xOffset, attackPos.position.y + weapon.yOffset);
        if (weapon.attackShape == Weapon.AttackShape.Circle)
            Gizmos.DrawWireSphere(center, weapon.attackRange);
        else {
            Gizmos.DrawWireCube(center, new Vector3(weapon.horizontalRange, weapon.verticalRange));
        }
    }
}