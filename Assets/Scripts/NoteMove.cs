using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteMove : MonoBehaviour {

	private float gameTime;
	private float posX,posY;
	public int laneValue;
	public float idealTime;
	private float[] posXList = new float[9] {-9f,-8.31492f,-6.364f,-3.444f,0f,3.444f,6.364f,8.31492f,9f}; // ノーツが動く最大距離
	private float[] posYList = new float[9] {3.6f,0.15585f,-2.764f,-4.715f,-5.4f,-4.715f,-2.764f,0.15585f,3.6f};
	private float[] idealPosXList = new float[9] {-7.65f,-7.0677f,-5.4094f,-2.928f,0f,2.928f,5.4094f,7.0677f,7.65f}; // ノーツの理想的な位置
	private float[] idealPosYList = new float[9] {3.6f,0.672f,-1.809f,-3.468f,-4.05f,-3.468f,-1.809f,0.672f,3.6f};

	void Start () {
		gameTime = 0;
	}
	
	void Update () {
		moveNote();
	}

	private void moveNote(){
		//iTween.MoveTo(gameObject,iTween.Hash("position",new Vector3(idealPosXList[laneValue],idealPosYList[laneValue],0f),"easeType","linear","time",(1.89f/2.0f),"oncomplete","destroyNote"));
		iTween.MoveTo(gameObject,iTween.Hash("position",new Vector3(posXList[laneValue],posYList[laneValue],0f),"easeType","linear","time",(2.22353f/2.0f),"oncomplete","destroyNote")); // 2.29176
		iTween.ScaleTo(gameObject,iTween.Hash("x",0.6f,"y",0.6f,"time",1.0f,"easeType","easeOutSine"));
	}

	private void destroyNote(){
		Destroy(gameObject);
		//Debug.Log(Mathf.Abs((NoteCreator2.gameTime - 1.0f) - idealTime));
		NoteCreator2.nextNoteValue[laneValue]++;
		StatusManager.noteCount[0]++;
	}
}
