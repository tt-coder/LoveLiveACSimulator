using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusManager : MonoBehaviour {

	public GameObject judgeObj;
	public GameObject comboObj;
	private Text judgeText;
	private Text comboText;
	public static int[] noteCount = new int[6] {0,0,0,0,0,0}; // 全体の消した数、Perfect、Great、Good、Bad，Miss
	private int[] tmpNoteCount = new int[6] {0,0,0,0,0,0};
	public static int combo = 0;
	private int tmpCombo = 0;
	private int tmpDeteleCount = 0;
	private AudioSource audioSe;
	public static float audioLength;

	void Awake(){
		Application.targetFrameRate = 60;
	}

	void Start () {
		judgeText = judgeObj.GetComponent<Text>();
		comboText = comboObj.GetComponent<Text>();
		audioSe = gameObject.GetComponent<AudioSource>();
	}
	
	void Update () {
		checkNoteCount();
		displayStatus();
	}

	private void checkNoteCount(){
		for(int i=0;i<6;i++){
			if(noteCount[i] > tmpNoteCount[i]){
				tmpNoteCount[i] = noteCount[i];
				if(i != 0){
					//setCombo(i);
					setJudge(i);
					playSE(i);
				}
			}
		}
		if(combo > tmpCombo){
			tmpCombo = combo;
			iTween.ScaleTo(comboObj,iTween.Hash("x",1.2f,"y",1.2f,"time",0.2f,"oncomplete", "endComboEffect","oncompletetarget", gameObject));
		}else if(combo == 0){
			tmpCombo = 0;
		}
	}

	private void setCombo(int judge){
		if(judge == 1 || judge == 2){
			combo++;
			iTween.ScaleTo(comboObj,iTween.Hash("x",1.2f,"y",1.2f,"time",0.2f,"oncomplete", "endComboEffect","oncompletetarget", gameObject));
		}else if(judge == 3 || judge == 5){
			combo = 0;
		}else if(judge == 5){
			combo = 0;
		}
	}

	private void setJudge(int judge){
		comboObj.transform.localScale = new Vector3(1,1,0);
		switch(judge){
			case 1:
				judgeText.text = "PERFECT";
				break;
			case 2:
				judgeText.text = "GREAT";
				break;
			case 3:
				judgeText.text = "GOOD";
				break;
			case 4:
				judgeText.text = "BAD";
				break;
			case 5:
				judgeText.text = "MISS";
				break;
		}
		iTween.ScaleTo(judgeObj, iTween.Hash("scale", new Vector3(1.2f, 1.2f, 0), "time", 0.1f, "oncomplete", "endJudgeEffect","oncompletetarget", gameObject));
	}

	private void endComboEffect() { // 判定用エフェクト終了時の縮小処理
        iTween.ScaleTo(comboObj, iTween.Hash("scale", new Vector3(1.0f, 1.0f, 0), "time", 0.01f, "looptype", "none"));
    }

	private void endJudgeEffect() { // 判定用エフェクト終了時の縮小処理
        iTween.ScaleTo(judgeObj, iTween.Hash("scale", new Vector3(0f, 0f, 0), "time", 0.05f, "looptype", "none"));
    }

	private void playSE(int judge){
		switch(judge){
			case 5:
				break;
			default:
				audioSe.Play();
				break;
		}
	}

	private void displayStatus(){
		if(combo != 0){
			comboText.text = combo.ToString() + " COMBO";
		}else{
			comboText.text = "";
		}
	}

}
