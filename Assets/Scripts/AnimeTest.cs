using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimeTest : MonoBehaviour {

	public GameObject effect;
	private GameObject newObj;
	void Start () {
		
	}

	void Update () {
		if(Input.GetKeyDown("space")){
			newObj = Instantiate(effect);
		}
	}

	private void deleteAnime(GameObject newObj){
		Destroy(newObj);
	}
}
