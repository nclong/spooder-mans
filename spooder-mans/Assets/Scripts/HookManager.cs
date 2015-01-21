using UnityEngine;
using System.Collections;

public class HookManager : MonoBehaviour {

	private LineRenderer line;
	private float hookSpeed;
	private float playerSpeed;
	private Vector3 dir;

	public CharacterAttributes attributes;

	public AudioSource clankAudio;
	//public AudioSource hookAudio;

	private SoundManager soundManager;
	public SoundManager theSoundManager;


	// Use this for initialization
	void Start () {
		//line = (LineRenderer)renderer;
		soundManager = (SoundManager)theSoundManager.GetComponent<SoundManager>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if( Mathf.Abs( transform.position.x ) > 15)
        {
            attributes.HookLaunched = false;
            transform.gameObject.SetActive( false );
        }
	}

	public void Launch( Vector3 dir, float distance, float hookSpeed, float playerSpeed )
	{
		this.hookSpeed = hookSpeed;
		this.playerSpeed = playerSpeed;
        this.dir = dir;
        
        Debug.Log("Player Scale: " + transform.parent.localScale.x.ToString());
        Debug.Log( "Hook Scale: " + transform.localScale.x.ToString() );
		transform.eulerAngles = new Vector3( transform.eulerAngles.x, transform.eulerAngles.y, dir.Angle() );
        WallAttributes wall = attributes.currentWall;
        //if( attributes.Jumping )
        //{
        //    transform.localPosition = new Vector2( this.dir.x, this.dir.y );
        //}
        //else
        //{
        //    transform.localPosition = new Vector2( this.dir.x * Mathf.Sign( transform.parent.localScale.x ), this.dir.y ); 
        //}
        transform.localPosition = new Vector2( this.dir.x * Mathf.Sign( transform.parent.localScale.x ), this.dir.y ); 
        Debug.Log( "Local Position: " + transform.localPosition );
		transform.gameObject.SetActive(true);
		rigidbody2D.velocity = this.dir.In2D() * this.hookSpeed;
        //line.SetPosition(0, transform.parent.position );


		//hookAudio.Play ();
		soundManager.playGrappleAudio ();
	}

	public void OnTriggerEnter2D(Collider2D collision)
	{
        Debug.Log( "Web hit: " + collision.gameObject.ToString() );
		GameObject collisionObject = collision.gameObject;
		WallAttributes wall = (WallAttributes)collisionObject.GetComponent<WallAttributes>();
		CharacterAttributes character = (CharacterAttributes)collisionObject.GetComponent<CharacterAttributes>();
        HookManager otherHook = collisionObject.GetComponent<HookManager>();
		if( wall != null )
		{

			if( wall == attributes.currentWall)
			{
				transform.gameObject.SetActive(false);
                attributes.HookLaunched = false;
			}
			else
			{
				transform.parent.rigidbody2D.velocity = Vector2.zero;
				dir = (transform.position - transform.parent.position).normalized;
				attributes.HookTraveling = true;
				rigidbody2D.velocity = Vector2.zero;
				transform.parent.rigidbody2D.velocity = dir * playerSpeed;

                attributes.anim.SetBool("Stunned", false);
                attributes.anim.SetBool("Up", false);
                attributes.anim.SetBool("Down", false);
                attributes.anim.SetBool("Idle", false);
                attributes.anim.SetBool("Jumped", false);
                attributes.anim.SetBool("Hooked", attributes.HookTraveling);            }

		}

        //Hook its a character while hook launcher is on wall and target is not newly spawned
		if( character != null && collisionObject != transform.parent.gameObject && attributes.OnWall && !character.newlySpawned )
		{
            character.OnWall = false;
            //Destroys Hook if hook target is sweeping
            if( character.SweepingActive )
            {
                attributes.HookLaunched = false;
                transform.gameObject.SetActive( false );
            }
            else
            {
                rigidbody2D.velocity = dir * -hookSpeed;
                character.Hooked = true;
                character.HookTraveling = false;
                character.Jumping = false;
                character.Sweeping = false;
                character.Swept = false;
                collisionObject.rigidbody2D.velocity = dir * -playerSpeed; 
            }

		}
        else if( character != null && collisionObject != transform.parent.gameObject && !attributes.OnWall && !character.newlySpawned )
        {
            if( character.SweepingActive )
            {
                attributes.HookLaunched = false;
                transform.gameObject.SetActive( false );
            }
            else
            {
                transform.parent.rigidbody2D.velocity = dir * playerSpeed;
                character.Hooked = true;
                character.HookTraveling = false;
                character.Jumping = false;
                character.Sweeping = false;
                character.Swept = false;
                attributes.HookTraveling = true;
            }
        }
        else if( character != null && collisionObject == transform.parent.gameObject )
        {
            transform.gameObject.SetActive( false );
            attributes.HookLaunched = false;
        }

		//if hooks collide each other
        if( otherHook != null )
        {
            CharacterAttributes otherCharacter = otherHook.transform.parent.gameObject.GetComponent<CharacterAttributes>();
            attributes.HookLaunched = false;
            transform.gameObject.SetActive( false );
            otherHook.transform.gameObject.SetActive( false );
            otherCharacter.HookLaunched = false;

			//clankAudio.Play ();
			soundManager.playClankAudio();
        }
	}

}
