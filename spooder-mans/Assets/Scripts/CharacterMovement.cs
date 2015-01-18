using UnityEngine;
using System.Collections;

public class CharacterMovement : MonoBehaviour {

	public float playerSpeed;
	public float verticalAccel;
	public float max_jump_accel_frames;
	public float jumpAccel;
	public float jumpDegrade;
	public Vector2 left_wall_jump_vector;
	public Vector2 right_wall_jump_vector;

	private bool jumpPressed;
	private bool jumpReleased = true;
	private CharacterAttributes attributes;
	private int framesAccelerating = 0;
	private int playerNum;
	private PlayerInput playerInput;

	public AudioSource jumpAudio;	
	public AudioSource grappleAudio;

	// Use this for initialization
	void Start () {
		attributes = GetComponent<CharacterAttributes> ();
		playerNum = attributes.playerNum;
		playerInput = InputManager.PlayerInputs [playerNum];
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		//Moving
		if (attributes.OnWall && !attributes.Sweeping) {
			rigidbody2D.velocity += new Vector2( 0f, playerInput.leftJoystickY) * verticalAccel;
			if( rigidbody2D.velocity.magnitude > playerSpeed )
			{
				rigidbody2D.velocity = rigidbody2D.velocity.normalized * playerSpeed;
			}
			if( playerInput.leftJoystickY.IsWithin(0f, 0.01f) && !attributes.Hooked && !attributes.HookTraveling)
			{
				rigidbody2D.velocity = Vector2.zero;
			}
		}



		//Jumping
		if (playerInput.jump || Input.GetKey (KeyCode.Space)) {

			//jump Audio
			if(attributes.OnWall){
				jumpAudio.Play ();
			}


			jumpPressed = true;		
		}
		else{
			jumpPressed = false;
			jumpReleased = true;
		}


		if( jumpPressed && jumpReleased && attributes.OnWall )
		{
			attributes.Jumping = true;
			framesAccelerating = 0;
			jumpReleased = false;
			attributes.OnWall = false;
		}



		if (attributes.Jumping) {
			float scale = 1f - (float)(++framesAccelerating) / (float) max_jump_accel_frames;
			float powerScale = Mathf.Pow(scale, jumpDegrade);
			if( framesAccelerating <= max_jump_accel_frames )
			{
				Vector2 jump_vector;
				jump_vector = transform.position.x > 0 ? right_wall_jump_vector : left_wall_jump_vector;
				rigidbody2D.velocity += jump_vector.normalized * jumpAccel * powerScale;
			}
		}


		//hook audio
		if(attributes.HookTraveling){
			grappleAudio.Play ();
		}
		else{
			grappleAudio.Stop ();
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
