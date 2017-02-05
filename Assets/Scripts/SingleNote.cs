﻿using System.Collections;
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
		
		nowTime = NoteCreator2.gameTime - 1.0f;
		if(nowTime >= idealTime){
			Debug.Log(Mathf.Abs(nowTime - idealTime));
			Destroy(gameObject);
			NoteCreator2.nextNoteValue[lane]++;
			StatusManager.noteCount[0]++;
			StatusManager.noteCount[1]++;
		}
		
		

		if(Input.GetKeyDown("a") && lane == 2 &&  NoteCreator2.nextNoteValue[lane] == laneIndex && isKeyDown == false && isNoteDistance()){
			nowTime = NoteCreator2.gameTime - 1.0f;
			timeLag = Mathf.Abs(nowTime - idealTime);
			displayJudgeEffect();
			if(timeLag <= perfectArea){
				Debug.Log("PERFECT");
				StatusManager.noteCount[1]++;
			}else if(timeLag <= perfectArea + greatArea){
				Debug.Log("GREAT");
				StatusManager.noteCount[2]++;
			}else if(timeLag <= perfectArea + greatArea + goodArea){
				Debug.Log("GOOD");
				StatusManager.noteCount[3]++;
			}else if(timeLag <= perfectArea + greatArea + goodArea + badArea){
				Debug.Log("BAD");
				StatusManager.noteCount[4]++;
			}else{
				Debug.Log("MISS");
				StatusManager.noteCount[5]++;
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

	private void displayJudgeEffect(){
		judgeEffectObj = Instantiate(judgeEffect, new Vector3(effectRingPosX[2], effectRingPosY[2], 0), Quaternion.identity) as GameObject;
		iTween.ScaleTo(judgeEffectObj, iTween.Hash("x",0.4f,"y",0.4f,"time",0.1f,"onComplete","deleteEffect"));
	}
}
