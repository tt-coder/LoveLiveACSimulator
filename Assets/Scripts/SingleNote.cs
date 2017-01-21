﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleNote : MonoBehaviour {

	private int lane;
	public int laneIndex;
	private bool isKeyDown = false;
	private float posX,posY,distance,distanceEval;

	// Use this for initialization
	void Start () {
		lane = GetComponent<NoteMove>().laneValue;
	}
	
	// Update is called once per frame
	void Update () {
		setLayer();
		detectionKeyInput();
	}

	private void setLayer(){
		if(lane != 0 && lane != 8){
			transform.SetSiblingIndex((int)transform.localPosition.y);
		}else{
			transform.SetSiblingIndex((int)transform.localPosition.x);
		}
	}

	private void detectionKeyInput(){
		if(Input.GetKeyDown("a") && lane == 0 &&  NoteCreator.nextNoteValue[lane] == laneIndex && isKeyDown == false){
			distance = Mathf.Sqrt(Mathf.Pow((0 - transform.localPosition.x),2) + Mathf.Pow((3.6f - transform.localPosition.y),2));
			distanceEval = distance/7.65f;
			if(distanceEval < 0) distanceEval = 0;
			Debug.Log(distanceEval);
			isKeyDown = true;
		}else{
			if(isKeyDown == true){
			isKeyDown = false;
			Destroy(gameObject);
			NoteCreator.nextNoteValue[lane]++;
		}
		}
	}
}
