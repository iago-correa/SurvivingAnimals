using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BunnyController : MonoBehaviour {

	public float speed;
	private float moveInputX;
	private float moveInputY;

	private Rigidbody2D rb;
	private bool facingRight = true;

	private Animator anim;
	private float idleTime;
	private float totalTime;

	// Use this for initialization
	void Start () {

		rb = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();

		idleTime = 5;
		totalTime = 0;

	}
	
	// Update is called once per frame
	void Update () {
		
		moveInputX = Input.GetAxis("Horizontal");
		moveInputY = Input.GetAxis("Vertical");

		rb.velocity = new Vector2 (moveInputX * speed, moveInputY * speed);

		if (!facingRight && moveInputX > 0) {

			Flip ();

		}else if(facingRight && moveInputX <0){

			Flip ();

		}

		if(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)){
			anim.SetBool("isWalking", true);
			totalTime = 0;
		} else {
			anim.SetBool("isWalking", false);
		}

		if(!(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)) && totalTime >= idleTime){
			anim.SetBool("isIdling", true);
		} else{
			anim.SetBool("isIdling", false);
		}

		// Para não rodar com o planeta
		transform.Rotate(Vector3.forward * Time.deltaTime * -1);

		totalTime += Time.deltaTime;

	}

	void Flip(){

		facingRight = !facingRight;
		Vector3 Scaler = transform.localScale;
		Scaler.x *= -1;
		transform.localScale = Scaler;

	}
}
