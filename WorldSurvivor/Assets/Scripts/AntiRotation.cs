using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiRotation : MonoBehaviour {

	private float multiplier;
	public GameObject world;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

		multiplier = world.GetComponent<EarthController>().multiplier;
		transform.Rotate(Vector3.forward * Time.deltaTime * -1*multiplier);
		
	}
}
