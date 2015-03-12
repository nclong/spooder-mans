using UnityEngine;
using System.Collections;

public class CharacterControls : MonoBehaviour
{
	// PUBLIC VARIABLES
	public int PLAYER_INDEX = 0;			// player index. MUST BE 0-3
	public int PLAYER_SCALE = 5;			// player's size scale
	public int MIN_JUMP_FRAMES = 4;			// min number of frames to apply jump acceleration
	public int MID_JUMP_FRAMES = 8;			// mid number of frames to apply jump acceleration
	public int MAX_JUMP_FRAMES = 16;		// max number of frames to apply jump acceleration
	public int MAX_JUMPS = 2;				// max number jumps allowed
	public int WEB_STUN_FRAMES = 20;
	
	public float fallAccel = 120f;			// how fast does character fall
	public float dropAccel = 200f;
	public float maxFallSpeed = 20f;		// max falling speed
	public float maxDropSpeed = 30f;
	public float jumpAccel = 800f;			// how quickly does a character jump
	public float maxJumpSpeed = 26f;		// max speed of jump
	public float bumpSpeed = 20f;
	public float jumpScaling = 0.9f;		// how does jump curve behave
	public float runSpeed = 14f;			// how fast does it run
	public float maxAirMoveSpeed = 12f;		// max movement speed in the air
	public float midairAcceleration = 50f;
	public float midairAgility = 200f;		// for midair active maneuvers
	public float wallDrag = 70f;			// affects player slide distance
	public float aerialBounceSpeed = 50f;	// aerial attack bouce speed
	public float aerialDiveSpeed = 40f;		// aerial dive speed
	public float resistanceOnWall = 1f;
	public float resistanceInAir = 1f;
	public float wallDashSpeed = 20f;		// dashing speed when attacking on the wall
	public float wallJumpForce = 0.5f;
	public Vector2 SpawnVector;
	public float SpawnVariance;

	public SoundManager sound;
	public GameStateManager gameStateManager;
	public GameObject Spawner;
	
	// PRIVATE VARIABLES
	private AnimationManager animan;		// animation state manager
	private int jumpTimer;					// jump timer
	private int jumpMaxTimer;
	private int directionFacing;			// which direction the player's facing, switched by the walls the player lands on
	private int framesSpawned;
	private bool respawning;
	public int framesNewlySpawned;
	public int framesBeforeRespawn;
	private int framesSinceDeath;
	private bool newlySpawned;
	private int lagFrames;
	private int lagTimer;
	private Vector2 storeVelocity;
	private Animator animator;
	private float tempFallSpeed;
	
	// INITIALIZE
	void Awake()
	{
		animan = GetComponent<AnimationManager> ();
		animator = GetComponent <Animator> ();
		jumpTimer = MAX_JUMP_FRAMES;
		jumpMaxTimer = MAX_JUMP_FRAMES;
		animan.SetSpawnStatuses ();
		directionFacing = 1;
		framesSpawned = 0;
		framesNewlySpawned = 0;
		framesSinceDeath = 0;
		respawning = false;
		newlySpawned = false;
		lagFrames = -1;
		lagTimer = -1;
		storeVelocity = Vector2.zero;
		tempFallSpeed = maxDropSpeed;
	}
	
	// FIXED UPDATE
	void FixedUpdate()
	{
		if ( lagTimer < lagFrames )
		{
			if ( lagTimer == lagFrames - 1 )
			{
				animator.speed = 1;
				rigidbody2D.velocity = storeVelocity;
				lagTimer = -1;
				lagFrames = -1;
				animan.ClearLagged ();
			}
			else 
			{
				lagTimer++;
				rigidbody2D.velocity = Vector2.zero;
				animator.speed = 0;
			}
		}
		else
		{
			transform.localScale = new Vector3 ( directionFacing * PLAYER_SCALE, PLAYER_SCALE, 1f );
			animan.ClearTriggers ();
			DoAttack ();
			DoMove ();
			DoJump ();
			DoFall ();
			DoKill ();
			
			// Increment jump timer
			jumpTimer = ( jumpTimer == jumpMaxTimer )? jumpMaxTimer : jumpTimer + 1;
		}
	}

