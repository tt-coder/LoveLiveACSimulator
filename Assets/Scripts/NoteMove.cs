using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteMove : MonoBehaviour {

	private float gameTime;
	private float posX,posY,speed = 1;
	public int laneValue;

	void Start () {
		gameTime = 0;
		laneValue = 1;
	}
	
	void Update () {
		moveNote();
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
				posY = 0.41844f * posX + 3.6f;
				break;
			case 2:
				posY = 1.0f * posX + 3.6f;
				break;
			case 3:
				posY = 2.43103f * posX + 3.6f;
				break;
			case 4:
				posY = -gameTime * speed + 3.6f;
				break;
			case 5:
				posY = 2.0862f * posX + 3.6f;
				break;
			case 6:
				posY = 2.0862f * posX + 3.6f;
				break;
			case 7:
				posY = 2.0862f * posX + 3.6f;
				break;
			case 8:
				posY = 2.0862f * posX + 3.6f;
				break;
			default:
				break;
		}
		gameObject.transform.localPosition = new Vector3(posX,posY,0);
	}

}
