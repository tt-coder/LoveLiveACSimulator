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
	public float greatArea = 0.0166f;
	public float goodArea = 0.01f;
	public float badArea = 0.01f;
	private float nowTime;
	private float[] effectRingPosX = new float[9] {-7.65f,-7.0677f,-5.4094f,-2.928f,0f,2.928f,5.4094f,7.0677f,7.65f};
	private float[] effectRingPosY = new float[9] {3.6f,0.672f,-1.809f,-3.468f,-4.05f,-3.468f,-1.809f,0.672f,3.6f};
	public GameObject judgeEffect;
	private GameObject judgeEffectObj;

	void Start () {
		perfectArea = 0.04f;
		greatArea = 0.01f;
		goodArea = 0.0166f;
		badArea = 0.0166f;
		lane = GetComponent<NoteMove>().laneValue;
		iTween.ScaleTo(gameObject,iTween.Hash("x",0.6f,"y",0.6f,"time",1.0f,"easeType","easeOutSine"));
	}
	
	void Update () {
		setLayer();
		detectionKeyInput();
		//test();
	}

	private void test(){
		if(Input.GetKey("f")){
			Debug.Log("OK");
		}
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

	private bool keyCheck(){
		if(Input.GetKeyDown("a") && lane == 0){
			return true;
		}
		if(Input.GetKeyDown("s") && lane == 1){
			return true;
		}
		if(Input.GetKeyDown("d") && lane == 2){
			return true;
		}
		if(Input.GetKeyDown("f") && lane == 3){
			return true;
		}
		if(Input.GetKeyDown("space") && lane == 4){
			return true;
		}
		if(Input.GetKeyDown("h") && lane == 5){
			return true;
		}
		if(Input.GetKeyDown("j") && lane == 6){
			return true;
		}
		if(Input.GetKeyDown("k") && lane == 7){
			return true;
		}
		if(Input.GetKeyDown("l") && lane == 8){
			return true;
		}
		return false;
	}

	private void detectionKeyInput(){
		
		nowTime = NoteCreator.gameTime - 1.0f;
		if(nowTime >= idealTime){
			Debug.Log(Mathf.Abs(nowTime - idealTime));
			Destroy(gameObject);
			NoteCreator.nextNoteValue[lane]++;
			StatusManager.noteCount[0]++;
			StatusManager.noteCount[1]++;
			StatusManager.combo++;
		}

		if(keyCheck() &&  NoteCreator.nextNoteValue[lane] == laneIndex && isKeyDown == false && isNoteDistance()){
			nowTime = NoteCreator.gameTime - 1.0f;
			timeLag = Mathf.Abs(nowTime - idealTime);
			if(timeLag <= perfectArea){
				Debug.Log("PERFECT");
				StatusManager.noteCount[1]++;
				StatusManager.combo++;
			}else if(timeLag <= perfectArea + greatArea){
				Debug.Log("GREAT");
				StatusManager.noteCount[2]++;
				StatusManager.combo++;
			}else if(timeLag <= perfectArea + greatArea + goodArea){
				Debug.Log("GOOD");
				StatusManager.noteCount[3]++;
				StatusManager.combo = 0;
			}else if(timeLag <= perfectArea + greatArea + goodArea + badArea){
				Debug.Log("BAD");
				StatusManager.noteCount[4]++;
				StatusManager.combo = 0;
			}else{
				Debug.Log("MISS");
				StatusManager.noteCount[5]++;
				StatusManager.combo = 0;
			}
			isKeyDown = true;
		}else{
			if(isKeyDown == true){
				isKeyDown = false;
				Destroy(gameObject);
				NoteCreator.nextNoteValue[lane]++;
				StatusManager.noteCount[0]++;
			}
		}

	}

	private void displayJudgeEffect(){
		judgeEffectObj = Instantiate(judgeEffect, new Vector3(effectRingPosX[2], effectRingPosY[2], 0), Quaternion.identity) as GameObject;
		iTween.ScaleTo(judgeEffectObj, iTween.Hash("x",0.4f,"y",0.4f,"time",0.1f,"onComplete","deleteEffect"));
	}
}
