using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusManager : MonoBehaviour {

	public static int deleteCount = 0;
	private int tmpDeteleCount = 0;
	private AudioSource audioSe;

	// Use this for initialization
	void Start () {
		audioSe = gameObject.GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		if(deleteCount > tmpDeteleCount){
			tmpDeteleCount = deleteCount;
			audioSe.Play();
		}
	}
}
