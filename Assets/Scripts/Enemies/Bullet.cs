using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	[HideInInspector] public int damage = 0;

	void OnCollisionEnter2D(Collision2D collision) {
		if (collision.gameObject.layer == 11 && collision.gameObject.GetComponent<CharectarDamage>().timeBtwinvincible <= 0)
		{
			collision.gameObject.GetComponent<CharectarDamage>().TakeDamage(this.gameObject, damage);
			Physics2D.IgnoreLayerCollision(10, 11, true);
		}
		Destroy(this.gameObject);
	}
}
