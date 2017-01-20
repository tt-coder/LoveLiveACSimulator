using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteCreator : MonoBehaviour {

	public GameObject singleNote;
	private GameObject obj;
	private float timeleft;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		timeleft -= Time.deltaTime;
        if (timeleft <= 0.0) {
            timeleft = 1.0f;
			/*
			for(int i=0;i<9;i++){
				createNote(i);
			}
			*/
			createNote();
        }
	}
	private void createNote(){
		obj = Instantiate(singleNote);
		obj.transform.parent = GameObject.Find("NoteParent").transform;
		obj.transform.position = new Vector3(0,3.6f,0);
		obj.GetComponent<NoteMove>().laneValue = Random.Range(0,8);
	}
	/*
	private void createNote(int num){
		obj = Instantiate(singleNote);
		obj.transform.parent = GameObject.Find("NoteParent").transform;
		obj.transform.position = new Vector3(0,3.6f,0);
		//obj.GetComponent<NoteMove>().laneValue = Random.Range(0,8);
		obj.GetComponent<NoteMove>().laneValue = num;
	}*/

}