	// COLLISIONS
	void OnCollisionEnter2D ( Collision2D collision )
	{
		if ( !animan.IsStunned () && !animan.IsLagged () )
		{
			string theTag = collision.gameObject.tag;
			if ( theTag == TagManager.wall )
			{
				jumpTimer = jumpMaxTimer;		// cancel jump
				animan.SetWallStatuses ();

				// set player to face the wall's direction
				WallProperties wall = collision.gameObject.GetComponent<WallProperties>();
				directionFacing = wall.directionFacing;

				// convert momentum to sliding
				rigidbody2D.velocity = new Vector2 ( 0f, rigidbody2D.velocity.x );	//This line creates less landing velocity
			}
			// If hitting a player, push them in opposite direction
			else if ( theTag == TagManager.player && !animan.IsAttacking () )
			{
				//Physics2D.IgnoreCollision ( this.collider2D, collision.collider );
				if ( rigidbody2D.position.y > collision.transform.position.y )
				{
					rigidbody2D.velocity = new Vector2 ( rigidbody2D.velocity.x, bumpSpeed );
					animan.ClearJumpCounter ();
					sound.playBounceAudio ();
				}
				else
					rigidbody2D.velocity = new Vector2 ( rigidbody2D.velocity.x, -1 * bumpSpeed );
			}
		}
	}

	void OnCollisionExit2D ( Collision2D collision )
	{
		string theTag = collision.gameObject.tag;
		if ( theTag == TagManager.wall )
		{
			animan.SetMidair ();
			rigidbody2D.velocity = new Vector2( runSpeed * directionFacing * wallJumpForce, rigidbody2D.velocity.y );
		}
	}
	
	// FLAG GETTERS AND SETTERS
	bool CanStartJump () { return !newlySpawned && !respawning && !animan.IsAttacking() && animan.IsJumpLifted() && animan.GetJumpCounter () < MAX_JUMPS && !animan.IsStunned (); }
	bool CanFall () { return !newlySpawned && !respawning && !animan.IsGettingPulled () && ( animan.IsMidair () || animan.IsStunned () ); }
	bool CanAttack () { return !newlySpawned && !respawning && animan.IsAttackLifted () && !animan.IsCharging () && !animan.IsAttacking () && !animan.IsStunned (); }
	bool CanMove () { return !newlySpawned && !respawning && !animan.IsGettingPulled () && !animan.IsAttacking () && !animan.IsStunned () && !animan.IsCharging (); }

	public float GetDirection() { return directionFacing; }
	public void SetDirection ( int dir ) { directionFacing = dir; }

	// DoMove: either initiate a move or support the continuation/interruption of one
	void DoMove ()
	{
		// input vector with joystick values
		Vector2 move = new Vector2 ( InputManagerLazy.GetInput( PLAYER_INDEX ).leftJoystickX, InputManagerLazy.GetInput( PLAYER_INDEX ).leftJoystickY );

		if ( animan.IsMidair () )		// if the player is midair and allowed to move, player can nudge the avatar around
		{
			if ( move.magnitude > 0f && CanMove () )
				AirNudge( move );
		}
		else 					// ... otherwise ...
		{
			if ( move.magnitude > 0f && CanMove () )
				RunOnWall ( move.y );		// player is on the wall and moving. allow player to run
			else
				DragOnWall ();				// player is on the wall and not moving. apply deceleration
		}
	}
	void AirNudge( Vector2 nudgeVec )		// nudge avatar towards input direction
	{
		rigidbody2D.velocity = Vector2.MoveTowards ( rigidbody2D.velocity, new Vector2 ( nudgeVec.normalized.x * maxAirMoveSpeed, rigidbody2D.velocity.y ), midairAcceleration * Time.deltaTime );
	}
	void RunOnWall( float direction )
	{
		if ( direction > 0f )				// run up. modify revelant states
		{
			animan.SetRunningUp ();
			animan.ClearRunningDown ();
			rigidbody2D.velocity = new Vector2( rigidbody2D.velocity.x, runSpeed );
		}
		else if ( direction < 0f )			// run down. modify revelant states
		{
			animan.SetRunningDown ();
			animan.ClearRunningUp ();
			rigidbody2D.velocity = new Vector2( rigidbody2D.velocity.x, -1 * runSpeed );
		}
	}
	void DragOnWall()						// no movements input, modify relevant states
	{
		animan.ClearRunningUp ();
		animan.ClearRunningDown ();
		rigidbody2D.velocity = Vector2.MoveTowards ( rigidbody2D.velocity, Vector2.zero, wallDrag * Time.deltaTime );
	}
	
