using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseEventHandler : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameObject.FindGameObjectWithTag("BackSong").GetComponent<PlayerScript>().PlayMusic();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
