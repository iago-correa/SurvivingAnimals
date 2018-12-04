using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManualController : MonoBehaviour {

	private AudioSource menuSound;

	// Use this for initialization
	void Start () {
		GameObject.FindGameObjectWithTag("BackSong").GetComponent<PlayerScript>().PlayMusic();
		menuSound = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space)){
 			SceneManager.LoadScene("PhaseScene", LoadSceneMode.Single);
 			menuSound.Play();
		}
	}
}
