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
		} else if (collision.gameObject.layer == 10) {
			Physics2D.IgnoreLayerCollision(10, 10, true);
		}
		Destroy(this.gameObject);
	}

	void Update ()
	{
    	transform.Rotate (0,0,180*Time.deltaTime); //rotates 50 degrees per second around z axis
	}
}
