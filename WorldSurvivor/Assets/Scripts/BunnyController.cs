using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BunnyController : MonoBehaviour {

	public float speed;
	public int life;
	public int energy;

	public Slider healthBar;
	public Slider energyBar;

	private float moveInputX;
	private float moveInputY;

	private Rigidbody2D rb;
	private bool facingRight = true;

	private Animator anim;
	private float idleTime;
	private float totalTime;

	private bool hidden;
	private SpriteRenderer renderer;
	public SpriteRenderer earRenderer;

	private bool sleeping = false;
	private bool toSleep = false;
	public float timeToWake;
	private float totalSleeping;

	// Use this for initialization
	void Start () {

		rb = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();

		renderer = GetComponent<SpriteRenderer>();

		idleTime = 5;
		totalTime = 0;

	}
	
	// Update is called once per frame
	void Update () {

		energy = energy - 1;
		healthBar.value = life;
		energyBar.value = energy;
		
		moveInputX = Input.GetAxis("Horizontal");
		moveInputY = Input.GetAxis("Vertical");

		rb.velocity = new Vector2 (moveInputX * speed, moveInputY * speed);

		if (!facingRight && moveInputX > 0) {

			Flip ();

		}else if(facingRight && moveInputX <0){

			Flip ();

		}

		if(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.UpArrow)){
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

		if(hidden == true && sleeping == true){
			Hide(0, false);
		}else if(hidden == true){
			Hide(0.5f, true);
		}else if(hidden == false){
			Unhide();
		}


		if(toSleep == true && Input.GetKeyUp(KeyCode.Space) && energy < 0.7*10000){
			Sleep();
		} 
		
		if(sleeping == true){
			energy += 7000/(int)(timeToWake*60);
			totalSleeping += Time.deltaTime;
		}

		if(sleeping == true && totalSleeping >= timeToWake){
			Wake();
			totalSleeping = 0;
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

	void OnTriggerEnter2D(Collider2D other){

		if(other.gameObject.tag == "Bush"){
			hidden = true;
		}

		if(other.gameObject.tag == "Hole"){
			toSleep = true;
		}

	}

	void OnTriggerExit2D(Collider2D other){

		if(other.gameObject.tag == "Bush"){
			hidden = false;
		}

	}

	void Hide(float alpha, bool body){
		if(body){
			renderer.color = new Color(1f, 1f, 1f, alpha); 
			earRenderer.color = new Color(1f, 1f, 1f, alpha); 
		}else if(!body){
			earRenderer.color = new Color(1f, 1f, 1f, alpha); 
		}
	}

	void Unhide(){
		renderer.color = new Color(1f, 1f, 1f, 1f); 
		earRenderer.color = new Color(1f, 1f, 1f, 1f); 
	}

	void Sleep(){
		Debug.Log("Dormir");
		anim.SetBool("isSleeping",true);
		hidden = true;
		sleeping = true;
		Hide(0, false);
	}

	void Wake(){
		Debug.Log("Acordar");
		anim.SetBool("isSleeping", false);
		sleeping = false;
		toSleep = false;
		hidden = false;
	}

}
