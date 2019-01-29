using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/Enemy Settings")]
public class EnemySettings : UpdatableData {
   
    [Header("Enemy Basics")]
	public int health;
	public GameObject model;

	[Header("Movement")]
	public bool movingRight = false;
	public Vector2 direction;
	public float speed;

    [Header("Patrol")]
    public bool detectPlayer;
	public int playerCheckRadius = 0;
	public LayerMask whatIsPlayer;
    [HideInInspector] public GameObject player;

    [Header("Combat")]
	public int damage;
	public bool canBeKnockedback;
	public float knockbackModifier;
	public bool canBeDazed;
	public float dazeTime;
	public float dazeTimeCountdown;
	public bool isDazed = false;

    public void PlayerDetect(GameObject Detector) {
		if (detectPlayer) {
			Collider2D playerCollider = Physics2D.OverlapCircle(Detector.transform.position, playerCheckRadius, whatIsPlayer);
			if (playerCollider != null) {
				player = playerCollider.gameObject;
			} else {
				player = null;
			}
		}
	}
}