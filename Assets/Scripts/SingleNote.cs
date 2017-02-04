using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleNote : MonoBehaviour {

	private int lane;
	public int laneIndex;
	private bool isKeyDown = false;
	private float posX,posY,distance,distanceEval;
	public float idealTime;
	private float timeLag;
	public float perfectArea = 0.04f;
	public float greatArea = 0.01f;
	public float goodArea = 0.0166f;
	public float badArea = 0.0166f;
	private float nowTime;

	void Start () {
		perfectArea = 0.04f;
		greatArea = 0.01f;
		goodArea = 0.0166f;
		badArea = 0.0166f;
		lane = GetComponent<NoteMove>().laneValue;
	}
	
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

	private bool isNoteDistance(){
		float x = transform.localPosition.x;
		float y = transform.localPosition.y;
		float distance = Mathf.Sqrt(Mathf.Pow((0 - transform.localPosition.x),2) + Mathf.Pow((3.6f - transform.localPosition.y),2));
		if(distance > 3.825f){
			return true;
		}else{
			return false;
		}
	}

	private void detectionKeyInput(){
		float gr = 0.01f;
		if(Input.GetKeyDown("a") && lane == 0 &&  NoteCreator2.nextNoteValue[lane] == laneIndex && isKeyDown == false && isNoteDistance()){
			nowTime = NoteCreator2.gameTime - 2.0f;
			timeLag = Mathf.Abs(nowTime - idealTime);
			if(timeLag <= perfectArea){
				Debug.Log("PERFECT");
				StatusManager.noteCount[1]++;
			}else if(timeLag <= perfectArea + greatArea){
				Debug.Log("GREAT");
			}else if(timeLag <= perfectArea + greatArea + goodArea){
				Debug.Log("GOOD");
			}else if(timeLag <= perfectArea + greatArea + goodArea + badArea){
				Debug.Log("BAD");
			}else{
				Debug.Log("MISS");
			}
			isKeyDown = true;
		}else{
			if(isKeyDown == true){
				isKeyDown = false;
				Destroy(gameObject);
				NoteCreator2.nextNoteValue[lane]++;
				StatusManager.noteCount[0]++;
			}
		}
	}
}
