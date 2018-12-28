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
	public bool detectPlayer;
	public int playerCheckRadius = 0;
	public LayerMask whatIsPlayer;
	[HideInInspector] public GameObject player;


	[Header("Combat")]
	public int enemyDamage;
	public bool canBeKnockedback;
	public float knockbackModifier;
	public bool canBeDazed;
	public float dazeTime;
	float dazeTimeCountdown;
	bool isDazed = false;
	
	Rigidbody2D rb;

	void Start() {
		rb = GetComponent<Rigidbody2D>();
	}

	void FixedUpdate() {
		PlayerDetect();
		Move();
	}

	void Update () {
		if (health <= 0) {
			Destroy(gameObject);
		}
	}
	void PlayerDetect() {
		if (detectPlayer) {
			Collider2D playerCollider = Physics2D.OverlapCircle(this.transform.position, playerCheckRadius, whatIsPlayer);
			if (playerCollider != null) {
				player = playerCollider.gameObject;
			} else {
				player = null;
			}
		}
	}

	void Move() {
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
					rb.MovePosition(rb.position + (Vector2.right * speed * Time.fixedDeltaTime));
				}
			} else {
				if ((hitInfoWall && hitInfoWall.transform.gameObject.layer == 9) || (!hitInfoGround && groundPatrol)) {
					movingRight = true;
					direction = Vector2.right;
					Flip();
				} else {
					rb.MovePosition(rb.position + (Vector2.left * speed * Time.fixedDeltaTime));
				}
			}
		} else {
			if (dazeTimeCountdown < 0) {
				isDazed = false;
			} else {
				dazeTimeCountdown -= Time.fixedDeltaTime;
			}
		}
	}

	public void TakeDamage(GameObject source, int thrust, int damage, bool knockback) {
		health -= damage;
		Debug.Log("test");
		if (knockback && canBeKnockedback) {
			Knockback(source, thrust);
		}
		if (canBeDazed) {
			isDazed	= true;
		}
		
		dazeTimeCountdown = dazeTime;
	}

	void Knockback(GameObject source, int thrust) {
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
		if (wallPatrol) {
			Gizmos.DrawRay(frontDetection.position, direction);
		}
		if (groundPatrol) {
			Gizmos.DrawRay(frontDetection.position, Vector2.down);
		}

		Gizmos.color = Color.blue;
		if (detectPlayer) {
			Gizmos.DrawWireSphere(this.transform.position, playerCheckRadius);
		}
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.layer == 11 && collision.gameObject.GetComponent<CharectarDamage>().timeBtwinvincible <= 0)
		{
			collision.gameObject.GetComponent<CharectarDamage>().TakeDamage(this.gameObject, enemyDamage);
			Physics2D.IgnoreLayerCollision(10, 11, true);
		}
	}
}
