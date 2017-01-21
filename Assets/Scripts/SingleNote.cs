using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleNote : MonoBehaviour {

	private int lane;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		setLayer();
	}

	void setLayer(){
		lane = GetComponent<NoteMove>().laneValue;
		Debug.Log(lane);
		if(lane != 0 && lane != 8){
			transform.SetSiblingIndex((int)transform.localPosition.y);
		}else{
			transform.SetSiblingIndex((int)transform.localPosition.x);
		}
	}
}
