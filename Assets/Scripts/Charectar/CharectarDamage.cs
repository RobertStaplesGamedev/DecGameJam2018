using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharectarDamage : MonoBehaviour {
	
	[HideInInspector] public float timeBtwinvincible = 0;
	public float  startTimeBtwinvincible;
	public int health = 10;
	public int knockbackForce = 1;

	Rigidbody2D rb;

	void Start() {
		rb = GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (timeBtwinvincible > 0) {
			timeBtwinvincible -= Time.fixedDeltaTime;
		} else {
			Physics2D.IgnoreLayerCollision(10, 11, false);
		}
	}
	
	public void TakeDamage(GameObject source, int damage) {
		health -= damage;
		timeBtwinvincible = startTimeBtwinvincible;
		Knockback(source);
	}
	void Knockback(GameObject source) {
		Vector2 direction = this.transform.position - source.transform.position;
		if (direction.x > 1) {
			direction = (new Vector2(1,1) * knockbackForce);
			rb.AddForce(direction);
		} else {
			direction = (new Vector2(-1,1) * knockbackForce);
			rb.AddForce(direction);
		}
		Debug.Log(direction);
	}
}
