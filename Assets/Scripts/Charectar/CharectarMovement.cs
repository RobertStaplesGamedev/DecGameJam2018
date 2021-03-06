﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharectarMovement : MonoBehaviour {

	[HideInInspector] public GameObject collidedObject;
	[Header("Model")]
	public Animator animator;
	public GameObject charectarModel;

	[Header("Moving")]
	public float moveSpeed = 40f;
	public float jumpForce = 10f;
	float moveInput;
	float lookInput;

	Rigidbody2D rb;

	[Header("Jumping")]
	public Transform groundCheck;
	bool isGround;
	bool isJumping;
	public float checkRadius;
	public LayerMask whatIsGround;
	public int extraJumps = 1;
	int jumps = 0;
	public float jumpTime;
	float jumpTimeCounter;
	public bool haveJumpRest;
	bool isRested = true;
	public float restTime;
	float restTimeCounter;

	[HideInInspector] public bool isRight = true;
	[HideInInspector] public bool isLookingup = false;
	[HideInInspector] public bool isLookingdown = false;


	void Start() {
		rb = GetComponent<Rigidbody2D>();
		restTimeCounter = -1;
	}

	void OnTriggerStay2D(Collider2D other) {
		if (other.gameObject != null) {
			collidedObject = other.gameObject;
		}
	}
	void OnTriggerExit2D(Collider2D other) {
		if (other.gameObject != null) {
			collidedObject = null;
		}
	}

	void Update()
	{
		lookInput = Input.GetAxis("Vertical");
		Look();
		Jump();
	}

	void FixedUpdate () {

		animator.SetBool("isGround", isGround);

		moveInput = Input.GetAxis("Horizontal");
		if (moveInput > 0 || moveInput < 0) {
			Move();
		}
	}

	void Look() {
		if (lookInput > 0) {
			isLookingup = true;
			isLookingdown = false;
		} else if (lookInput < 0) {
			isLookingdown = true;
			isLookingup = false;
		} else {
            isLookingdown = false;
			isLookingup = false;
		}
	}

	void Move() {
		if (rb.velocity.x < moveSpeed && rb.velocity.x > -moveSpeed) {
			rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
		}

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

	void Jump() {

		isGround = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

		if (isGround) {
			if (haveJumpRest) {
				if (restTimeCounter < 0) {
					restTimeCounter = restTime;
					isRested = true;
					jumps = extraJumps;
					jumpTimeCounter = jumpTime;
				} else if (restTimeCounter > 0 && !isRested) {
					restTimeCounter -= Time.deltaTime;
				}
			} else {
				jumps = extraJumps;
				jumpTimeCounter = jumpTime;
			}
		}

		if (Input.GetButtonDown("Jump")) {
			if (jumps > 0) {
				isJumping = true;
				jumpTimeCounter = jumpTime;
			} 
			else if (jumps == 0) {
				isJumping = true;
				jumpTimeCounter = jumpTime;
			}
		} 
		
		if (Input.GetButton("Jump") && isJumping && jumps >= 0) {
			if (haveJumpRest) {
				if (isRested) {
					if (jumpTimeCounter > 0) {
						rb.velocity = Vector2.up * jumpForce;
						jumpTimeCounter -= Time.deltaTime;
					} else {
						isJumping = false;
					}
				}
			} else {
				if (jumpTimeCounter > 0) {
					rb.velocity = Vector2.up * jumpForce;
					jumpTimeCounter -= Time.deltaTime;
				} else {
					isJumping = false;
				}
			}
		}

		if (Input.GetButtonUp("Jump")) {
			if (jumps < 0) {
				isRested = false;
				isJumping = false;
			} else if (jumps >= 0) {
				jumps--;
			}
		}
	}

	void Flip() {
		isRight = !isRight;
		Vector3 scaler = charectarModel.transform.localScale;
		scaler.x *= -1f;
		charectarModel.transform.localScale = scaler;
	}
	void OnDrawGizmosSelected(){
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(groundCheck.position, checkRadius);

	}
}
