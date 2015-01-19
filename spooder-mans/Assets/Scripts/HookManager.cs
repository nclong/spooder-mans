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
	void Update () {
		if( enabled )
		{

			//line.SetPosition(1, transform.position);
		}
		//line.SetPosition(0, transform.parent.position);
	}

	public void Launch( Vector3 dir, float distance, float hookSpeed, float playerSpeed )
	{
		this.hookSpeed = hookSpeed;
		this.playerSpeed = playerSpeed;
        this.dir = dir;

		transform.eulerAngles = new Vector3( transform.eulerAngles.x, transform.eulerAngles.y, dir.Angle() );
        transform.localPosition = new Vector2(this.dir.x * Mathf.Sign(transform.parent.localScale.x), this.dir.y);
        Debug.DrawRay(transform.position, dir, Color.red, 2f);
		transform.gameObject.SetActive(true);
		rigidbody2D.velocity = this.dir.In2D() * this.hookSpeed;
		//line.SetPosition(0, transform.parent.position );


		//hookAudio.Play ();
		soundManager.playGrappleAudio ();
	}

	public void OnTriggerEnter2D(Collider2D collision)
	{
		GameObject collisionObject = collision.gameObject;
		WallAttributes wall = (WallAttributes)collisionObject.GetComponent<WallAttributes>();
		CharacterAttributes character = (CharacterAttributes)collisionObject.GetComponent<CharacterAttributes>();
        HookManager otherHook = collisionObject.GetComponent<HookManager>();
		if( wall != null )
		{

			if( wall == attributes.currentWall)
			{
				transform.gameObject.SetActive(false);
			}
			else
			{
				transform.parent.rigidbody2D.velocity = Vector2.zero;
				dir = (transform.position - transform.parent.position).normalized;
				attributes.HookTraveling = true;
				rigidbody2D.velocity = Vector2.zero;
				transform.parent.rigidbody2D.velocity = dir * playerSpeed;

                attributes.anim.SetBool("Idle", false);
                attributes.anim.SetBool("Jumped", false);
                attributes.anim.SetBool("Hooked", attributes.HookTraveling);
            }

		}

		if( character != null && collisionObject != transform.parent.gameObject && attributes.OnWall )
		{
			rigidbody2D.velocity = dir * -hookSpeed;
			character.Hooked = true;
			collisionObject.rigidbody2D.velocity = dir * -playerSpeed;

		}
        else if( character != null && collisionObject != transform.parent.gameObject && !attributes.OnWall )
        {
            transform.parent.rigidbody2D.velocity = dir * playerSpeed;
            character.Hooked = true;
            attributes.HookTraveling = true;
        }
        else if( character != null && collisionObject == transform.parent.gameObject )
        {
            attributes.HookLaunched = false;
            transform.gameObject.SetActive( false );
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
