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

    [Header("Combat")]
	public int damage;
	public bool canBeKnockedback;
	public float knockbackModifier;
	public bool canBeDazed;
	public float dazeTime;
	public float dazeTimeCountdown;
	public bool isDazed = false;

}