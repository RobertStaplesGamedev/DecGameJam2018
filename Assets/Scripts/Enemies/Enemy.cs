using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	[Header("Enemy Basics")]
	public int health;
	public GameObject model;

	[Header("Movement")]
	bool movingRight = false;
	Vector2 direction = Vector2.left;
	public float speed;


	[Header("Patrol")]
	public bool wallPatrol;
	public bool groundPatrol;
	public Transform frontDetection;
	public float wallDetectionDistance = 2f;
	RaycastHit2D hitInfoWall;
	RaycastHit2D hitInfoGround;

	[Header("Combat")]
	public bool canBeKnockedback;
	public float knockbackModifier;
	public bool canBeDazed;
	public float dazeTime;
	float dazeTimeCountdown;
	bool isDazed = false;
	
	void Update () {

		if (health <= 0) {
			Destroy(gameObject);
		}

		if (wallPatrol) {
			hitInfoWall = Physics2D.Raycast(frontDetection.position, direction, wallDetectionDistance);
		}
		
		if (groundPatrol) {
			hitInfoGround = Physics2D.Raycast(frontDetection.position, Vector2.down, 1);
		}

		if (!isDazed) {
			if (movingRight) {
				if ((hitInfoWall && hitInfoWall.transform.gameObject.layer == 9) || (!hitInfoGround && groundPatrol)) {
					movingRight = false;
					direction = Vector2.left;
					Flip();
				} else {
					transform.Translate(Vector2.right * speed * Time.deltaTime);
				}
			} else {
				if ((hitInfoWall && hitInfoWall.transform.gameObject.layer == 9) || (!hitInfoGround && groundPatrol)) {
					movingRight = true;
					direction = Vector2.right;
					Flip();
				} else {
					transform.Translate(Vector2.left * speed * Time.deltaTime);
				}
			}
		} else {
			if (dazeTimeCountdown < 0) {
				isDazed = false;
			} else {
				dazeTimeCountdown -= Time.deltaTime;
			}
		}
	}
	public void TakeDamage(GameObject source, int thrust, int damage, bool knockback) {
		health -= damage;
		if (knockback && canBeKnockedback) {
			Knockback(source, thrust);
		}
		if (canBeDazed) {
			isDazed	= true;
		}
		
		dazeTimeCountdown = dazeTime;
		//Debug.Log("damage taken");
	}

	void Knockback(GameObject source, int thrust) {
		Vector2 direction = source.transform.position - this.transform.position;
		float angle = Mathf.Atan2 (direction.y, direction.x);
		direction = new Vector2 (Mathf.Cos (angle), Mathf.Sin (angle));
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
		Debug.Log(direction);
		if (knockbackModifier > 0) {
			this.GetComponent<Rigidbody2D>().AddForce(direction * (thrust/knockbackModifier));
		} else {
			this.GetComponent<Rigidbody2D>().AddForce(direction * thrust);
		}
	}
	void Flip() {
		Vector3 scaler = model.transform.localScale;
		scaler.x *= -1f;
		model.transform.localScale = scaler;
	}

	void OnDrawGizmosSelected() {
		Gizmos.color = Color.red;
		Gizmos.DrawRay(frontDetection.position, direction);
		Gizmos.DrawRay(frontDetection.position, Vector2.down);
	}
}
