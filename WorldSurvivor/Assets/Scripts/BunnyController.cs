using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

	public bool hidden;
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

	public bool shock;
	public Vector3 shockVec;
	public float shockTime;
	private float timeSpentShock;
	public int magnitude;

	public bool startDig;
	public bool isDigging;
	public float digTime;
	private float diggingTime;
	private float undiggingTime;
	private bool isUndigging;
	public SpriteRenderer digHole;

	public GameObject shockBlood;
	public GameObject deathBlood;

	private float noEnergyTimer;
	private bool dead;

	private GameObject gameOver;
	private float timeChangeScene;

	private AudioSource[] sounds;
	private bool soundsControlStep;

	// Use this for initialization
	void Start () {

		rb = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();

		renderer = GetComponent<SpriteRenderer>();

		sounds = GetComponents<AudioSource>();

		rb.gravityScale = 0;

		idleTime = 5;
		totalTime = 0;
		timeSpentShock = 0;
		timeChangeScene = 0;

		soundsControlStep = false;
		dead = false;

		gameOver = GameObject.FindGameObjectWithTag("GameOver");
		gameOver.SetActive(false);

	}
	
	// Update is called once per frame
	void Update () {

		if(life <= 0 && !dead){
			die();
		}else if(dead){
			Debug.Log("Ttimer");
			Debug.Log(timeChangeScene);
			if(timeChangeScene >= 4){
				Debug.Log("Trocar");
				SceneManager.LoadScene("MainMenuScene", LoadSceneMode.Single);
				GameObject.FindGameObjectWithTag("BackSong").GetComponent<PlayerScript>().StopMusic();
				Destroy(GameObject.FindGameObjectWithTag("Player"),0.3f);
			}else{
				timeChangeScene += Time.deltaTime;
			}
		}

		healthBar.value = life;
		energyBar.value = energy;
		
		if(!sleeping){

			energy = energy - 1;

			if(!isDigging && !isUndigging && !dead){
				moveInputX = Input.GetAxis("Horizontal");
				moveInputY = Input.GetAxis("Vertical");
			}else{
				moveInputX = 0;
				moveInputY = 0;
			}

			rb.velocity = new Vector2 (moveInputX * speed, moveInputY * speed);

			if (!facingRight && moveInputX > 0) {

				Flip ();

			}else if(facingRight && moveInputX <0){

				Flip ();

			}

			if(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.UpArrow)){
				anim.SetBool("isWalking", true);
				totalTime = 0;
				if(soundsControlStep == false){
					sounds[1].Play();	
					soundsControlStep = true;
				}
			} else {
				anim.SetBool("isWalking", false);
				if(soundsControlStep == true){
					sounds[1].Pause();	
					soundsControlStep = false;
				}
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

			if(shock){
				if(timeSpentShock <= shockTime){
					shockFunc();
					timeSpentShock += Time.deltaTime;
				}else{
					timeSpentShock = 0;
					shock = false;
				}
			}

			if(isDigging){
				diggingTime += Time.deltaTime;
				Hide(1 - diggingTime/digTime);
				digHole.color = new Color(1f, 1f, 1f, diggingTime/digTime); 
			}else if(Input.GetKeyDown(KeyCode.Z)){
				dig();
			}

			if(isDigging && diggingTime >= digTime && !isUndigging){
				isUndigging = true;
				transform.position = world.transform.position + new Vector3(0f,34.5f,0f);
				sounds[0].Play();
			}

			if(isUndigging && undiggingTime < digTime){
				undiggingTime += Time.deltaTime;
				Hide(undiggingTime/digTime);
				digHole.color = new Color(1f, 1f, 1f, (undiggingTime/digTime)); 
				endDig();
			}else if(isDigging && undiggingTime >= digTime){
				stopDig();
				digHole.color = new Color(1f, 1f, 1f, 0); 
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
			Debug.Log("totalSleeping");
			Debug.Log(totalSleeping);
			if(initSleep - totalSleeping < 0.05 && initSleep - totalSleeping > 0){
				Wake();
				totalSleeping = 0;
			}

		}

		if(energy <=0){
			noEnergyTimer += Time.deltaTime;
			if(noEnergyTimer >= 0.5){
				noEnergy();
				noEnergyTimer = 0;
			}
		} else {
			speed = 7;
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

	void OnCollisionEnter2D(Collision2D other){
		if(other.gameObject.tag == "Food"){
			if(life + other.gameObject.GetComponent<CarrotController>().health <= 100){
				life += other.gameObject.GetComponent<CarrotController>().health;
			}
			energy += other.gameObject.GetComponent<CarrotController>().energy;
			sounds[4].Play();
			other.gameObject.GetComponent<BoxCollider2D>().enabled = false;
			other.gameObject.GetComponent<Renderer>().enabled = false;
		}else if(other.gameObject.tag == "Human"){
			shock = true;
			shockVec = other.gameObject.transform.position;
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
		Debug.Log("initSleep");
		Debug.Log(initSleep);
	}

	void Wake(){
		world.GetComponent<EarthController>().multiplier = initMultiplier + incrMultiplier;
		Unhide();
		sleepRenderer.color = new Color(1f, 1f, 1f, 0f);
		Debug.Log("Acordar");
		sleeping = false;
		toSleep = false;
	}

	void shockFunc(){
		Instantiate(shockBlood, transform.position, transform.rotation);
		var force = transform.position - shockVec;
		force.Normalize();
		force = new Vector3(force.x,0,0);
		gameObject.GetComponent<Rigidbody2D>().AddForce(force * magnitude);
		sounds[2].Play();	
	}

	void dig(){
		Debug.Log("Cavar");
		isDigging = true;
		anim.SetBool("isDigging", true);
		rb.gravityScale = 4;
		sounds[0].Play();
	}

	void stopDig(){
		Debug.Log("Parar de cavar");
		isDigging = false;
		isUndigging = false;
		anim.SetBool("isDigging", false);
		diggingTime = 0;
		undiggingTime = 0;
		rb.gravityScale = 0;
		Unhide();
		energy -= 500;
	}

	void endDig(){
		rb.gravityScale = -4;
	}

	void die(){
		sounds[3].Play();
		Instantiate(deathBlood, transform.position, transform.rotation);		
		world.GetComponent<EarthController>().multiplier = 0;

		Debug.Log(gameOver);
		gameOver.SetActive(true);
		renderer.enabled = false;
		earRenderer.enabled = false;
		dead = true;

	}

	void noEnergy(){
		speed = 3;
		life -= 5;
	}

}
