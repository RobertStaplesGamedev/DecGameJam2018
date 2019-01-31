using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	public EnemySettings enemysettings;
	public GameObject model;

	[HideInInspector] public int health;

	Rigidbody2D rb;

	void Start() {
		rb = GetComponent<Rigidbody2D>();
		health = enemysettings.health;
	}

	void Update () {
		if (health <= 0) {
			Destroy(gameObject);
		}
	}

    public void TakeDamage(GameObject source, int thrust, int damage, bool knockback) {
		health -= damage;
		if (knockback && enemysettings.canBeKnockedback) {
			EnemyKnockback(source, thrust);
		}
		if (enemysettings.canBeDazed) {
			enemysettings.isDazed = true;
		}
		
		enemysettings.dazeTimeCountdown = enemysettings.dazeTime;
	}

	public void EnemyKnockback(GameObject source, int thrust) {
		Vector2 direction = source.transform.position - this.transform.position;
		//left of the object
		if (direction.x < 0 && direction.x < direction.y && direction.x < -direction.y) {
			direction = Vector2.right;
		} 
		//right of the object
		else if (direction.x > 0 && direction.x > direction.y && direction.x > -direction.y) {
			direction = Vector2.left;
		}
		//below of the object
		else if (direction.y < 0 && direction.x > direction.y && -direction.x > direction.y) {
			direction = Vector2.up;
		}
		//above of the object
		else if (direction.y > 0 && direction.x < direction.y && -direction.x < direction.y) {
			direction = Vector2.down;
		}
		if (enemysettings.knockbackModifier > 0) {
			rb.AddForce(direction * (thrust/enemysettings.knockbackModifier));
		} else {
			rb.AddForce(direction * thrust);
		}
	}

	void OnCollisionEnter2D(Collision2D collision) {
		if (collision.gameObject.layer == 11 && collision.gameObject.GetComponent<CharectarDamage>().timeBtwinvincible <= 0)
		{
			collision.gameObject.GetComponent<CharectarDamage>().TakeDamage(this.gameObject, enemysettings.damage);
			Physics2D.IgnoreLayerCollision(10, 11, true);
		}
	}
}
