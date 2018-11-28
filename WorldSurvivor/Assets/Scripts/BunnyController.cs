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
	private float multiplier;
	public bool walkable;
	private Rigidbody2D rb;
	private bool facingRight = true;

	private Animator anim;
	private float idleTime;
	private float totalTime;

	private bool hidden;
	private SpriteRenderer renderer;
	public SpriteRenderer earRenderer;

	public bool sleeping = false;
	private bool toSleep = false;
	private float initSleep;
	private float totalSleeping;
	public SpriteRenderer sleepRenderer;
	public GameObject world;
	private float initMultiplier;
	public int incrMultiplier;
	private int amountIcrEnergy = 0;

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

		healthBar.value = life;
		energyBar.value = energy;

		if(!sleeping){

			energy = energy - 1;

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

			if(hidden == true){
				Hide(0.5f);
			}else if(hidden == false){
				Unhide();
			}

		}

		if(toSleep == true && Input.GetKeyUp(KeyCode.Space) && energy < 0.7*10000){
			Sleep();
		} 
		
		if(sleeping == true){
			world.GetComponent<EarthController>().multiplier = 100;
			if(amountIcrEnergy < 7000){
				energy += (int)7000/(100);
				amountIcrEnergy += (int)7000/(100);
			}
			if(energy > 10000){
				energy = 10000;
			}
			totalSleeping = world.transform.localRotation.z;
			if(initSleep - totalSleeping < 0.1 && initSleep - totalSleeping > 0){
				Wake();
				totalSleeping = 0;
			}
		}

		// Para não rodar com o planeta
		multiplier = world.GetComponent<EarthController>().multiplier;
		transform.Rotate(Vector3.forward * Time.deltaTime * -1*multiplier);

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

		if(other.gameObject.tag == "Earth"){
			walkable = true;
		}

	}

	void OnTriggerExit2D(Collider2D other){

		if(other.gameObject.tag == "Bush"){
			hidden = false;
		}

		if(other.gameObject.tag == "Hole"){
			toSleep = false;
		}

		if(other.gameObject.tag == "Earth"){
			walkable = false;
		}

	}

	void Hide(float alpha){
		renderer.color = new Color(1f, 1f, 1f, alpha); 
		earRenderer.color = new Color(1f, 1f, 1f, alpha); 
	}

	void Unhide(){
		renderer.color = new Color(1f, 1f, 1f, 1f); 
		earRenderer.color = new Color(1f, 1f, 1f, 1f); 
	}

	void Sleep(){
		initMultiplier = world.GetComponent<EarthController>().multiplier;
		Hide(0);
		sleepRenderer.color = new Color(1f, 1f, 1f, 1f);
		Debug.Log("Dormir");
		sleeping = true;
		amountIcrEnergy = 0;
		initSleep = world.transform.localRotation.z;
	}

	void Wake(){
		world.GetComponent<EarthController>().multiplier = initMultiplier + incrMultiplier;
		Unhide();
		sleepRenderer.color = new Color(1f, 1f, 1f, 0f);
		Debug.Log("Acordar");
		sleeping = false;
		toSleep = false;
	}

}
