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

	bool isMovingRight;

	Rigidbody2D rb;

	void Start() {
		rb = GetComponent<Rigidbody2D>();
		isMovingRight = enemyScript.enemysettings.movingRight;
		if (isMovingRight) {
			enemyScript.enemysettings.direction = Vector2.right;
		} else {
			enemyScript.enemysettings.direction = Vector2.left;
		}
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
			if (isMovingRight) {
				if ((hitInfoWall && hitInfoWall.transform.gameObject.layer == 9) || (!hitInfoGround && groundPatrol)) {
					isMovingRight = false;
					enemyScript.enemysettings.direction = Vector2.left;
					Flip();
				} else {
					rb.MovePosition(rb.position + (enemyScript.enemysettings.direction * enemyScript.enemysettings.speed * Time.fixedDeltaTime));
				}
			} else {
				if ((hitInfoWall && hitInfoWall.transform.gameObject.layer == 9) || (!hitInfoGround && groundPatrol)) {
					isMovingRight = true;
					enemyScript.enemysettings.direction = Vector2.right;
					Flip();
				} else {
					rb.MovePosition(rb.position + (enemyScript.enemysettings.direction * enemyScript.enemysettings.speed * Time.fixedDeltaTime));
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
		Vector3 scaler = enemyScript.model.transform.localScale;
		scaler.x *= -1f;
		enemyScript.model.transform.localScale = scaler;
	}

	void OnDrawGizmosSelected() {
		Gizmos.color = Color.red;
		Vector2 directionDraw;
		if (isMovingRight) {
			directionDraw = Vector2.right;
		} else {
			directionDraw = Vector2.left;
		}

		if (wallPatrol) {
			Gizmos.DrawRay(frontDetection.position, directionDraw);
		}
		if (groundPatrol) {
			Gizmos.DrawRay(frontDetection.position, Vector2.down);
		}
	}
}
