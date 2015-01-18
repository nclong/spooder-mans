using UnityEngine;
using System.Collections;

public class HookLauncher : MonoBehaviour {

	public GameObject theHook;
	public GameObject hookCursor;
	public float cursorDistance;
	public float hookLaunchSpeed;
	public float playerSpeed;
	public float maxChainDistance;
	
	private bool inputReceived;
	private bool inputReleased = true;
	private HookManager hookManager;
	private CharacterAttributes attributes;

	private Vector2 mousePosition;
	// Use this for initialization
	void Start () {
		theHook.SetActive(true);
		hookManager = (HookManager)theHook.GetComponent<HookManager>();
		theHook.SetActive(false);
		attributes = GetComponent<CharacterAttributes> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Vector3 mouseInWorld3D = Camera.main.ScreenToWorldPoint( Input.mousePosition );
		mousePosition = mouseInWorld3D.In2D();

		hookCursor.transform.localPosition = (mouseInWorld3D - transform.position).normalized * cursorDistance;

		if( Input.GetButton ("Hook1" ))
		{
			inputReceived = true;
		}
		else
		{
			inputReleased = true;
			inputReceived = false;
		}

		if( inputReceived && inputReleased && !attributes.Hooked && !attributes.HookTraveling )
		{
			Vector3 hookDirection = (mouseInWorld3D - transform.position).normalized;
			hookManager.attributes = attributes;
			hookManager.Launch( hookDirection, cursorDistance, hookLaunchSpeed, playerSpeed );

			inputReleased = false;
		}

		if( (theHook.transform.position - transform.position).magnitude >= maxChainDistance )
		{
			theHook.rigidbody2D.velocity *= -1;
		}

	
	}

	public void OnCollsionEnter2D(Collision2D collision)
	{
		GameObject collisionObject = collision.gameObject;
		WallAttributes wall = (WallAttributes)GetComponent<WallAttributes>();
		if( wall != null )
		{
			rigidbody2D.velocity = Vector2.zero;
			theHook.SetActive( false );
			attributes.OnWall = true;
			attributes.HookTraveling = false;
		}
	}
}
