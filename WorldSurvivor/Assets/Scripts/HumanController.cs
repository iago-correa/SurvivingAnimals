using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanController : MonoBehaviour {

	public int moveType;	
	public bool onHunt;
	private GameObject prey;
	public float speed;
	public float timeToGiveUp;
	private float timeSpent;
	private bool surrender;
	
	public bool shock;
	public float shockTime;
	private float timeSpentShock;
	public Vector3 shockVec;
	private float magnitude;

	public float timeOutScreen;
	public Vector3 posInit;
	private Renderer humanRenderer;
	private bool needReplace;
	private bool visibility;

	public float flipTime;
	private float toFlipTime;


	// Use this for initialization
	void Start () {
		
		timeSpent = 0f;
		timeSpentShock = 0f;
		//posInit = transform.position;
		//Debug.Log(posInit);
		humanRenderer = GetComponent<Renderer>();
		needReplace = false;
		visibility = true;

	}
	
	// Update is called once per frame
	void Update () {


		if(onHunt && !shock){
			var teste = prey.GetComponent<BunnyController>().hidden;
			if(!teste && !shock){
				var target = prey.transform.position;
				transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
			}//else if(teste){
				//totalSpent += Time.deltaTime;
			//}
		} 

		if(shock){
			if(timeSpentShock <= shockTime){
				shockFunc();
				timeSpentShock += Time.deltaTime;
			}else{
				timeSpentShock = 0;
				shock = false;
			}
		}

		if(!onHunt && !shock){
			transform.position = transform.position;
		}


		if(toFlipTime >= flipTime){
			flip();
			toFlipTime = 0;
		}else if(!onHunt){
			toFlipTime += Time.deltaTime;
		}
		
		//transform.position = posInit;

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

	void OnCollisionEnter2D(Collision2D other){
		if(other.gameObject.tag == "Player"){
			other.gameObject.GetComponent<BunnyController>().life -= 40;
			shock = true;
			shockVec = other.gameObject.transform.position;
		}else{
    		Physics2D.IgnoreCollision(other.collider, GetComponent<Collider2D>());
		}
	}

	void shockFunc(){
		var force = transform.position - shockVec;
		force.Normalize();
		force = new Vector3(force.x,0,0);
		gameObject.GetComponent<Rigidbody2D>().AddForce(force * magnitude);
	}

	void flip(){
		//transform.Rotate(Vector3.forward);
		//transform.localRotation = transform.localRotation+Quaternion.Euler(0, 180, 0);
		transform.localScale = new Vector3(transform.localScale.x *-1, transform.localScale.y, transform.localScale.z);
		//Debug.Log("Vira");
	}

}
