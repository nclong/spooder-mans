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
	private Vector2 cursorDirection;
	private PlayerInput playerInput;
	private int playerNum;

	private Vector2 mousePosition;
	// Use this for initialization
	void Start () {
		theHook.SetActive(true);
		hookManager = (HookManager)theHook.GetComponent<HookManager>();
		theHook.SetActive(false);
		attributes = GetComponent<CharacterAttributes> ();
		cursorDirection = new Vector2 (1f, 0f);
		playerInput = InputManager.PlayerInputs [playerNum];
		playerNum = attributes.playerNum;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
//		Vector3 mouseInWorld3D = Camera.main.ScreenToWorldPoint( Input.mousePosition );
//		mousePosition = mouseInWorld3D.In2D();
//
//		hookCursor.transform.localPosition = (mouseInWorld3D - transform.position).normalized * cursorDistance;

		if (!playerInput.rightJoystickX.IsWithin (0f, 0.01f) && !playerInput.rightJoystickY.IsWithin (0f, 0.01f)) {
			cursorDirection = new Vector2(playerInput.rightJoystickX, playerInput.rightJoystickY);
			cursorDirection = cursorDirection.normalized;
			hookCursor.transform.localPosition = cursorDirection * cursorDistance;
		}

		if( playerInput.hook )
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
			Vector3 hookDirection = cursorDirection;
			hookManager.attributes = attributes;
			hookManager.Launch( hookDirection, cursorDistance, hookLaunchSpeed, playerSpeed );
            attributes.HookLaunched = true;
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
