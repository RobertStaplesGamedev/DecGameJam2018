using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	public int health;
	public float speed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if(health <= 0) {
			Destroy(gameObject);
		}

		transform.Translate(Vector2.left * speed * Time.deltaTime);
	}
	public void TakeDamage(GameObject source, int thrust, int damage, bool knockback) {
		health -= damage;
		if (knockback) {
			Knockback(source, thrust);
		}
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
			direction = Vector2.up;
		}
		Debug.Log(direction);
		this.GetComponent<Rigidbody2D>().AddForce(direction * thrust);
	}
}
