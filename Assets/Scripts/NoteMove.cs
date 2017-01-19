using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteMove : MonoBehaviour {

	private float gameTime;
	private float posX,posY,speed = 3;
	// Use this for initialization
	void Start () {
		gameTime = 0;
		//iTween.MoveTo(gameObject,iTween.Hash("x",-2.9f,"y",-3.45f,"time",1f));
	}
	
	// Update is called once per frame
	void Update () {
		moveNote();
	}

	private void moveNote(){
		gameTime += Time.deltaTime;
		posX = -gameTime * speed;
		posY = 2.0862f * posX + 2.6f;
		gameObject.transform.localPosition = new Vector3(posX,posY,0);

	}

}
