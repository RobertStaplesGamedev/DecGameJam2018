using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour {

[Header("Charectar")]
	public GameObject charectar;

[Header("GameObjects")]
    GameObject collidedObject;
    public PlantObject seed;
	private int collectedObjects;
	private bool resetVictoryText = false;
	private int level = 1;

[Header("UI")]
	public TextMeshProUGUI CollectedObjectsNum;
	public TextMeshProUGUI CollectedObjectsNumTotal;
	public TextMeshProUGUI VictoryText;
	public Camera PlayerCamera;

    void Update() {
        if (charectar.GetComponent<CharectarMovement>().collidedObject != null) {
            collidedObject = charectar.GetComponent<CharectarMovement>().collidedObject;
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
					charectar.GetComponent<CharectarMovement>().extraJumps = 1;
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
    void ResetVictory() {
		VictoryText.gameObject.SetActive(false);
		resetVictoryText = false;
	}

}
