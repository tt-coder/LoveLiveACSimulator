using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongNote : MonoBehaviour {
	// Common
	private int lane;
	public int laneIndexStart,laneIndexEnd;
	private float posX,posY,distance,distanceEval;
	private string[] key = new string[9] {"a","s","d","f","space","h","j","k","l"};
	// 判定
	private float timeLag;
	public float perfectArea = 0.04f;
	public float greatArea = 0.0166f;
	public float goodArea = 0.01f;
	public float badArea = 0.01f;
	private float nowTime;
	// 生成関係
	public GameObject longNoteEnd;
	private GameObject longStartObj, longEndObj, longLineObj;
	public float startTime, endTime;
	private bool isCreateEnd = false;
	private bool isStartDestroy = false;
	// ライン
	public Material lineMaterial1;
	private LineRenderer longLine;
	private Vector3 lineStartPos, lineEndPos, lineStartOffset = new Vector3(-0.13f,0,0), lineEndOffset;
	private float[] lineOffsetX = new float[9] {-0.13f,-0.13f,-0.095f,-0.05f,0,0.05f,0.095f,0.13f,0.13f};
	private float[] lineOffsetY = new float[9] {0,-0.06f,-0.095f,-0.13f,-0.14f,-0.13f,-0.095f,-0.06f,0};
	// エフェクト
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
		longStartObj = transform.FindChild("LongNoteStart").gameObject;
		createLine();
		iTween.ScaleTo(longStartObj,iTween.Hash("x",1.0f,"y",1.0f,"time",1.0f,"easeType","easeOutSine"));
		lineStartPos = gameObject.transform.localPosition;
		lineEndPos = new Vector3(0f,3.6f,0f);
		updateLineWidth();
	}
	
	void Update () {
		autoDelete();
		//detectionKeyInput();
		createLongEnd();
	}

	private void createLongEnd(){ // 終点の生成と、始点終点間の線の描画
		if(NoteCreator.gameTime >= endTime){ // もし「終点があるべき時間」となったら
			if(isCreateEnd == false){ // 終点をまだ生成してなかったら
				longEndObj = Instantiate(longNoteEnd, new Vector3(0, 3.6f, 0), Quaternion.identity) as GameObject; // 終点を生成
				longEndObj.transform.parent = gameObject.transform; // 親をNoteLongとする
				longEndObj.GetComponent<NoteMove>().laneValue = lane;
				isCreateEnd = true; // 終点生成フラグON
				updateLineWidth1();
				iTween.ScaleTo(longEndObj,iTween.Hash("x",1.0f,"y",1.0f,"time",1.0f,"easeType","easeOutSine"));
			}
			updateLinePosition(lineStartPos , longEndObj.transform.position ); // 線の描画(始点の座標と終点の座標間に描画)
		}else{ // まだ終点が来てなかったら
			updateLinePosition(lineStartPos, lineEndPos);
		}
	}

	private void createLine(){ // 線の生成と初期化
		longLineObj = new GameObject(); // 線の生成
		longLineObj.transform.parent = gameObject.transform; // 親をNoteLongとする
		longLine = new LineRenderer();
		longLine = longLineObj.AddComponent<LineRenderer>(); // LineRendererをAdd 
		longLine.material = lineMaterial1;
		longLine.startWidth = 0f;
		longLine.endWidth = 0f;
		updateLineWidth();
	}

	private void updateLinePosition(Vector3 start, Vector3 end){ // 線の描画(引数：始点の座標、終点の座標)
		longLine.SetPosition(0, start);
		longLine.SetPosition(1, end);
	}

	private void updateLineWidth(){ // 線の幅を時間ごとに変化させる
		iTween.ValueTo(longLineObj, iTween.Hash("from",0f, "to",1.2f, "time", (1.5f/2.0f),"onUpdate", "updateWidth","onupdatetarget", gameObject));
	}

	private void updateWidth(float width){
		longLine.startWidth = width;
	}

	private void updateLineWidth1(){ // 線の幅を時間ごとに変化させる
		float time = endTime - startTime + 0.5f;
		iTween.ValueTo(longLineObj, iTween.Hash("from",0f, "to",1.2f, "time", 0.6f,"onUpdate", "updateWidth1","onupdatetarget", gameObject));
	}

	private void updateWidth1(float width){
		longLine.endWidth = width;
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
			if(Input.GetKey(key[i]) && lane == i){
				return true;
			}
		}
		return false;
	}

	private void detectionKeyInput(){
		nowTime = NoteCreator.gameTime - 1.0f;
		if(keyCheck() && NoteCreator.nextNoteValue[lane] == laneIndexStart && isNoteDistance()){ // 始点を押した時の判定
			if(isStartDestroy == false){ // まだ始点が押されていなかったら
				isStartDestroy = true;
				Destroy(longStartObj);
				lineStartPos = new Vector3(effectRingPosX[lane],effectRingPosY[lane],0f);
				NoteCreator.nextNoteValue[lane]++;
				StatusManager.noteCount[0]++;
				timeLag = Mathf.Abs(nowTime - startTime);
				judgeTimeLag(timeLag);
			}
		}else{
			if(isStartDestroy == false){ // 始点が押されていないときのラインスタート位置
				lineStartPos = gameObject.transform.localPosition + new Vector3(lineOffsetX[lane],lineOffsetY[lane],0f);
			}
			if(keyCheck() == false && nowTime - (10f/60f) >= startTime && isStartDestroy == false){ // 通り過ぎた時のミス処理
				Destroy(gameObject);
				NoteCreator.nextNoteValue[lane]++;
				StatusManager.noteCount[0]++;
				StatusManager.noteCount[5]++;
				StatusManager.combo = 0;
			}
			if(!keyCheck() && isStartDestroy && isNoteDistance() && NoteCreator.nextNoteValue[lane] == laneIndexEnd){ // 終点で離すときの判定
				Destroy(gameObject);
				NoteCreator.nextNoteValue[lane]++;
				StatusManager.noteCount[0]++;
				timeLag = Mathf.Abs(nowTime - endTime);
				judgeTimeLag(timeLag);
			}
		}
	}

	private void autoDelete(){
		nowTime = NoteCreator.gameTime - 1.0f;
		if(nowTime >= startTime){ // 始点のある時刻となったら
			if(isStartDestroy == false){
				isStartDestroy = true; // 始点を押す
				Destroy(longStartObj);
				lineStartPos = new Vector3(effectRingPosX[lane],effectRingPosY[lane],0f);
				NoteCreator.nextNoteValue[lane]++;
				StatusManager.noteCount[0]++;
				StatusManager.noteCount[1]++;
				StatusManager.combo++;
			}
		}else{
			lineStartPos = gameObject.transform.localPosition + new Vector3(lineOffsetX[lane],lineOffsetY[lane],0f);
		}

		if(nowTime >= endTime){ // 終点のある時刻となったら終点を離す
			NoteCreator.nextNoteValue[lane]++;
			StatusManager.noteCount[0]++;
			StatusManager.noteCount[1]++;
			StatusManager.combo++;
			Destroy(gameObject);
		}
	}

	private void judgeTimeLag(float lag){
		if(lag <= perfectArea){
			StatusManager.noteCount[1]++;
			StatusManager.combo++;
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

	private void displayJudgeEffect(){
		judgeEffectObj = Instantiate(judgeEffect, new Vector3(effectRingPosX[2], effectRingPosY[2], 0), Quaternion.identity) as GameObject;
		iTween.ScaleTo(judgeEffectObj, iTween.Hash("x",0.4f,"y",0.4f,"time",0.1f,"onComplete","deleteEffect"));
	}
}
