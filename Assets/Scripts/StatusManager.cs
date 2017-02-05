using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusManager : MonoBehaviour {

	public GameObject judge;
	public GameObject comboObj;
	private Text judgeText;
	private Text comboText;
	public static int[] noteCount = new int[6] {0,0,0,0,0,0}; // 全体の消した数、Perfect、Great、Good、Bad，Miss
	private int[] tmpNoteCount = new int[6] {0,0,0,0,0,0};
	private int combo = 0;
	private int tmpDeteleCount = 0;
	private AudioSource audioSe;
	public static float audioLength;

	void Awake(){
		Application.targetFrameRate = 60;
	}

	void Start () {
		judgeText = judge.GetComponent<Text>();
		comboText = comboObj.GetComponent<Text>();
		audioSe = gameObject.GetComponent<AudioSource>();
		iTween.ValueTo(gameObject,iTween.Hash("from",1f,"to",0f,"time",0.75f,"onupdate","SetValue"));
	}

	void SetValue(float alpha) {
		// iTweenで呼ばれたら、受け取った値をImageのアルファ値にセット
		judgeText.color = new Color(255,255,255, alpha);
		
	}
	
	void Update () {
		checkNoteCount();
		displayStatus();
	}

	private void checkNoteCount(){
		for(int i=0;i<6;i++){
			if(noteCount[i] > tmpNoteCount[i]){
				tmpNoteCount[i] = noteCount[i];
				setCombo(i);
				playSE(i);
			}
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

	private void endComboEffect() { // 判定用エフェクト終了時の縮小処理
        iTween.ScaleTo(comboObj, iTween.Hash("scale", new Vector3(1.0f, 1.0f, 0), "time", 0.01f, "looptype", "none"));
    }

	private void playSE(int judge){
		switch(judge){
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
