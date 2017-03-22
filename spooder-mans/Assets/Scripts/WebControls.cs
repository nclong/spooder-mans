using UnityEngine;
using System.Collections;

public class WebControls : MonoBehaviour
{
	
	// PUBLIC VARIABLES
	public float travelSpeed;
	public float playerSpeed;
	//public float distance;
	//private Vector3 dir;
	
	//public AudioSource clankAudio;
	//public AudioSource hookAudio;
	
	//private SoundManager soundManager;
	//public SoundManager theSoundManager;

	// PRIVATE VARIABLES
	private LineRenderer lineRender;

	// INITIALIZE
	void Awake ()
	{
		lineRender = GetComponent<LineRenderer> ();
		//soundManager = (SoundManager)theSoundManager.GetComponent<SoundManager>();
	}
	
	// FIXED UPDATE
	void FixedUpdate ()
	{
		if ( enabled )
			lineRender.SetPosition( 1, transform.position );
		lineRender.SetPosition( 0, transform.parent.position );
	}

	// Launch: launches the web a certain distance in a certain direction
	public void Launch ( Vector2 dir, float distance, float webspeed, float playerSpeed)
	{
		gameObject.SetActive( true );
		GetComponent<Rigidbody2D>().velocity = dir * travelSpeed;
		//lineRender.SetPosition ( 0, transform.parent.position );
		//hookAudio.Play ();
		//soundManager.playGrappleAudio ();
	}
	
	//public void Launch( Vector3 dir, float distance, float hookSpeed, float playerSpeed )
	//{
		//this.hookSpeed = hookSpeed;
		//this.playerSpeed = playerSpeed;
		//this.dir = dir;
		
		//Debug.Log("Player Scale: " + transform.parent.localScale.x.ToString());
		//Debug.Log( "Hook Scale: " + transform.localScale.x.ToString() );
		//transform.eulerAngles = new Vector3( transform.eulerAngles.x, transform.eulerAngles.y, dir.Angle() );
		//WallAttributes wall = attributes.currentWall;
		//if( attributes.Jumping )
		//{
		//	transform.localPosition = new Vector2( this.dir.x, this.dir.y );
		//}
		//else
		//{
		//	transform.localPosition = new Vector2( this.dir.x * Mathf.Sign( transform.parent.localScale.x ), this.dir.y ); 
		//}
	//}
	
	public void OnTriggerEnter2D (Collider2D collision)
	{
		//Debug.Log( "Web hit: " + collision.gameObject.ToString() );
		//GameObject collisionObject = collision.gameObject;
		//WallAttributes wall = (WallAttributes)collisionObject.GetComponent<WallAttributes>();
		//CharacterAttributes character = (CharacterAttributes)collisionObject.GetComponent<CharacterAttributes>();
		//HookManager otherHook = collisionObject.GetComponent<HookManager>();
		//if( wall != null )
		//{
			
			//if( wall == attributes.currentWall)
			//{
				//transform.gameObject.SetActive(false);
			//}
			//else
			//{
				//transform.parent.rigidbody2D.velocity = Vector2.zero;
				//dir = (transform.position - transform.parent.position).normalized;
				//attributes.HookTraveling = true;
				//rigidbody2D.velocity = Vector2.zero;
				//transform.parent.rigidbody2D.velocity = dir * playerSpeed;
				
				//attributes.anim.SetBool("Stunned", false);
				//attributes.anim.SetBool("Up", false);
				//attributes.anim.SetBool("Down", false);
				//attributes.anim.SetBool("Idle", false);
				//attributes.anim.SetBool("Jumped", false);
				//attributes.anim.SetBool("Hooked", attributes.HookTraveling);            }
			
		//}
		
		//if( character != null && collisionObject != transform.parent.gameObject && attributes.OnWall && !character.newlySpawned )
		//{
			//if( character.SweepingActive )
			//{
				//attributes.HookLaunched = false;
				//transform.gameObject.SetActive( false );
			//}
			//else
			//{
				//rigidbody2D.velocity = dir * -hookSpeed;
				//character.Hooked = true;
				//collisionObject.rigidbody2D.velocity = dir * -playerSpeed; 
			//}
			
		//}
		//else if( character != null && collisionObject != transform.parent.gameObject && !attributes.OnWall && !character.newlySpawned )
		//{
			//if( character.SweepingActive )
			//{
				//attributes.HookLaunched = false;
				//transform.gameObject.SetActive( false );
			//}
			//else
			//{
				//transform.parent.rigidbody2D.velocity = dir * playerSpeed;
				//character.Hooked = true;
				//attributes.HookTraveling = true;
			//}
		//}
		//else if( character != null && collisionObject == transform.parent.gameObject )
		//{
		//	transform.gameObject.SetActive( false );
			//attributes.HookLaunched = false;
		//}
		
		//if hooks collide each other
		//if( otherHook != null )
		//{
			//CharacterAttributes otherCharacter = otherHook.transform.parent.gameObject.GetComponent<CharacterAttributes>();
			//attributes.HookLaunched = false;
			//transform.gameObject.SetActive( false );
			//otherHook.transform.gameObject.SetActive( false );
			//otherCharacter.HookLaunched = false;
			
			//clankAudio.Play ();
			//soundManager.playClankAudio();
		//}
	}
}
