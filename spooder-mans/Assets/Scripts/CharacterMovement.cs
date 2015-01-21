using UnityEngine;
using System.Collections;

public class CharacterMovement : MonoBehaviour {

	public float playerSpeed;
	public float verticalAccel;
    public float maxHorizSpeed;
    public float horizAccel;
	public int max_jump_accel_frames;
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
			rigidbody2D.velocity += new Vector2( 0f, playerInput.leftJoystickY) * verticalAccel;
			if( rigidbody2D.velocity.magnitude > playerSpeed )
			{
				rigidbody2D.velocity = rigidbody2D.velocity.normalized * playerSpeed;
			}
			if( playerInput.leftJoystickY.IsWithin(0f, 0.001f) && !attributes.Hooked && !attributes.HookTraveling)
			{
				rigidbody2D.velocity = Vector2.zero;
			}

            if (rigidbody2D.velocity.y < 0 )
            {
                attributes.anim.SetBool("Idle", false);
                attributes.anim.SetBool("Up", false);
                attributes.anim.SetBool("Stunned", false);
                attributes.anim.SetBool("Down", true);
            }
            else if (rigidbody2D.velocity.y > 0)
            {
                attributes.anim.SetBool("Idle", false);
                attributes.anim.SetBool("Down", false);
                attributes.anim.SetBool("Stunned", false);

                attributes.anim.SetBool("Up", true);
            }
            else if (rigidbody2D.velocity.y == 0)
            {
                attributes.anim.SetBool("Idle", true);
                attributes.anim.SetBool("Down", false);
                attributes.anim.SetBool("Stunned", false);
                attributes.anim.SetBool("Up", false);
            }

		}

        if( !attributes.OnWall && !attributes.HookTraveling && !attributes.Hooked && !attributes.Sweeping && !attributes.Swept )
        {
            rigidbody2D.velocity += new Vector2( playerInput.leftJoystickX , 0f );

            if( Mathf.Abs( rigidbody2D.velocity.x ) > maxHorizSpeed )
            {
                rigidbody2D.velocity = new Vector2( rigidbody2D.velocity.x / Mathf.Abs( rigidbody2D.velocity.x ) * maxHorizSpeed, rigidbody2D.velocity.y );
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
                rigidbody2D.velocity = Vector2.zero;
            }
		}



		if (attributes.Jumping) {
            if( jumpPressed )
            {
                ++framesAccelerating;
            }
            else
            {
                framesAccelerating = max_jump_accel_frames + 1;
            }
			float scale = 1f - (float)(framesAccelerating) / (float) max_jump_accel_frames;
			float powerScale = Mathf.Pow(scale, jumpDegrade);
			if( framesAccelerating <= max_jump_accel_frames )
			{
				Vector2 jump_vector;
                if( attributes.currentWall == null )
                {
                    if( playerInput.leftJoystickX != 0f )
                    {
                        jump_vector = new Vector2( playerInput.leftJoystickX / Mathf.Abs( playerInput.leftJoystickX ), 1 );
                    }
                    else
                    {
                        jump_vector = new Vector2( 0f, 1f );
                    } 
                }
                else if( attributes.currentWall.WhichWall == "Left" )
                {
                    jump_vector = right_wall_jump_vector;
                    jump_vector = left_wall_jump_vector;
                }
                else
                {
                    jump_vector = right_wall_jump_vector;
                }

				rigidbody2D.velocity += jump_vector * jumpAccel * powerScale;
			}
		}

	}
}
