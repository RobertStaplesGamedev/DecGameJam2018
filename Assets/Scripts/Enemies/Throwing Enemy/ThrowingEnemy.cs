using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingEnemy : MonoBehaviour {

public Transform throwPos;
public GameObject projectilePrefab;
public int damage;
public Animator animator;

[Header("Patrol")]
public bool detectPlayer;
public int playerCheckRadius = 0;
public LayerMask whatIsPlayer;
[HideInInspector] public GameObject playerDetect;

public Sprite sittingSprite;

public float timeBtwThrows;
float startTimeBtwThrows;

bool isSitting = false;

	// Update is called once per frame
	void FixedUpdate () {
		PlayerDetect(throwPos.gameObject);
		if (startTimeBtwThrows < 0 && playerDetect != null) {
			ThrowAtplayer(playerDetect.transform.position);
			startTimeBtwThrows = timeBtwThrows;
		} else {
			startTimeBtwThrows -= Time.fixedDeltaTime;
		}

		if (playerDetect != null) {
			if (!isSitting) {
				// animator.Play("Sit");
				// animator.SetBool("canSeePlayer", true);
				//this.GetComponentInChildren<Animation>().Play("Sit");
				//this.GetComponentInChildren<SpriteRenderer>().sprite = sittingSprite;
			}
			isSitting = true;
		} else {
			if (isSitting) {
				// animator.SetBool("canSeePlayer", false);
			}
			isSitting = false;
		}
	}

	public void PlayerDetect(GameObject Detector) {
		if (detectPlayer) {
			Collider2D playerCollider = Physics2D.OverlapCircle(Detector.transform.position, playerCheckRadius, whatIsPlayer);
			if (playerCollider != null) {
				playerDetect = playerCollider.gameObject;
			} else {
				playerDetect = null;
			}
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
	void OnDrawGizmosSelected() {
		Gizmos.color = Color.blue;
		if (detectPlayer) {
			Gizmos.DrawWireSphere(this.transform.position, playerCheckRadius);
		}
	}
}
