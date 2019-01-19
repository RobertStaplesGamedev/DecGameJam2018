using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Baroon : MonoBehaviour {

    Vector2[] patrolBox;
    public Enemy enemyScript;
    Rigidbody2D rb;
    [Range(-1,1)]
    public int xStartDirection;
    [Range(-1,1)]
    public int yStartDirection;

    [Header("Movement")]
    Vector2 direction;

    [Header("Patrol")]
    public Transform detector;
	public float detectionDistance = 2f;

    void Start() {
        DrawPatrolBox();
        rb = this.GetComponent<Rigidbody2D>();
        direction = new Vector2(xStartDirection * enemyScript.speed,yStartDirection * enemyScript.speed);
        rb.velocity = direction;
    }

    void FixedUpdate() {
        Move();
    }

    /*
        Move Scripts
    */

    void Move() {
        //Check for Collision with wall
        RaycastHit2D upHitInfo = Physics2D.Raycast(detector.position, Vector2.up, detectionDistance);
        RaycastHit2D frontHitInfo = Physics2D.Raycast(detector.position, direction, detectionDistance);
        RaycastHit2D downHitInfo = Physics2D.Raycast(detector.position, Vector2.down, detectionDistance);
        
        //Check if front hit detected anything
        if (frontHitInfo.transform != null) {
            Flip();
            direction = new Vector2(-direction.x, direction.y);
        } else if (downHitInfo.transform != null || upHitInfo.transform != null) {
            direction = new Vector2(direction.x, -direction.y);
        //Check for Edge of Patrol Box
        } else {
            Vector2 hitPatrolBox = DetectPatrolBox();
            if (hitPatrolBox != Vector2.zero && (hitPatrolBox.x < this.transform.position.x || hitPatrolBox.x > this.transform.position.x)) {
                Flip();
                direction = new Vector2(-direction.x, direction.y);
            } else {
                direction = new Vector2(direction.x, -direction.y);
            }
        }
        //Keep moving on current trajectory 
        rb.velocity = direction;
    }

    void Flip() {
		Vector3 scaler = enemyScript.model.transform.localScale;
		scaler.x *= -1f;
		enemyScript.model.transform.localScale = scaler;
	}

    Vector2 DetectPatrolBox() {
        if (detector.position.x + detectionDistance >= patrolBox[1].x) {
            return patrolBox[1];
        } else if (detector.position.x - detectionDistance <= patrolBox[0].x) {
            return patrolBox[0];
        } else if (detector.position.y + detectionDistance >= patrolBox[0].y) {
            return patrolBox[0];
        } else if (detector.position.y - detectionDistance <= patrolBox[2].y) {
            return patrolBox[2];
        } else {
            return Vector2.zero;
        }
    }

    void DrawPatrolBox() {
        //TODO: Draw patrol box with handles rather than just vectors
        patrolBox = new Vector2[4] {
            new Vector2(this.transform.position.x - 1,this.transform.position.y + 1),
            new Vector2(this.transform.position.x + 1,this.transform.position.y + 1),
            new Vector2(this.transform.position.x + 1,this.transform.position.y - 1),
            new Vector2(this.transform.position.x - 1,this.transform.position.y - 1)
        };
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.green;
		Gizmos.DrawLine(patrolBox[0],patrolBox[1]);
        Gizmos.DrawLine(patrolBox[1],patrolBox[2]);
        Gizmos.DrawLine(patrolBox[2],patrolBox[3]);
        Gizmos.DrawLine(patrolBox[3],patrolBox[0]);

        Gizmos.color = Color.red;
		Gizmos.DrawRay(detector.position, direction);
		Gizmos.DrawRay(detector.position, Vector2.down);
        Gizmos.DrawRay(detector.position, Vector2.up);
    }
}