	// DoJump: either initiate a jump or support the continuation/interruption of one
	void DoJump ()
	{
		// Player has jumped?
		if ( InputManagerLazy.GetInput( PLAYER_INDEX ).jump )
		{
			// Player started jump?
			if ( jumpTimer == jumpMaxTimer )
			{
				if ( CanStartJump () )			// check states to see if allowed to jump
				{
					sound.playJumpAudio ();
					animan.JumpAndSetJumpStatuses();
					animan.IncrementJumpCounter ();
					animan.ClearJumpLifted ();

					jumpTimer = 0;								// Reset jump timer
					jumpMaxTimer = MIN_JUMP_FRAMES;				// Set max jump frames to min

					if ( Mathf.Sign( rigidbody2D.velocity.y ) == -1f )	// if falling, set fall speed to zero for more responsive jump
						rigidbody2D.velocity = new Vector2 ( rigidbody2D.velocity.x, 0f );
				}
			}
			// if just about to hit jump frame limit, increase limits accordingly
			if ( jumpTimer == jumpMaxTimer - 1 )
			{
				if ( jumpMaxTimer == MIN_JUMP_FRAMES )
					jumpMaxTimer = MID_JUMP_FRAMES;
				else if ( jumpMaxTimer == MID_JUMP_FRAMES )
					jumpMaxTimer = MAX_JUMP_FRAMES;
			}
		} else {		// player stopped inputting jump command, restore values
			animan.SetJumpLifted ();
		}
		
		// if a jump is in progress, apply acceleration
		if ( jumpTimer < jumpMaxTimer )
		{
			float scale = 1f - ( (float) jumpTimer / (float) MAX_JUMP_FRAMES );		// scaling jump acceleration over the maximum POSSIBLE jump
			float scaledMore = Mathf.Pow ( scale, jumpScaling );
			Vector2 jumpVector = new Vector2 ( rigidbody2D.velocity.x, maxJumpSpeed );

			if ( !animan.IsMidair () )			// if jumping from wall, apply a burst of speed in the horizontal direction
				jumpVector.x = runSpeed * directionFacing;
			else if ( InputManagerLazy.GetInput( PLAYER_INDEX ).leftJoystickX != 0f )
			{	
				// if midair, give some more control in the horizontal direction. this is in addition to the move function's speed boost
				float xBurst = InputManagerLazy.GetInput( PLAYER_INDEX ).leftJoystickX * maxAirMoveSpeed;
				rigidbody2D.velocity = Vector2.MoveTowards ( rigidbody2D.velocity, new Vector2 ( xBurst, rigidbody2D.velocity.y ), midairAgility * Time.fixedDeltaTime );
			}

			// apply jump acceleration
			rigidbody2D.velocity = Vector2.MoveTowards ( rigidbody2D.velocity, jumpVector, jumpAccel * scaledMore * Time.fixedDeltaTime );
		}
	}

	// DoFall: if able to fall, apply falling acceleration
	void DoFall ()
	{
		if ( CanFall () )
		{
			float fallSpeed = maxDropSpeed;
			float tempFallAccel = dropAccel;
			if ( InputManagerLazy.GetInput( PLAYER_INDEX ).jump || animan.IsHooking () )
			{
				fallSpeed = maxFallSpeed;
				tempFallAccel = fallAccel;
			}
			rigidbody2D.velocity = Vector2.MoveTowards( rigidbody2D.velocity, new Vector2 ( rigidbody2D.velocity.x, fallSpeed * -1 ), tempFallAccel * Time.fixedDeltaTime );
		}
	}

