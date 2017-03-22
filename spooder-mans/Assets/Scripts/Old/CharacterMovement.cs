﻿using UnityEngine;
using System.Collections;

public class CharacterMovement : MonoBehaviour {
	
	public float playerSpeed;
	public float verticalAccel;
	public float maxHorizSpeed;
	public float horizAccel;
	public float max_jump_accel_frames;
	public float jumpAccel;
	public float jumpDegrade;
	public Vector2 left_wall_jump_vector;
	public Vector2 right_wall_jump_vector;
	public GameObject theHook;
	
	private bool jumpPressed;
	private bool jumpReleased = true;
	private CharacterAttributes attributes;
	private int framesAccelerating = 0;
	private int playerNum;
	private PlayerInput playerInput;
	
	//public AudioSource jumpAudio;	
	
	private SoundManager soundManager;
	public SoundManager theSoundManager;
	
	// Use this for initialization
	void Start () {
		attributes = GetComponent<CharacterAttributes> ();
		playerNum = attributes.playerNum;
		playerInput = InputManager.PlayerInputs [playerNum];
		
		soundManager = (SoundManager)theSoundManager.GetComponent<SoundManager>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		//Moving
		if ((attributes.OnWall && !attributes.Sweeping && !attributes.HookLaunched && !attributes.Hooked )) {
			GetComponent<Rigidbody2D>().velocity += new Vector2( 0f, playerInput.leftJoystickY) * verticalAccel;
			if( GetComponent<Rigidbody2D>().velocity.magnitude > playerSpeed )
			{
				GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity.normalized * playerSpeed;
			}
			if( playerInput.leftJoystickY.IsWithin(0f, 0.001f) && !attributes.Hooked && !attributes.HookTraveling)
			{
				GetComponent<Rigidbody2D>().velocity = Vector2.zero;
			}
			
			if (GetComponent<Rigidbody2D>().velocity.y < 0 )
			{
				attributes.anim.SetBool("Idle", false);
				attributes.anim.SetBool("Up", false);
				attributes.anim.SetBool("Stunned", false);
				attributes.anim.SetBool("Down", true);
			}
			else if (GetComponent<Rigidbody2D>().velocity.y > 0)
			{
				attributes.anim.SetBool("Idle", false);
				attributes.anim.SetBool("Down", false);
				attributes.anim.SetBool("Stunned", false);
				
				attributes.anim.SetBool("Up", true);
			}
			else if (GetComponent<Rigidbody2D>().velocity.y == 0)
			{
				attributes.anim.SetBool("Idle", true);
				attributes.anim.SetBool("Down", false);
				attributes.anim.SetBool("Stunned", false);
				attributes.anim.SetBool("Up", false);
			}
			
		}
		
		if( !attributes.OnWall && !attributes.HookTraveling && !attributes.Hooked && !attributes.Sweeping && !attributes.Swept )
		{
			GetComponent<Rigidbody2D>().velocity += new Vector2( playerInput.leftJoystickX , 0f );
			
			if( Mathf.Abs( GetComponent<Rigidbody2D>().velocity.x ) > maxHorizSpeed )
			{
				GetComponent<Rigidbody2D>().velocity = new Vector2( GetComponent<Rigidbody2D>().velocity.x / Mathf.Abs( GetComponent<Rigidbody2D>().velocity.x ) * maxHorizSpeed, GetComponent<Rigidbody2D>().velocity.y );
			}
		}
		
		
		
		//Jumping
		if (playerInput.jump || Input.GetKey (KeyCode.Space)) {
			
			//jump Audio
			if(attributes.OnWall){
				//jumpAudio.Play ();
				soundManager.playJumpAudio();
			}
			
			
			jumpPressed = true;		
		}
		else{
			jumpPressed = false;
			jumpReleased = true;
		}
		
		
		if( jumpPressed && jumpReleased && (attributes.OnWall || attributes.HookTraveling ) )
		{
			attributes.Jumping = true;
			framesAccelerating = 0;
			jumpReleased = false;
			attributes.OnWall = false;
			
			attributes.anim.SetBool("Stunned", false);
			attributes.anim.SetBool("Up", false);
			attributes.anim.SetBool("Down", false);
			attributes.anim.SetBool("Idle", false);
			attributes.anim.SetBool("Hooked", false);
			attributes.anim.SetBool("Jumped", attributes.Jumping);
			if( attributes.HookTraveling )
			{
				theHook.SetActive( false );
				attributes.HookTraveling = false;
				attributes.HookLaunched = false;
				GetComponent<Rigidbody2D>().velocity = Vector2.zero;
			}
		}
		
		
		
		if (attributes.Jumping) {
			float scale = 1f - (float)(++framesAccelerating) / (float) max_jump_accel_frames;
			float powerScale = Mathf.Pow(scale, jumpDegrade);
			if( framesAccelerating <= max_jump_accel_frames )
			{
				Vector2 jump_vector;
				if( playerInput.leftJoystickX != 0f )
				{
					jump_vector = new Vector2( GetComponent<Rigidbody2D>().velocity.x, 1 );
				}
				else
				{
					jump_vector = new Vector2( 0f, 1f );
				}
				GetComponent<Rigidbody2D>().velocity += jump_vector.normalized * jumpAccel * powerScale;
			}
		}
		
	}
	//
	//	void OnTriggerEnter2D (Collider2D other){
	//		Debug.Log ("collided");
	//		if(other.tag == "WallLeft"){
	//			jump_vector = new Vector2(1,1);
	//			rigidbody2D.velocity = new Vector2(0,0);
	//			rigidbody2D.gravityScale = 0;
	//			Debug.Log ("Set grav to 0, gravity = " + rigidbody2D.gravityScale);
	//
	//		}
	//		if(other.tag == "WallRight"){
	//			jump_vector = new Vector2(-1,1);
	//			rigidbody2D.velocity = new Vector2(0,0);
	//			rigidbody2D.gravityScale = 0;
	//			Debug.Log ("Set grav to 0, gravity = " + rigidbody2D.gravityScale);
	//			
	//		}
	//	}
}
