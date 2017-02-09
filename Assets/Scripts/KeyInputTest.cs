﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;
using UniRx.Triggers;

public class KeyInputTest : MonoBehaviour {
	private GameObject[] button = new GameObject[9];
	//private string[] key = new string[9] {"a","s","d","f"," ","h","j","k","l"};
	//private int[] keyBuffer = new int[9] {0,0,0,0,0,0,0,0,0};
	private int keyBuffer = 1000000000;
	private int checkBuffer = 1000000000;
	private int[] buffer = new int[16];
	private int idealKeyBinary = 1000000000;
	protected int[] keyBinary = new int[9] {1000000001,1000000010,1000000100,1000001000,1000010000,1000100000,1001000000,1010000000,1100000000};
	void Start () {
		for(int i=0;i<9;i++){
			button[i] = GameObject.Find("Button" + (i+1).ToString());
		}
		idealKeyBinary |= keyBinary[0];
		idealKeyBinary |= keyBinary[1];
		//Debug.Log(idealKeyBinary);
	}

	void Update () {
		for(int i = 15;i>0;i--){
			buffer[i] = buffer[i-1];
		}
		buffer[0] = 1000000000;
		//detectionKeyInput();
		keyTest();
		keyCheck();
		//Debug.Log(keyCheck());
	}

	void keyTest(){
		if(Input.GetKeyDown("a")){
			buffer[0] |= keyBinary[0];
		}
		if(Input.GetKeyDown("s")){
			buffer[0] |= keyBinary[1];
		}
		//buffer[0] |= 1000000000;
		//Debug.Log(keyBinary[0] |= keyBinary[1]);
	}

	bool keyCheck(){
		for(int i=0;i<2;i++){
			checkBuffer |= buffer[i];
		}
		if(checkBuffer == idealKeyBinary){
			changeColor(0,true);
			changeColor(1,true);
			checkBuffer = 1000000000;
			Debug.Log("OK");
			return true;
		}else{
			changeColor(0,false);
			changeColor(1,false);
			checkBuffer = 1000000000;
			return false;
		}
	}

	void detectionKeyInput(){
		if(Input.GetKey("a")){
			changeColor(0,true);
		}else{
			changeColor(0,false);
		}
		if(Input.GetKey("s")){
			changeColor(1,true);
		}else{
			changeColor(1,false);
		}
		if(Input.GetKey("d")){
			changeColor(2,true);
		}else{
			changeColor(2,false);
		}
		if(Input.GetKey("f")){
			changeColor(3,true);
		}else{
			changeColor(3,false);
		}
		if(Input.GetKey("space")){
			changeColor(4,true);
		}else{
			changeColor(4,false);
		}
		if(Input.GetKey("h")){
			changeColor(5,true);
		}else{
			changeColor(5,false);
		}
		if(Input.GetKey("j")){
			changeColor(6,true);
		}else{
			changeColor(6,false);
		}
		if(Input.GetKey("k")){
			changeColor(7,true);
		}else{
			changeColor(7,false);
		}
		if(Input.GetKey("l")){
			changeColor(8,true);
		}else{
			changeColor(8,false);
		}
	}

	void changeColor(int i, bool b){
		if(b == true){
			button[i].GetComponent<SpriteRenderer>().color = new Color(255f,255f,255f,1f);
		}else{
			button[i].GetComponent<SpriteRenderer>().color = new Color(255f,255f,255f,0.2f);
		}
	}
}
