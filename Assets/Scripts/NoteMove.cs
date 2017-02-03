using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteMove : MonoBehaviour {

	private float gameTime;
	private float posX,posY,speed = 0.00000005f;
	public int laneValue;
	private float rad;

	private float[] posXList = new float[9] {-9f,-8.31492f,-6.364f,-3.444f,0f,3.444f,6.364f,8.31492f,9f};
	private float[] posYList = new float[9] {3.6f,0.15585f,-2.764f,-4.715f,-5.4f,-4.715f,-2.764f,0.15585f,3.6f};
	private float[] idealPosXList = new float[9] {-7.65f,-7.0677f,-5.4094f,-2.928f,0f,2.928f,5.4094f,7.0677f,7.65f};
	private float[] idealPosYList = new float[9] {3.6f,0.672f,-1.809f,-3.468f,-4.05f,-3.468f,-1.809f,0.672f,3.6f};

	void Start () {
		gameTime = 0;
		//laneValue = 1;
	}
	
	void Update () {
		//moveNote();
		moveNote2();
	}

	private void moveNote2(){
		iTween.MoveTo(gameObject,iTween.Hash("position",new Vector3(idealPosXList[laneValue],idealPosYList[laneValue],0f),"easeType","linear","time",1.0f,"oncomplete","destroyNote"));
		//iTween.MoveTo(gameObject,iTween.Hash("position",new Vector3(posXList[laneValue],posYList[laneValue],0f),"easeType","linear","time",2.0f,"oncomplete","destroyNote"));
		iTween.ScaleTo(gameObject,iTween.Hash("x",0.4f,"y",0.4f,"time",1.0f,"easeType","easeOutSine"));
	}

	private void destroyNote(){
		Destroy(gameObject);
		NoteCreator2.nextNoteValue[laneValue]++;
		StatusManager.deleteCount++;
	}

	private void moveNote(){
		gameTime += Time.deltaTime;
		if(laneValue >= 0 && laneValue <= 3){
			posX = -gameTime * speed;
		}else if(laneValue >= 5 && laneValue <= 8){
			posX = gameTime * speed;
		}
		switch(laneValue){
			case 0:
				posY = 3.6f;
				break;
			case 1:
				posY = 0.41428f * posX + 3.6f;
				break;
			case 2:
				posY = 1.0f * posX + 3.6f;
				break;
			case 3:
				posY = 2.41393f * posX + 3.6f;
				break;
			case 4:
				posY = -gameTime * speed + 3.6f;
				break;
			case 5:
				posY = -2.41393f * posX + 3.6f;
				break;
			case 6:
				posY = -1.0f * posX + 3.6f;
				break;
			case 7:
				posY = -0.41428f * posX + 3.6f;
				break;
			case 8:
				posY = 3.6f;
				break;
			default:
				break;
		}
		gameObject.transform.localPosition = new Vector3(posX,posY,0);
	}

}
