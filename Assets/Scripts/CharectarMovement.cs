using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharectarMovement : MonoBehaviour {

	bool isGround;
	bool isRight = true;

	GameObject collidedObject;

	public float moveSpeed = 40f;
	public float jumpForce = 10f;
	float moveInput;

	Rigidbody2D rb;

	public Transform groundCheck;
	public float checkRadius;
	public LayerMask whatIsGround;

	public int extraJumps = 1;
	private int jumps = 1;

	private int collectedObjects;

	public Animator animator;
	public GameObject charectarModel;
	public TextMeshProUGUI Collectedferns;

	void Start() {
		rb = GetComponent<Rigidbody2D>();
	}

	void OnTriggerStay2D(Collider2D other) {
		if (other.gameObject.GetComponent<CollectObject>() != null) {
			collidedObject = other.gameObject;
		} else {
			return;
		}
	}
	void OnTriggerExit2D(Collider2D other) {
		if (other.gameObject.GetComponent<CollectObject>() != null) {
			collidedObject = null;
		} else {
			return;
		}
	}

	void Update()
	{
		if (isGround) {
			jumps = extraJumps;
		}
		if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)) && jumps > 0 ) {
			rb.velocity = Vector2.up * jumpForce;
			jumps--;
		} else if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)|| Input.GetKeyDown(KeyCode.Space)) && jumps == 0 && isGround) {
			rb.velocity = Vector2.up * jumpForce;
		}
		if (Input.GetKeyDown(KeyCode.E) && collidedObject != null) {
			Destroy(collidedObject.gameObject);
			collectedObjects++;
			Collectedferns.text = collectedObjects.ToString();
		}
	}

	void FixedUpdate () {

		isGround = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

		animator.SetBool("isGround", isGround);

		moveInput = Input.GetAxis("Horizontal");
		rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
		if (isRight == false && moveInput > 0) {
			Flip();
		} else if (isRight == true && moveInput < 0) {
			Flip();
		}
		bool isMoving = false;
		if (moveInput > 0.01f || moveInput < -0.01f) {
			isMoving = true;
		} else {
			isMoving = false;
		}
		 

		animator.SetBool("isMoving", isMoving);
	}
	void Flip() {
		isRight = !isRight;
		Vector3 scaler = charectarModel.transform.localScale;
		scaler.x *= -1f;
		charectarModel.transform.localScale = scaler;
	}
}
