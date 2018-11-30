using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundController : MonoBehaviour {

	public Transform carrot;

	// Use this for initialization
	void Start () {
		var carrot0 = Instantiate(carrot, transform.position + new Vector3(-1.1f, 0.7f, 0), Quaternion.identity);
		carrot0.transform.parent = gameObject.transform;
		var carrot1 = Instantiate(carrot, transform.position + new Vector3(-1.1f + 1, 0.7f, 0), Quaternion.identity);
		carrot1.transform.parent = gameObject.transform;
		var carrot2 = Instantiate(carrot, transform.position + new Vector3(-1.1f + 2, 0.7f, 0), Quaternion.identity);
		carrot2.transform.parent = gameObject.transform;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
