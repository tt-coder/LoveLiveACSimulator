using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteCreator : MonoBehaviour {

	public GameObject singleNote;
	private GameObject obj;
	private float timeleft;
	public static int[] laneNoteCount = new int[9];
	public static int[] nextNoteValue = new int[9];

	void Start () {
		for(int i=0;i<9;i++){
			laneNoteCount[i] = 0;
			nextNoteValue[i] = 0;
		}
	}

	void Update () {
		timeleft -= Time.deltaTime;
        if (timeleft <= 0.0) {
            timeleft = 1.0f;
			/*
			for(int i=0;i<9;i++){
				createNote(i);
			}
			*/
			//createNote();
			createNote(0);
        }
	}
	/*
	private void createNote(){
		obj = Instantiate(singleNote);
		obj.transform.parent = GameObject.Find("NoteParent").transform;
		obj.transform.position = new Vector3(0,3.6f,0);
		int lane = Random.Range(0,8);
		obj.GetComponent<NoteMove>().laneValue = lane;
		obj.GetComponent<SingleNote>().laneIndex = laneNoteCount[lane];
		laneNoteCount[lane]++;
	}
	*/
	private void createNote(int num){
		obj = Instantiate(singleNote);
		obj.transform.parent = GameObject.Find("NoteParent").transform;
		obj.transform.position = new Vector3(0,3.6f,0);
		obj.GetComponent<NoteMove>().laneValue = num;
		obj.GetComponent<SingleNote>().laneIndex = laneNoteCount[num];
		laneNoteCount[num]++;
	}

}
