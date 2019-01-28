using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Baroon : MonoBehaviour {

    Vector2[] patrolBox;
    public Enemy enemyScript;
    Rigidbody2D rb;
    [Range(-1,1)]
    public float xStartDirection;
    [Range(-1,1)]
    public float yStartDirection;

    [Header("Movement")]
    Vector2 direction;

    [Header("Patrol")]
    public bool usePatrol = true;
    public Transform detector;
    public float detectionDistanceFront = 2f;
    public float detectionDistanceUp = 2f;
	public float detectionDistanceDown = 2f;

    void Start() {
        rb = this.GetComponent<Rigidbody2D>();
        direction = new Vector2(xStartDirection * enemyScript.speed,yStartDirection * enemyScript.speed);
        rb.velocity = direction;
        DrawPatrolBox();
    }

    void FixedUpdate() {
        Move();
    }

    /*
        Move Scripts
    */

    void Move() {
        //Check for Collision with wall
        RaycastHit2D upHitInfo = Physics2D.Raycast(detector.position, Vector2.up, detectionDistanceUp);
        RaycastHit2D frontHitInfo = Physics2D.Raycast(detector.position, direction, detectionDistanceFront);
        
        RaycastHit2D downHitInfo = Physics2D.Raycast(detector.position, Vector2.down, detectionDistanceDown);
        //Debug.Log(rb.velocity);
        //Check if front hit detected anything
        if (frontHitInfo.transform != null && frontHitInfo.transform != this.transform && frontHitInfo.transform.gameObject.layer != 11 && frontHitInfo.transform.gameObject.layer != 10) {
            direction = new Vector2(-direction.x, direction.y);
            Flip();
        } else if ((downHitInfo.transform != null && downHitInfo.transform.gameObject.layer != 11) || (upHitInfo.transform != null&& upHitInfo.transform.gameObject.layer != 11)) {
            direction = new Vector2(direction.x, -direction.y);
        //Check for Edge of Patrol Box
        } 
        else if (usePatrol) {
            Vector2 hitPatrolBox = DetectPatrolBox();
            if (hitPatrolBox == patrolBox[1] || hitPatrolBox == patrolBox[3] && (hitPatrolBox.x < this.transform.position.x || hitPatrolBox.x > this.transform.position.x)) {
                direction = new Vector2(-direction.x, direction.y);
                Flip();
            } else if (hitPatrolBox == patrolBox[2] || hitPatrolBox == patrolBox[0] && (hitPatrolBox.y < this.transform.position.y || hitPatrolBox.y > this.transform.position.y)) {
                direction = new Vector2(direction.x, -direction.y);
            }
        }
        //Keep moving on current trajectory
        rb.velocity = direction;
        //Debug.Log(rb.velocity);
    }

    void Flip() {
		Vector3 scaler = enemyScript.model.transform.localScale;
		scaler.x *= -1f;
		enemyScript.model.transform.localScale = scaler;

	}

    Vector2 DetectPatrolBox() {
        if (detector.position.x + detectionDistanceFront >= patrolBox[1].x) {
            return patrolBox[1];
        } else if (detector.position.x - detectionDistanceFront <= patrolBox[0].x) {
            return patrolBox[3];
        } else if (detector.position.y + detectionDistanceUp >= patrolBox[0].y) {
            return patrolBox[0];
        } else if (detector.position.y - detectionDistanceDown <= patrolBox[2].y) {
            return patrolBox[2];
        } else {
            return Vector2.zero;
        }
    }

    void DrawPatrolBox() {
        //TODO: Draw patrol box with handles rather than just vectors
        patrolBox = new Vector2[4] {
            new Vector2(this.transform.position.x - 2,this.transform.position.y + 2),
            new Vector2(this.transform.position.x + 2,this.transform.position.y + 2),
            new Vector2(this.transform.position.x + 2,this.transform.position.y - 2),
            new Vector2(this.transform.position.x - 2,this.transform.position.y - 2)
        };
    }

    void OnDrawGizmosSelected() {
        Vector2 directionDraw = new Vector2(detectionDistanceFront * direction.x,0);
        Gizmos.color = Color.green;
        if (!EditorApplication.isPlaying) {
            DrawPatrolBox();
            directionDraw = new Vector2(detectionDistanceFront * xStartDirection,0);
        } else {
            directionDraw = new Vector2(detectionDistanceFront * direction.x,0);
        }
        if (usePatrol) {
            Gizmos.DrawLine(patrolBox[0],patrolBox[1]);
            Gizmos.DrawLine(patrolBox[1],patrolBox[2]);
            Gizmos.DrawLine(patrolBox[2],patrolBox[3]);
            Gizmos.DrawLine(patrolBox[3],patrolBox[0]);
        }

        Gizmos.color = Color.red;
		Gizmos.DrawRay(detector.position, directionDraw);
		Gizmos.DrawRay(detector.position, (new Vector2(Vector2.down.x * detectionDistanceFront, Vector2.down.y * detectionDistanceDown)));
        Gizmos.DrawRay(detector.position, (new Vector2(Vector2.up.x * detectionDistanceFront, Vector2.up.y * detectionDistanceUp)));
    }
}