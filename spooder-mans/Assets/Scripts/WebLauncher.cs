using UnityEngine;
using System.Collections;

public class WebLauncher : MonoBehaviour {
	
	public float cursorDistance = 3f;
	public Vector3 cursorScale = new Vector3 ( 5f, 5f, 1f );
	public Vector3 markerScale = new Vector3 ( 3f, 3f, 1f );
	public Vector3 webScale = new Vector3 ( 10f, 10f, 1f );
	public Vector3 webColliderSize = new Vector3 ( 0.08f, 0.16f);
	public GameObject cursorTemplate;
	public GameObject webTemplate;
	public Sprite[] cursorSprites;
	public Sprite[] webSprites;
	public Material[] webMaterials;
	public LayerMask layer;
	public float webRange = 30f;
	public float webSpeed = 60f;
	public float playerSpeed = 30f;
	public SoundManager sound;

	private CharacterControls controls;
	private bool hookButtonReleased;
	private AnimationManager animan;
	private GameObject cursor;
	private GameObject web;
	private LineRenderer lineRender;

	private Vector2 dir;

	// INITIALIZE
	void Awake ()
	{
		// get relevant components
		animan = GetComponent<AnimationManager> ();
		controls = GetComponent<CharacterControls> ();
		hookButtonReleased = true;

		// create a cursor
		cursor = ( GameObject ) Instantiate ( cursorTemplate );
		cursor.SetActive ( true );
		SpriteRenderer cursorSprite = cursor.GetComponent<SpriteRenderer> ();
		cursorSprite.sprite = cursorSprites[ controls.PLAYER_INDEX ];
		cursor.transform.localScale = cursorScale;
		cursor.SetActive ( false );

		// create a web
		web = ( GameObject ) Instantiate ( webTemplate );
		web.SetActive ( true );
		SpriteRenderer webSprite = web.GetComponent<SpriteRenderer> ();
		BoxCollider2D webCollider = web.GetComponent<BoxCollider2D> ();
		webCollider.size = webColliderSize;
		webSprite.sprite = webSprites[ controls.PLAYER_INDEX ];
		web.transform.localScale = webScale;
		lineRender = web.GetComponent<LineRenderer> ();
		lineRender.material = webMaterials[ controls.PLAYER_INDEX ];
		web.SetActive ( false );
	}

	// FIXED UPDATE
	void FixedUpdate ()
	{
		cursor.transform.localScale = new Vector3( cursor.transform.localScale.x , cursor.transform.localScale.y, cursor.transform.localScale.z );
		DoUpdateCursor();
		DoHook ();
		DoDrawLine ();
	}

	// DoUpdateCursor: move the cursor and orient it properly. Must multiply x axis by player facing direction, but only for visual reasons
	private void DoUpdateCursor()
	{

		Vector2 joystickVect = new Vector2 ( InputManagerLazy.GetInput( controls.PLAYER_INDEX ).rightJoystickX, InputManagerLazy.GetInput( controls.PLAYER_INDEX ).rightJoystickY );
		if ( joystickVect.magnitude < 0.3 )
			cursor.SetActive ( false );
		else
		{
			cursor.SetActive ( true );
			Vector2 offset = joystickVect.normalized * cursorDistance;
			cursor.transform.position = transform.position + new Vector3 ( offset.x, offset.y, 0f );
			cursor.transform.eulerAngles = new Vector3( cursor.transform.eulerAngles.x, cursor.transform.eulerAngles.y, joystickVect.Angle() );
		}
	}

	// CanHook: can shoot a web when player is not already using it, not attacking, and not stunned
	private bool CanHook()
	{
		return cursor.activeInHierarchy && !animan.IsHooking() && !animan.IsAttacking() && !animan.IsStunned();
	}

	// DoHook: start a hook or continue one
	private void DoHook()
	{
		// Get web direction
		Vector2 joystickVect = new Vector2 ( InputManagerLazy.GetInput( controls.PLAYER_INDEX ).rightJoystickX, InputManagerLazy.GetInput( controls.PLAYER_INDEX ).rightJoystickY ).normalized;

		// If hook command is received ...
		if ( InputManagerLazy.GetInput( controls.PLAYER_INDEX ).hook )
		{
			if ( hookButtonReleased && CanHook() )		// only execute if player is allowed to hook and the button has been released from before
			{
				// get direction to launch
				dir = new Vector2 ( GetComponent<Rigidbody2D>().position.x, GetComponent<Rigidbody2D>().position.y ) + ( joystickVect * webRange );

				// launch the web
				sound.playGrappleAudio ();
				web.SetActive ( true );
				animan.SetHooking ();
				web.GetComponent<WebPuller> ().StartPull ( dir, GetComponent<Rigidbody2D>(), animan, webSpeed, playerSpeed, controls );
			}
			hookButtonReleased = false;		// player has pressed the button
		}
		// Otherwise, indicate the button is released
		else
			hookButtonReleased = true;
	}

	// DoDrawLine: draw the line to the web
	private void DoDrawLine()
	{
		if ( animan.IsHooking() )
		{
			lineRender.enabled = true;
			lineRender.SetPosition( 0, transform.position );
			lineRender.SetPosition ( 1, web.transform.position );
		}
		else
		{
			lineRender.enabled = false;
		}
	}
}
