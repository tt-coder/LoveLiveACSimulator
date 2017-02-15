using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongNote : MonoBehaviour {
	// Common
	public int laneValue;
	private int lane;
	public int laneIndexStart,laneIndexEnd;
	private float posX,posY,distance,distanceEval;
	private float transTimeStart,transTimeEnd;
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
	public Material lineMaterial1,lineMaterial2;
	private LineRenderer longLine;
	private Vector3 lineStartPos, lineEndPos, lineStartOffset = new Vector3(-0.13f,0,0), lineEndOffset;
	private float[] lineOffsetX = new float[9] {-0.13f,-0.13f,-0.095f,-0.05f,0,0.05f,0.095f,0.13f,0.13f};
	private float[] lineOffsetY = new float[9] {0,-0.06f,-0.095f,-0.13f,-0.14f,-0.13f,-0.095f,-0.06f,0};
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
		longStartObj = transform.FindChild("LongNoteStart").gameObject;
		createLine();
		transTimeStart = startTime - NoteCreator.gameTime + 1.0f;
		iTween.ScaleTo(longStartObj,iTween.Hash("x",1.0f,"y",1.0f,"time",transTimeStart,"easeType","easeOutSine"));
		lineStartPos = gameObject.transform.localPosition;
		lineEndPos = new Vector3(0,3.6f,0);
		updateLineWidth();
	}
	
	void Update () {
		autoDelete();
		detectionKeyInput();
		createLongEnd();
	}

	private void createLongEnd(){ // 終点の生成と、始点終点間の線の描画
		if(NoteCreator.gameTime >= endTime){ // もし「終点があるべき時間」となったら
			if(isCreateEnd == false){ // 終点をまだ生成してなかったら
				longEndObj = Instantiate(longNoteEnd, new Vector3(0, 3.6f, 0), Quaternion.identity) as GameObject; // 終点を生成
				longEndObj.transform.parent = gameObject.transform; // 親をNoteLongとする
				longEndObj.GetComponent<NoteMove>().laneValue = lane;
				longEndObj.GetComponent<NoteMove>().idealTime = endTime;
				longEndObj.GetComponent<SpriteRenderer>().sortingLayerName = "Notes";
				transTimeEnd = endTime - NoteCreator.gameTime + 1.0f;
				isCreateEnd = true; // 終点生成フラグON
				updateLineWidth1();
				iTween.ScaleTo(longEndObj,iTween.Hash("x",0.8f,"y",0.8f,"time",transTimeEnd,"easeType","easeOutSine"));
			}
			float posX = longEndObj.transform.position.x;
			float posY = longEndObj.transform.position.y;
			updateLinePosition(lineStartPos , new Vector3(posX, posY, 0f) ); // 線の描画(始点の座標と終点の座標間に描画)
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
		iTween.ValueTo(longLineObj, iTween.Hash("from",0f, "to",1.2f, "time", transTimeStart,"onUpdate", "updateWidth","onupdatetarget", gameObject,"easeType","easeOutSine"));
	}

	private void updateWidth(float width){
		longLine.startWidth = width;
	}

	private void updateLineWidth1(){ // 線の幅を時間ごとに変化させる
		iTween.ValueTo(longLineObj, iTween.Hash("from",0f, "to",0.9f, "time", transTimeEnd,"onUpdate", "updateWidth1","onupdatetarget", gameObject,"easeType","easeOutCubic"));
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
			if(Input.GetKeyDown(key[i]) && lane == i){
				return true;
			}
		}
		return false;
	}

	private bool keyDownCheck(){
		for(int i=0;i<9;i++){
			if(Input.GetKeyDown(key[i]) && lane == i){
				return true;
			}
		}
		return false;
	}

	private bool keyUpCheck(){
		for(int i=0;i<9;i++){
			if(Input.GetKeyUp(key[i]) && lane == i){
				return true;
			}
		}
		return false;
	}

	private void detectionKeyInput(){
		nowTime = NoteCreator.gameTime - 1.0f;
		if(keyDownCheck() && NoteCreator.nextNoteValue[lane] == laneIndexStart && isNoteDistance()){
			if(isStartDestroy == false){
				isStartDestroy = true;
				Destroy(longStartObj);
				longLine.material = lineMaterial2;
				lineStartPos = new Vector3(effectRingPosX[lane],effectRingPosY[lane],0f);
				NoteCreator.nextNoteValue[lane]++;
				StatusManager.noteCount[0]++;
				timeLag = Mathf.Abs(nowTime - startTime);
				judgeTimeLag(timeLag, "start");
			}
		}else{
			if(isStartDestroy){
				if(keyUpCheck() && NoteCreator.nextNoteValue[lane] == laneIndexEnd && isNoteDistance()){
					Destroy(gameObject);
					NoteCreator.nextNoteValue[lane]++;
					StatusManager.noteCount[0]++;
					timeLag = Mathf.Abs(nowTime - endTime);
					judgeTimeLag(timeLag,"end");
				}
			}else{
				float posX = gameObject.transform.localPosition.x;
				float posY = gameObject.transform.localPosition.y;
				lineStartPos = new Vector3(posX,posY,0f) + new Vector3(lineOffsetX[lane],lineOffsetY[lane],0f);
			}
			if(nowTime - (10f/60f) >= startTime && isStartDestroy == false){
				Destroy(gameObject);
				NoteCreator.nextNoteValue[lane]+=2; // 始点と終点の分
				StatusManager.noteCount[0]++;
				StatusManager.noteCount[5]++;
				StatusManager.combo = 0;
			}
		}
		/*
		if(keyCheck() && NoteCreator.nextNoteValue[lane] == laneIndexStart && isNoteDistance()){ // 始点を押した時の判定
			if(isStartDestroy == false){ // まだ始点が押されていなかったら
				isStartDestroy = true;
				Destroy(longStartObj);
				lineStartPos = new Vector3(effectRingPosX[lane],effectRingPosY[lane],0f);
				NoteCreator.nextNoteValue[lane]++;
				StatusManager.noteCount[0]++;
				timeLag = Mathf.Abs(nowTime - startTime);
				judgeTimeLag(timeLag, "start");
			}
		}else{
			if(isStartDestroy == false){ // 始点が押されていないときのラインスタート位置
				lineStartPos = gameObject.transform.localPosition + new Vector3(lineOffsetX[lane],lineOffsetY[lane],0f);
			}
			if(!keyCheck() && nowTime - (10f/60f) >= startTime && isStartDestroy == false){ // 通り過ぎた時のミス処理
				Destroy(gameObject);
				NoteCreator.nextNoteValue[lane]+=2; // 始点と終点の分
				StatusManager.noteCount[0]++;
				StatusManager.noteCount[5]++;
				StatusManager.combo = 0;
			}
			if(!keyCheck() && isStartDestroy && isNoteDistance() && NoteCreator.nextNoteValue[lane] == laneIndexEnd){ // 終点で離すときの判定
				Destroy(gameObject);
				NoteCreator.nextNoteValue[lane]++;
				StatusManager.noteCount[0]++;
				timeLag = Mathf.Abs(nowTime - endTime);
				judgeTimeLag(timeLag,"end");
			}
		}
		*/
	}

	private void autoDelete(){
		nowTime = NoteCreator.gameTime - 1.0f;
		if(nowTime >= startTime){ // 始点のある時刻となったら
			if(isStartDestroy == false){
				isStartDestroy = true; // 始点を押す
				Destroy(longStartObj);
				longLine.material = lineMaterial2;
				lineStartPos = new Vector3(effectRingPosX[lane],effectRingPosY[lane],0f);
				NoteCreator.nextNoteValue[lane]++;
				StatusManager.noteCount[0]++;
				StatusManager.noteCount[1]++;
				StatusManager.combo++;
				generateEffect(1);
			}
		}else{
			float posX = gameObject.transform.localPosition.x;
			float posY = gameObject.transform.localPosition.y;
			lineStartPos = new Vector3(posX,posY,0f) + new Vector3(lineOffsetX[lane],lineOffsetY[lane],0f);
		}

		if(nowTime >= endTime){ // 終点のある時刻となったら終点を離す
			NoteCreator.nextNoteValue[lane]++;
			StatusManager.noteCount[0]++;
			StatusManager.noteCount[1]++;
			StatusManager.combo++;
			Destroy(gameObject);
			generateEffect(1);
		}
	}

	private void judgeTimeLag(float lag, string s){
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
			if(s == "start"){
				NoteCreator.nextNoteValue[lane]++;
			}
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
