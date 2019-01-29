using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	public EnemySettings enemysettings;
	
	Rigidbody2D rb;

	void Start() {
		rb = GetComponent<Rigidbody2D>();
	}

	void FixedUpdate() {
		enemysettings.PlayerDetect(this.gameObject);
	}

	void Update () {
		if (enemysettings.health <= 0) {
			Destroy(gameObject);
		}
	}

    public void TakeDamage(GameObject source, GameObject hitObject, int thrust, int damage, bool knockback) {
		enemysettings.health -= damage;
		if (knockback && enemysettings.canBeKnockedback) {
			EnemyKnockback(source, this.gameObject, thrust);
		}
		if (enemysettings.canBeDazed) {
			enemysettings.isDazed	= true;
		}
		
		enemysettings.dazeTimeCountdown = enemysettings.dazeTime;
	}

	public void EnemyKnockback(GameObject source, GameObject hitObject, int thrust) {
		Vector2 knockbackDirection = source.transform.position - hitObject.transform.position;
		//left of the object
		if (knockbackDirection.x < 0 && knockbackDirection.x < knockbackDirection.y && knockbackDirection.x < -knockbackDirection.y) {
			knockbackDirection = Vector2.right;
		} 
		//right of the object
		else if (knockbackDirection.x > 0 && knockbackDirection.x > knockbackDirection.y && knockbackDirection.x > -knockbackDirection.y) {
			knockbackDirection = Vector2.left;
		}
		//below of the object
		else if (knockbackDirection.y < 0 && knockbackDirection.x > knockbackDirection.y && -knockbackDirection.x > knockbackDirection.y) {
			knockbackDirection = Vector2.up;
		}
		//above of the object
		else if (knockbackDirection.y > 0 && knockbackDirection.x < knockbackDirection.y && -knockbackDirection.x < knockbackDirection.y) {
			knockbackDirection = Vector2.down;
		}
		if (enemysettings.knockbackModifier > 0) {
			this.GetComponent<Rigidbody2D>().AddForce(knockbackDirection * (thrust/enemysettings.knockbackModifier));
		} else {
			this.GetComponent<Rigidbody2D>().AddForce(knockbackDirection * thrust);
		}
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.layer == 11 && collision.gameObject.GetComponent<CharectarDamage>().timeBtwinvincible <= 0)
		{
			collision.gameObject.GetComponent<CharectarDamage>().TakeDamage(this.gameObject, enemysettings.damage);
			Physics2D.IgnoreLayerCollision(10, 11, true);
		}
	}
}
