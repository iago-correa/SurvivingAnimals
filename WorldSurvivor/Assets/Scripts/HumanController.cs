using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanController : MonoBehaviour {

	public int moveType;	
	public bool onHunt;
	private GameObject prey;
	public float speed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if(onHunt){
			var target = prey.transform.position;
			transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
		}
		
	}

	void OnTriggerEnter2D(Collider2D other){
		if(other.gameObject.tag == "Player"){
			onHunt = true;
			prey = other.gameObject;
		}
	}

	void OnTriggerExit2D(Collider2D other){
		if(other.gameObject.tag == "Player"){
			onHunt = false;
		}
	}

}
