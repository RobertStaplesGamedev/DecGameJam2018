using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Inventory/Weapon", order = 1)]
public class Weapon : ScriptableObject {
	
	public string weaponName;

	public enum AttackShape {Circle, Rectangle};
	public AttackShape attackShape;

	public float xOffset;
	public float yOffset;

	[Header("Circle")]
	public float attackRange;

	[Header("Rectangle")]
	public float horizontalRange;
	public float verticalRange;

    public int damage;
    public bool knockback;

	public Collider2D[] DrawAttackShape(Vector2 center, LayerMask whatIsEnemies) {
		center = new Vector2(center.x + xOffset, center.y + yOffset);
		if (attackShape == AttackShape.Circle) {
			return Physics2D.OverlapCircleAll(center, attackRange, whatIsEnemies);
		} else {
			return Physics2D.OverlapBoxAll(center, new Vector2(horizontalRange, verticalRange), 0, whatIsEnemies);
		}
	}
}
