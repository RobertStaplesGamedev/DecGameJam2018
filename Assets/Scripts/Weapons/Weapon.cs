using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Inventory/Weapon", order = 1)]
public class Weapon : ScriptableObject {
	
	public string weaponName;
	public int thrust;

	public enum AttackShape {Circle, Rectangle};
	public AttackShape attackShape;

	[Header("Circle")]
	public float attackRange;

	[Header("Rectangle")]
	public float horizontalRange;
	public float verticalRange;

	[Header("Offsetting")]
	public float xOffsetHorizontal;
	public float yOffsetHorizontal;
	public float xOffsetUp;
	public float yOffsetUp;
	public float xOffsetDown;
	public float yOffsetDown;


    public int damage;
    public bool knockback;

	public Collider2D[] DrawAttackShape(Vector2 center, int angle, LayerMask whatIsEnemies, int looking=0) {
		center = SetAttackShape(center, angle, looking);
		if (attackShape == AttackShape.Circle) {
			return Physics2D.OverlapCircleAll(center, attackRange, whatIsEnemies);
		} else {
			return Physics2D.OverlapBoxAll(center, new Vector2(horizontalRange, verticalRange), angle, whatIsEnemies);
		}
	}
	public Vector2 SetAttackShape(Vector2 center, int angle, int looking=0) {
		if (looking == 2) {
			return new Vector2(center.x + xOffsetUp, center.y + yOffsetUp);
		} else if (looking == 1) {
			return new Vector2(center.x + xOffsetDown, center.y + yOffsetDown);
		} else if (looking == 3) {
			return new Vector2(center.x + -xOffsetHorizontal, center.y + yOffsetHorizontal);
		} else {
			return new Vector2(center.x + xOffsetHorizontal, center.y + yOffsetHorizontal);
		}
	}
}
