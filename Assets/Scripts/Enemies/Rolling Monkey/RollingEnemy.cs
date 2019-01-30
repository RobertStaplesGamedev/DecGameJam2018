using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingEnemy : MonoBehaviour {

	public Enemy enemyScript;

	[Header("Patrol")]
	public bool wallPatrol;
	public bool groundPatrol;
	public Transform frontDetection;
	public float wallDetectionDistance = 2f;
	RaycastHit2D hitInfoWall;
	RaycastHit2D hitInfoGround;
	
	Rigidbody2D rb;

	void Start() {
		rb = GetComponent<Rigidbody2D>();
	}

	void FixedUpdate() {
		Move();
	}

	void Update () {
		if (enemyScript.enemysettings.health <= 0) {
			Destroy(gameObject);
		}
	}

	void Move() {
		if (wallPatrol) {
			hitInfoWall = Physics2D.Raycast(frontDetection.position, enemyScript.enemysettings.direction, wallDetectionDistance);
		}
		
		if (groundPatrol) {
			hitInfoGround = Physics2D.Raycast(frontDetection.position, Vector2.down, 1);
		}

		if (!enemyScript.enemysettings.isDazed) {
			if (enemyScript.enemysettings.movingRight) {
				if ((hitInfoWall && hitInfoWall.transform.gameObject.layer == 9) || (!hitInfoGround && groundPatrol)) {
					enemyScript.enemysettings.movingRight = false;
					enemyScript.enemysettings.direction = Vector2.left;
					Flip();
				} else {
					rb.MovePosition(rb.position + (Vector2.right * enemyScript.enemysettings.speed * Time.fixedDeltaTime));
				}
			} else {
				if ((hitInfoWall && hitInfoWall.transform.gameObject.layer == 9) || (!hitInfoGround && groundPatrol)) {
					enemyScript.enemysettings.movingRight = true;
					enemyScript.enemysettings.direction = Vector2.right;
					Flip();
				} else {
					rb.MovePosition(rb.position + (Vector2.left * enemyScript.enemysettings.speed * Time.fixedDeltaTime));
				}
			}
		} else {
			if (enemyScript.enemysettings.dazeTimeCountdown < 0) {
				enemyScript.enemysettings.isDazed = false;
			} else {
				enemyScript.enemysettings.dazeTimeCountdown -= Time.fixedDeltaTime;
			}
		}
	}

	void Flip() {
		Vector3 scaler = enemyScript.enemysettings.model.transform.localScale;
		scaler.x *= -1f;
		enemyScript.enemysettings.model.transform.localScale = scaler;
	}

	void OnDrawGizmosSelected() {
		Gizmos.color = Color.red;
		if (wallPatrol) {
			Gizmos.DrawRay(frontDetection.position, enemyScript.enemysettings.direction);
		}
		if (groundPatrol) {
			Gizmos.DrawRay(frontDetection.position, Vector2.down);
		}
	}
}
