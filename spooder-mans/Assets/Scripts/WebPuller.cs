using UnityEngine;
using System.Collections;

public class WebPuller : MonoBehaviour
{

	private float webSpeed;
	private float playerSpeed;
	private CharacterControls controls;
	private Rigidbody2D player;
	private Rigidbody2D target;
	private bool hitWall;
	private AnimationManager playerAniman;
	private float webRange;
	private Vector2 targetPosition;
	private bool rebound;
	
	// FIXED UPDATE
	void FixedUpdate ()
	{
		if ( playerAniman.IsHooking() )
		{
			DoLaunch ();
			DoPull ();
		}
		else
		{
			EndHook ();
		}
	}

	// StartPull: tell the attached gameObject to begin a pull command
	public void StartPull ( Vector2 targetPosition, Rigidbody2D player, AnimationManager animan, float webSpeed, float playerSpeed, CharacterControls controls )
	{
		GetComponent<Rigidbody2D>().position = player.position;
		this.targetPosition = targetPosition;
		this.webSpeed = webSpeed;
		this.playerSpeed = playerSpeed;
		this.player = player;
		this.playerAniman = animan;
		this.controls = controls;
		this.hitWall = false;
		animan.SetHooking ();
		rebound = false;
	}

	// DoLaunch: launching phase.
	void DoLaunch ()
	{
		// if player is hooking and isn't getting pulled, do launch phase
		if ( !rebound && playerAniman.IsHooking () )
		{
			if ( !playerAniman.IsGettingPulled () && GetComponent<Rigidbody2D>().position != targetPosition )
				transform.position = Vector2.MoveTowards ( GetComponent<Rigidbody2D>().position, targetPosition, webSpeed * Time.fixedDeltaTime );
			else
				rebound = true;
		}
	}

	void DoPull ()
	{
		// if player is hooking and is getting pulled, do pull sequence
		if ( rebound && playerAniman.IsHooking () )
		{
			if ( hitWall )				// hit a wall
			{
				player.velocity = Vector2.zero;
				player.position = Vector2.MoveTowards ( player.position, targetPosition, playerSpeed * Time.fixedDeltaTime );
				playerAniman.ClearJumpCounter ();
			}
			else
			{
				if ( target == null )	// player didn't hook another player, bring hook back
				{
					transform.position = Vector2.MoveTowards ( transform.position, player.position, webSpeed * Time.fixedDeltaTime );
				}
				else					// player hooked another player
				{
					playerAniman.ClearJumpCounter ();
					transform.position = Vector2.MoveTowards ( transform.position, player.position, playerSpeed * Time.fixedDeltaTime );
					target.position = Vector2.MoveTowards ( target.position, player.position, playerSpeed * Time.fixedDeltaTime );
				}
			}
		}
	}

	void OnTriggerEnter2D ( Collider2D collision )
	{
		if ( playerAniman.IsHooking() )
		{
			AnimationManager targetAniman = collision.gameObject.GetComponent<AnimationManager> ();
			WallProperties wallprop = collision.gameObject.GetComponent<WallProperties> ();

			if ( targetAniman != null )
			{
				// Hit a player with web
				if ( targetAniman != playerAniman && !rebound && !targetAniman.IsAttacking () )
				{
					targetAniman.SetStunned( controls.WEB_STUN_FRAMES );
					target = targetAniman.gameObject.GetComponent<Rigidbody2D>();
					rebound = true;	// flag getting pulled
				}
				// Hit self with web
				else if ( rebound )
				{
					EndHook ();
					//player.position = targetPosition;
				}
			}
			// Hit a wall with web
			else if ( wallprop != null )
			{
				if ( playerAniman.IsMidair() || controls.GetDirection() != wallprop.directionFacing )
				{
					controls.SetDirection ( wallprop.directionFacing * -1 );	// set player to face the wall getting hooked to
					hitWall = true;
					rebound = true;
					playerAniman.SetGettingPulled ();							// flag getting pulled
				}
			}
			// If hit anything else, cancel web
			else if ( collision.GetComponent<Collider2D>() != null )
			{
				rebound = true;
			}
		}
	}

	void EndHook ()
	{
		playerAniman.ClearAllHooking();
		target = null;
		gameObject.SetActive ( false );
	}
}
