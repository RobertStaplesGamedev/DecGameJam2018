using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeMonkey : MonoBehaviour {
	
    public Enemy enemyScript;

	[Header("Patrol")]
	public Transform frontDetection;
	public float wallDetectionDistance = 2f;
	RaycastHit2D hitInfoWall;
	RaycastHit2D hitInfoGround;
	public int playerCheckRadius = 0;
	public LayerMask whatIsPlayer;
    [HideInInspector] public GameObject playerDetect;
	public GameObject detector;
	
    [Header("Attack")]
    public float attackRadius = 1f;
    float timeBtwAttack;
    public float startTimeBtwAttack;
    [HideInInspector] public GameObject playerAttack;

	Rigidbody2D rb;

	bool isMovingRight;

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
		PlayerDetect();

		if (enemyScript.enemysettings.health <= 0) {
			Destroy(gameObject);
		}
	}

	void Move() {
		hitInfoWall = Physics2D.Raycast(frontDetection.position, enemyScript.enemysettings.direction, wallDetectionDistance);		
		hitInfoGround = Physics2D.Raycast(frontDetection.position, Vector2.down, 1);

		if ((enemyScript.enemysettings.canBeDazed && !enemyScript.enemysettings.isDazed) || !enemyScript.enemysettings.canBeDazed) {
			if (playerAttack != null) {
				Attack();
			} else if (isMovingRight) {
				if ((hitInfoWall && hitInfoWall.transform.gameObject.layer == 9) || !hitInfoGround || (playerDetect != null && playerDetect.transform.position.x < this.transform.position.x)) {
					isMovingRight = false;
					enemyScript.enemysettings.direction = Vector2.left;
					Flip();
				} else {
					rb.MovePosition(rb.position + (enemyScript.enemysettings.direction * enemyScript.enemysettings.speed * Time.fixedDeltaTime));
				}
			} else {
				if ((hitInfoWall && hitInfoWall.transform.gameObject.layer == 9) || !hitInfoGround || (playerDetect != null && playerDetect.transform.position.x > this.transform.position.x)) {
					isMovingRight = true;
					enemyScript.enemysettings.direction = Vector2.right;
					Flip();
				} else {
					rb.MovePosition(rb.position + (enemyScript.enemysettings.direction * enemyScript.enemysettings.speed * Time.fixedDeltaTime));
				}
			}
		} else if (enemyScript.enemysettings.canBeDazed && enemyScript.enemysettings.isDazed) {
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

    public void PlayerDetect() {
		Collider2D playerColliderDetect = Physics2D.OverlapCircle(detector.transform.position, playerCheckRadius, whatIsPlayer);
		Collider2D playerColliderAttack = Physics2D.OverlapCircle(detector.transform.position, attackRadius, whatIsPlayer);
		if (playerColliderDetect != null) {
			playerDetect = playerColliderDetect.gameObject;
		} else {
			playerDetect = null;
		}

		if (playerColliderAttack != null) {
			playerAttack = playerColliderDetect.gameObject;
		} else {
			playerAttack = null;
		}
	}

	// public void PlayerDetect() {
		
	// }

	public void Attack() {
		if (timeBtwAttack <=0) {
			//Play attack animation
			Collider2D playerDamage = Physics2D.OverlapCircle(detector.transform.position, attackRadius, whatIsPlayer);
			if (playerDamage.gameObject != null) {
				playerDamage.gameObject.GetComponentInParent<CharectarDamage>().TakeDamage(this.gameObject, enemyScript.enemysettings.damage);
				timeBtwAttack = startTimeBtwAttack;
			}
        } else {
            timeBtwAttack -= Time.deltaTime;
		}
	}

	void OnDrawGizmosSelected() {
		Gizmos.color = Color.red;
		Gizmos.DrawRay(frontDetection.position, enemyScript.enemysettings.direction);
		Gizmos.DrawRay(frontDetection.position, Vector2.down);

		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(this.transform.position, playerCheckRadius);

		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(detector.transform.position, attackRadius);
	}
}