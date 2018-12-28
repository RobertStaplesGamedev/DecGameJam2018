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
		
		float vel = 10;
		float xDistance = playerPos.x - this.transform.position.x;
		// float xVel = xDistance / timeToTarget;
		// xVel = Mathf.Sqrt(-xVel);

		// float yDistance = playerPos.y - this.transform.position.y;
		// float yVel = 0.5f * (this.GetComponent<Rigidbody2D>().gravityScale * timeToTarget);

		// float angle = 0.5f * Mathf.Asin((this.GetComponent<Rigidbody2D>().gravityScale * xDistance) / (vel * vel));
		// angle = Mathf.Atan(angle);
		
		// Debug.Log(angle);
		// float xVel = vel * (Mathf.Sin(angle) * Mathf.Rad2Deg);
		// //Debug.Log(xVel);
		// float yVel = vel * (Mathf.Cos(angle) * Mathf.Rad2Deg);
		// //Debug.Log(yVel);
		Vector2 velocity = VelocityCal(this.transform.position, playerPos, Random.Range(0.5f, 2));
		// //Debug.Log(velocity);
		projectile.GetComponent<Rigidbody2D>().velocity = velocity;
	}
	Vector2 VelocityCal(Vector2 origin, Vector2 target, float t) {
		Vector2 distance = target - origin;

		float sX = distance.x;
		float sY = distance.y;

		float vX = (sX / t);
		float vY = sY / t + 0.5f * Mathf.Abs(Physics2D.gravity.y) * t;

		return new Vector2(vX,vY);
	}
}
