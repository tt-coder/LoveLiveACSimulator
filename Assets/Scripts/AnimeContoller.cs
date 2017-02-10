using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimeContoller : MonoBehaviour {

	private GameObject[] perfectEffect = new GameObject[3];

	void Start () {
		if(gameObject.name == "PerfectEffect(Clone)" || gameObject.name == "PerfectEffect"){
			perfectEffect[0] = gameObject.transform.FindChild("Ring").gameObject;
		}
	}
	
	void Update () {
		
	}

	void deleteAnime(){
		Destroy(gameObject);
	}

	void deletePerfectEffectRing(){
		Destroy(perfectEffect[0]);
	}

}
