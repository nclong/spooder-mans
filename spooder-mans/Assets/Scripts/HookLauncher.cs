using UnityEngine;
using System.Collections;

public class HookLauncher : MonoBehaviour {

	public GameObject theHook;
	public GameObject hookCursor;
	public float cursorDistance;
	public float hookLaunchSpeed;
	public float playerSpeed;
	public float maxChainDistance;
    public LayerMask markableLayer;
	
	private bool inputReceived;
	private bool inputReleased = true;
	private HookManager hookManager;
	private CharacterAttributes attributes;
	private Vector2 cursorDirection;
	private PlayerInput playerInput;
	private int playerNum;
    private GameObject farMarker;
    private RaycastHit2D farMarkerRay;

	private Vector2 mousePosition;
	// Use this for initialization
	void Start () {
		theHook.SetActive(true);
		hookManager = (HookManager)theHook.GetComponent<HookManager>();
		theHook.SetActive(false);
		attributes = GetComponent<CharacterAttributes> ();
		cursorDirection = new Vector2 (1f, 0f);
        playerNum = attributes.playerNum;
		playerInput = InputManager.PlayerInputs [playerNum];
        farMarker = (GameObject)Instantiate(hookCursor);
        farMarker.transform.parent = transform;
        farMarker.transform.localPosition = Vector3.zero;
        farMarker.transform.localScale *= 2;
        farMarker.SetActive( false );
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		if (!playerInput.rightJoystickX.IsWithin (0f, 0.0001f) && !playerInput.rightJoystickY.IsWithin (0f, 0.0001f)) {

			cursorDirection = new Vector2(playerInput.rightJoystickX * (playerInput.inverted ? -1 : 1), playerInput.rightJoystickY);
			cursorDirection = cursorDirection.normalized;
			hookCursor.transform.localPosition = cursorDirection * cursorDistance;
            hookCursor.transform.eulerAngles = new Vector3( hookCursor.transform.eulerAngles.x, hookCursor.transform.eulerAngles.y, cursorDirection.Angle() );
		}

        farMarkerRay = Physics2D.Raycast( hookCursor.transform.position.In2D(), cursorDirection, 100f,  markableLayer.value );
        if( farMarkerRay.collider != null )
        {
            GameObject hitObject = farMarkerRay.collider.gameObject;
            WallAttributes wall = hitObject.GetComponent<WallAttributes>();
            CharacterAttributes hitCharacterAttributes = hitObject.GetComponent<CharacterAttributes>();
            if( wall != null || ( hitCharacterAttributes != null && hitObject != transform.gameObject ) )
            {
                farMarker.SetActive( true );
                farMarker.transform.position = farMarkerRay.point;
            }
            else
            {
                farMarker.SetActive( false );
            }
        }
        else
        {
            farMarker.SetActive( false );
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

		if( inputReceived && inputReleased && !attributes.Hooked && !attributes.HookTraveling && !attributes.Swept && !attributes.Sweeping )
		{
            rigidbody2D.velocity = Vector2.zero;
			Vector3 hookDirection = (hookCursor.transform.position - transform.position);
            //hookDirection = new Vector3(hookDirection.x * Mathf.Sign(transform.localScale.x), hookDirection.y, hookDirection.z);
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
