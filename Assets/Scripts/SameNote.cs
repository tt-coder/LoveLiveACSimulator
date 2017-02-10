using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SameNote : MonoBehaviour {
	// Common
	private int lane;
	public int laneIndex;
	private bool isKeyDown = false;
	private float posX,posY,distance,distanceEval;
	private string[] key = new string[9] {"a","s","d","f","space","h","j","k","l"};
	public bool noteImage;
	public Sprite noteImageBlue;
	public Sprite noteImageOrange;
	// 判定
	public float idealTime;
	private float timeLag;
	private float nowTime;
	public float perfectArea = 0.04f;
	public float greatArea = 0.0166f;
	public float goodArea = 0.01f;
	public float badArea = 0.01f;
	// エフェクト
	private float[] effectRingPosX = new float[9] {-7.65f,-7.0677f,-5.4094f,-2.928f,0f,2.928f,5.4094f,7.0677f,7.65f};
	private float[] effectRingPosY = new float[9] {3.6f,0.672f,-1.809f,-3.468f,-4.05f,-3.468f,-1.809f,0.672f,3.6f};
	public GameObject[] judgeEffect = new GameObject[4];
	private GameObject judgeEffectObj;

	void Start () {
		perfectArea = 0.04f;
		greatArea = 0.01f;
		goodArea = 0.0166f;
		badArea = 0.0166f;
		lane = GetComponent<NoteMove>().laneValue;
		iTween.ScaleTo(gameObject,iTween.Hash("x",0.6f,"y",0.6f,"time",1.0f,"easeType","easeOutSine"));
		if(!noteImage){
			gameObject.GetComponent<SpriteRenderer>().sprite = noteImageBlue;
		}else{
			gameObject.GetComponent<SpriteRenderer>().sprite = noteImageOrange;
		}
	}
	
	void Update () {
		setLayer();
		//autoDelete();
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

	private bool keyCheck(){
		for(int i=0;i<9;i++){
			if(Input.GetKeyDown(key[i]) && lane == i){
				return true;
			}
		}
		return false;
	}

	private void detectionKeyInput(){
		nowTime = NoteCreator.gameTime - 1.0f;
		if(keyCheck() &&  NoteCreator.nextNoteValue[lane] == laneIndex && isKeyDown == false && isNoteDistance()){
			timeLag = Mathf.Abs(nowTime - idealTime);
			judgeTimeLag(timeLag);
			isKeyDown = true;
		}else{
			if(isKeyDown == true){
				isKeyDown = false;
				Destroy(gameObject);
				NoteCreator.nextNoteValue[lane]++;
				StatusManager.noteCount[0]++;
			}
			if(nowTime - (10f/60f) >= idealTime){
				Destroy(gameObject);
				NoteCreator.nextNoteValue[lane]++;
				StatusManager.noteCount[0]++;
				StatusManager.noteCount[5]++;
				StatusManager.combo = 0;
			}
		}

	}

	private void autoDelete(){
		nowTime = NoteCreator.gameTime - 1.0f;
		if(nowTime >= idealTime){
			Destroy(gameObject);
			NoteCreator.nextNoteValue[lane]++;
			StatusManager.noteCount[0]++;
			StatusManager.noteCount[1]++;
			StatusManager.combo++;
		}
	}

	private void judgeTimeLag(float lag){
		if(lag <= perfectArea){
			StatusManager.noteCount[1]++;
			StatusManager.combo++;
			generateEffect(1);
		}else if(lag <= perfectArea + greatArea){
			StatusManager.noteCount[2]++;
			StatusManager.combo++;
		}else if(lag <= perfectArea + greatArea + goodArea){
			StatusManager.noteCount[3]++;
			StatusManager.combo = 0;
		}else if(lag <= perfectArea + greatArea + goodArea + badArea){
			StatusManager.noteCount[4]++;
			StatusManager.combo = 0;
		}else{
			Destroy(gameObject);
			StatusManager.noteCount[5]++;
			StatusManager.combo = 0;
		}
	}

	private void generateEffect(int judge){
		switch(judge){
			default:
				judgeEffectObj = Instantiate(judgeEffect[0], new Vector3(effectRingPosX[lane], effectRingPosY[lane] , 0), Quaternion.identity) as GameObject;
				break;
		}
	}

}
