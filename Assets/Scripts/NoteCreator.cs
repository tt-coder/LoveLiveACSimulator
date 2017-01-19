using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteCreator : MonoBehaviour {

	public GameObject singleNote;
	private GameObject obj;

	// Use this for initialization
	void Start () {
		obj = Instantiate(singleNote);
		obj.transform.parent = GameObject.Find("NoteParent").transform;
		obj.transform.position = new Vector3(0,3.6f,0);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
