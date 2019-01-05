using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingEnemy : MonoBehaviour {

public Transform throwPos;
public GameObject projectilePrefab;
public int damage;


public float timeBtwThrows;
float startTimeBtwThrows;
	
	void Start() {

	}

	// Update is called once per frame
	void FixedUpdate () {
		if (startTimeBtwThrows < 0 && this.GetComponent<Enemy>().player != null) {
			ThrowAtplayer(this.GetComponent<Enemy>().player.transform.position);
			startTimeBtwThrows = timeBtwThrows;
		} else {
			startTimeBtwThrows -= Time.fixedDeltaTime;
		}
	}

	void ThrowAtplayer(Vector2 playerPos) {
		GameObject projectile = Instantiate(projectilePrefab, throwPos.position, Quaternion.identity);
		projectile.GetComponent<Bullet>().damage = damage;
		//a = (v2 − u2 ) / 2x
		//v2 = 2ax
		//x = (v*v)t+.5at
		//v*v= x/t
		
		Vector2 velocity = VelocityCal(this.transform.position, playerPos, 1);

		projectile.GetComponent<Rigidbody2D>().velocity = velocity;
	}
	Vector2 VelocityCal(Vector2 origin, Vector2 target, float t) {

		Vector2 distance = target - origin;

		float sX;
		if (distance.x > 0) {
			sX = distance.x - 0.5f;
		} else {
			sX = distance.x + 0.5f;
		}
		float sY = distance.y;

		float vX = (sX / t);
		float vY = sY / t + 0.5f * Mathf.Abs(Physics2D.gravity.y) * t;
		
		return new Vector2(vX,vY);
	}
}
