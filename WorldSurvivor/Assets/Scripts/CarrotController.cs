using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarrotController : MonoBehaviour {

	public float floatFactor;
	public int energy; // normal de 30 a 50, rara de 100 a 150
	public int health; //1 ou 2
	public bool isFloating;
	private Animator anim;

	public float timer;
	public bool status;

	// Use this for initialization
	void Start () {

		anim = GetComponent<Animator> ();
		float prob = Random.value;
		if(prob <= floatFactor){
			isFloating = true;
			prob = Random.value;
			energy = 200 + (int)(100*prob);
			health = 2;
			anim.SetBool("isRare", true);
		}else{
			isFloating = false;
			energy = 100 + (int)(50 * prob);
			health = 1;
			anim.SetBool("isRare", false);
		}

		timer = 100;
	}
	
	// Update is called once per frame
	void Update () {
		if(timer < 100){
			timer += Time.deltaTime;
		}else{
			GetComponent<BoxCollider2D>().enabled = true;
			GetComponent<Renderer>().enabled = true;
		}
	}

	void OnCollisionEnter2D(Collision2D other){
		if(other.gameObject.tag == "Player"){
			timer = 0;
		}
	}

}