	// DoAttack: if told to attack and able to attack, set trigger
	void DoAttack()
	{
		if ( InputManagerLazy.GetInput( PLAYER_INDEX ).sweep )
		{
			if ( CanAttack() )
				animan.SetAttackStarted ();
			animan.ClearAttackLifted ();
		} else
			animan.SetAttackLifted ();
		if ( animan.IsHitStarted () )
		{
			sound.playSweepAudio ( PLAYER_INDEX );
			if ( animan.IsMidair () )
				DoAerialBounce ();
			else
				DoWallDash ();
			animan.ClearHitStarted ();
		}
	}

	// DoAerialBounce: this allows players to perform an aerial
	public void DoAerialBounce()
	{
		//Debug.Log ("asdasdas");
		Vector2 inputs = new Vector2 ( InputManagerLazy.GetInput( PLAYER_INDEX ).leftJoystickX, InputManagerLazy.GetInput( PLAYER_INDEX ).leftJoystickY );
		float aerialSpeed = aerialBounceSpeed;

		if ( inputs.y < 0f )
			aerialSpeed = -1f * aerialDiveSpeed;

		rigidbody2D.velocity = new Vector2 ( inputs.x * maxAirMoveSpeed, aerialSpeed );			// Apply the aerial maneuver
	}

	// DoAerialBounce: this allows players to perform a wall dash
	public void DoWallDash ()
	{
		rigidbody2D.velocity = new Vector2 ( rigidbody2D.velocity.x, wallDashSpeed * InputManagerLazy.GetInput( PLAYER_INDEX ).leftJoystickY );
	}

	// GetHit: apply an acceleration scaled down based on character's resistance
	public void GetHit ( Vector2 direction, float power )
	{
		if ( !newlySpawned )
		{
			rigidbody2D.velocity = Vector2.zero;
			float resistance = resistanceOnWall;
			Vector2 newDir = direction;
			if ( animan.IsMidair() )
				resistance = resistanceInAir;
			else
				newDir = new Vector2 ( directionFacing, direction.y ).normalized;
			rigidbody2D.velocity = newDir * power / resistance;
			animan.SetStunned ( ( int ) power );
			SetLag ( ( int ) ( power / 3f ) );
		}
	}

	// SetLag: Pause animations and defer velocity applications until end of lag period.
	public void SetLag ( int frames )
	{
		lagFrames = frames;
		if ( lagFrames < 1 )
			lagFrames = 1;
		lagTimer = 0;
		animan.SetLagged ();
		Debug.Log ( rigidbody2D.velocity );
		storeVelocity = rigidbody2D.velocity;
	}

	public bool IsLagged ()
	{
		return animan.IsLagged ();
	}


	void DoKill()
	{
		if( transform.position.y <= -25 || transform.position.y >= 25 )
		{
			animan.SetSpawnStatuses ();
			KillPlayer();
		}
		else if( newlySpawned )
		{
			Debug.Log ("asdasd");
			++framesSpawned;
			if( framesSpawned > framesNewlySpawned + framesBeforeRespawn)
			{
				newlySpawned = false;
				framesSpawned = 0;
			}
		}
		else if( respawning )
		{
			++framesSinceDeath;
			if( framesSinceDeath >= framesBeforeRespawn )
			{
				renderer.enabled = true;
				rigidbody2D.velocity = new Vector2( SpawnVector.x + Random.Range( -SpawnVariance, SpawnVariance ), SpawnVector.y/* + Random.Range( -SpawnVariance, SpawnVariance ) */);
				framesSinceDeath = 0;
				newlySpawned = true;
				respawning = false;
			}
		}
	}

	public void KillPlayer()
	{
		foreach( Transform child in transform )
			child.gameObject.SetActive( false );
		framesSpawned = 0;
		sound.playDeathAudio ( PLAYER_INDEX );
		gameStateManager.LosePlayerLife( PLAYER_INDEX );
		
		if( gameStateManager.playerStock[ PLAYER_INDEX ] > 0 )
		{
			respawning = true;
			transform.position = Spawner.transform.position;
			renderer.enabled = false;
			rigidbody2D.velocity = Vector2.zero;
		}
		else
		{
			transform.gameObject.SetActive ( false );
		}
	}
}
