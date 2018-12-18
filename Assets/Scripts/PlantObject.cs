using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantObject : MonoBehaviour {

	public SpriteRenderer shelter;
	public Sprite seed;
	public Sprite sprout;
	public Sprite fernShelter;
	public Sprite branchShelter;

	public void SetShelterActive(bool setActive) {
		shelter.gameObject.SetActive(setActive);
	}

	public void SetShelterSprite(int level){
		if (level== 1) {
			shelter.sprite = fernShelter;
			this.GetComponent<SpriteRenderer>().sprite = seed;
		} else {
			shelter.sprite = branchShelter;
			this.GetComponent<SpriteRenderer>().sprite = sprout;
		}
	}

}
