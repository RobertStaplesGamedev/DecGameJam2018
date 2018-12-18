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
	private bool resetVictoryText = false;
	private int level = 1;

	public Animator animator;
	public GameObject charectarModel;
	public TextMeshProUGUI CollectedObjectsNum;
	public TextMeshProUGUI CollectedObjectsNumTotal;
	public PlantObject seed;
	public TextMeshProUGUI VictoryText;
	public Camera PlayerCamera;

	void Start() {
		rb = GetComponent<Rigidbody2D>();
	}

	void OnTriggerStay2D(Collider2D other) {
		if (other.gameObject.GetComponent<CollectObject>() != null) {
			collidedObject = other.gameObject;
		} else if (other.gameObject.GetComponent<PlantObject>() != null) {
			collidedObject = other.gameObject;
		}
	}
	void OnTriggerExit2D(Collider2D other) {
		if (other.gameObject.GetComponent<CollectObject>() != null) {
			collidedObject = null;
		} else if (other.gameObject.GetComponent<PlantObject>() != null) {
			collidedObject = null;
		}
	}

	void Update()
	{
		//Code That is always relevent
		if (isGround) {
			jumps = extraJumps;
		}
		if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)) && jumps > 0 ) {
			rb.velocity = Vector2.up * jumpForce;
			jumps--;
			if (resetVictoryText) {
				ResetVictory();
			}
		} else if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)|| Input.GetKeyDown(KeyCode.Space)) && jumps == 0 && isGround) {
			rb.velocity = Vector2.up * jumpForce;
			if (resetVictoryText) {
				ResetVictory();
			}
		}

		if (Input.GetKeyDown(KeyCode.E) && collidedObject != null && collidedObject.GetComponent<CollectObject>() != null) {
			Destroy(collidedObject.gameObject);
			collectedObjects++;
			CollectedObjectsNum.text = collectedObjects.ToString();
		}
		//Level Specific Code
		if (level == 1) {
			PlayerCamera.orthographicSize = 0.75f;
			 if (Input.GetKeyDown(KeyCode.E) && collidedObject != null && collidedObject.GetComponent<PlantObject>() != null) {
				if (collectedObjects == 6) {
					seed.SetShelterSprite(level);
					seed.SetShelterActive(true);
					VictoryText.gameObject.SetActive(true);
					extraJumps = 1;
					collectedObjects = 0;
					CollectedObjectsNum.text = collectedObjects.ToString();
					level++;
					resetVictoryText = true;
				}
			}
		}
		if (level==2) {
			CollectedObjectsNumTotal.text = "/10";
			if (collectedObjects == 1) {
				seed.SetShelterActive(false);
				seed.SetShelterSprite(level);
			}
			PlayerCamera.orthographicSize = 1f;
			if (Input.GetKeyDown(KeyCode.E) && collidedObject != null && collidedObject.GetComponent<PlantObject>() != null) {
				if (collectedObjects == 10) {
					seed.SetShelterSprite(level);
					seed.SetShelterActive(true);
					VictoryText.gameObject.SetActive(true);
					VictoryText.text = "You have now unlocked the pickaxe";
					collectedObjects = 0;
				}
			}
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
	void ResetVictory() {
		VictoryText.gameObject.SetActive(false);
		resetVictoryText = false;
	}
}
