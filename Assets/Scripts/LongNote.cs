using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongNote : MonoBehaviour {

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
		detectionKeyInput();
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
			//updateLinePosition(longStartObj.transform.position, newEndObj.transform.position); // 線の描画(始点の座標と終点の座標間に描画)
			/*
            if(isTouching == true){ // タッチされていたら
                if(Math.Abs(longEndObj.transform.position.x) > Math.Abs(effectPosX[BarNum])){ // 終点がタッチバーを越したら、
                    updateLinePosition(new Vector3(0, posY[BarNum], 0), new Vector3(0, posY[BarNum], 0)); // 中間部分を非常時 
                }else{ // 終点がタッチバーを越してなかったら
                    updateLinePosition(new Vector3(effectPosX[BarNum], posY[BarNum], 0), newEndObj.transform.position + new Vector3(iOSValue(BarNum),0,0)); // バーと終点間に描画
                }
            }else{ // タッチされていなかったら
                updateLinePosition(longStartObj.transform.position + new Vector3(iOSValue(BarNum),0,0) , newEndObj.transform.position + new Vector3(iOSValue(BarNum),0,0)); // 線の描画(始点の座標と終点の座標間に描画)
            }
			*/
		}else{ // まだ終点が来てなかったら
			updateLinePosition(lineStartPos, lineEndPos);
			/*
            if(isStartDestroy == true){ // タッチされていたら
              	updateLinePosition(new Vector3(effectRingPosX[lane], effectRingPosY[lane], 0), new Vector3(0, 3.6f, 0)); // バーと画面の真ん中間に描画
            }else{ // タッチされていなかったら
				//updateLinePosition(longStartObj.transform.position , new Vector3(0, 3.6f, 0)); // 線の描画(始点の座標と画面の真ん中間に描画)
				updateLinePosition(lineStartPos, new Vector3(0, 3.6f, 0));
            }
			*/
		}
	}

	private void createLine(){ // 線の生成と初期化
		longLineObj = new GameObject(); // 線の生成
		longLineObj.transform.parent = gameObject.transform; // 親をNoteLongとする
		longLine = new LineRenderer();
		longLine = longLineObj.AddComponent<LineRenderer>(); // LineRendererをAdd 
		longLine.material = lineMaterial1;
		//longLine.SetColors(Color.red,Color.red); // 色指定
		//longLine.SetWidth(1.5f,0f); // 幅指定(始点の幅、終点の幅)
		//longLine.SetVertexCount(2); // 頂点数指定
		longLine.startWidth = 0f;
		longLine.endWidth = 0f;
		updateLineWidth();
	}

	private void updateLinePosition(Vector3 start, Vector3 end){ // 線の描画(引数：始点の座標、終点の座標)
		longLine.SetPosition(0, start);
		longLine.SetPosition(1, end);
	}

	private void updateLineWidth(){ // 線の幅を時間ごとに変化させる
		//float time = endTime - startTime + 0.5f;
		iTween.ValueTo(longLineObj, iTween.Hash("from",0f, "to",1.2f, "time", startTime,"onUpdate", "updateWidth","onupdatetarget", gameObject));
	}

	private void updateWidth(float width){
		//longLine.SetWidth(1.5f,width);
		longLine.startWidth = width;
	}

	private void updateLineWidth1(){ // 線の幅を時間ごとに変化させる
		float time = endTime - startTime + 0.5f;
		iTween.ValueTo(longLineObj, iTween.Hash("from",0f, "to",1.2f, "time", 0.6f,"onUpdate", "updateWidth1","onupdatetarget", gameObject));
	}

	private void updateWidth1(float width){
		//longLine.SetWidth(1.5f,width);
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
		if(nowTime >= startTime){
			//Debug.Log(Mathf.Abs(nowTime - idealTime));
			if(isStartDestroy == false){
				isStartDestroy = true;
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


		if(nowTime >= endTime){
			NoteCreator.nextNoteValue[lane]++;
			StatusManager.noteCount[0]++;
			StatusManager.noteCount[1]++;
			StatusManager.combo++;
			Destroy(gameObject);
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
				//Destroy(gameObject);
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
