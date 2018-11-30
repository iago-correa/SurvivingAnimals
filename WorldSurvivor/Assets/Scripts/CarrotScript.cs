using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarrotScript : MonoBehaviour {

	public float floatFactor;
	public int energy; // normal de 30 a 50, rara de 100 a 150
	public int health; //1 ou 2
	public bool isFloating;
	private Animator anim;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		float prob = Random.value;
		if(prob <= floatFactor){
			isFloating = true;
			prob = Random.value;
			energy = 100 + (int)(50*prob);
			health = 2;
			anim.SetBool("isRare", true);
		}else{
			isFloating = false;
			energy = 30 + (int)(20 * prob);
			health = 1;
			anim.SetBool("isRare", false);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
