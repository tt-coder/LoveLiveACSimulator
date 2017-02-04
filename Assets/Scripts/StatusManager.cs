using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusManager : MonoBehaviour {

	public GameObject judge;
	private Text judgeText;

	public static int[] noteCount = new int[6] {0,0,0,0,0,0}; // 全体の消した数、Perfect、Great、Good、Bad，Miss
	private int[] tmpNoteCount = new int[6] {0,0,0,0,0,0};

	private int tmpDeteleCount = 0;
	private AudioSource audioSe;

	void Awake(){
		Application.targetFrameRate = 60;
	}

	void Start () {
		judgeText = judge.GetComponent<Text>();
		audioSe = gameObject.GetComponent<AudioSource>();
		iTween.ValueTo(gameObject,iTween.Hash("from",1f,"to",0f,"time",0.75f,"onupdate","SetValue"));
	}

	void SetValue(float alpha) {
		// iTweenで呼ばれたら、受け取った値をImageのアルファ値にセット
		judgeText.color = new Color(255,255,255, alpha);
		
	}
	
	void Update () {
		checkNoteCount();
	}

	private void checkNoteCount(){
		for(int i=0;i<6;i++){
			if(noteCount[i] > tmpNoteCount[i]){
				tmpNoteCount[i] = noteCount[i];
				if(i!=0 && i!=5){
					audioSe.Play();
				}
			}
		}
	}
}
