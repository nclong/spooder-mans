using UnityEngine;
using System.Collections;

public class HookLauncher : MonoBehaviour {

	public GameObject theHook;
	public GameObject hookCursor;
	public float cursorDistance;
	public float hookLaunchSpeed;
	public float playerSpeed;
	public float maxChainDistance;
    public float farMarkerOffset;
    public LayerMask markableLayer;

    public float rotationAcceleration;
    public float rotationMaxSpeed;
	public float rotationZeroThreshold;

	private bool inputReceived;
	private bool inputReleased = true;
	private HookManager hookManager;
	private CharacterAttributes attributes;
	private Vector2 cursorDirection;
	private PlayerInput playerInput;
	private int playerNum;
    private GameObject farMarker;
    private RaycastHit2D farMarkerRay;
    private float currentAngle;
    private float stickAngle;
    private float actualMaxRotationSpeed;
    private float rotationSpeed;
    private bool rotateClockwise = false;
	// Use this for initialization
	void Start () {
        attributes = GetComponent<CharacterAttributes>();
		theHook.SetActive(true);
		hookManager = (HookManager)theHook.GetComponent<HookManager>();
        hookManager.attributes = attributes;
		theHook.SetActive(false);
		
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

		if (playerInput.rightJoystickX != 0f || playerInput.rightJoystickY != 0f ) {

            stickAngle = Mathf.Atan2( playerInput.rightJoystickY, playerInput.rightJoystickX ) * Mathf.Rad2Deg;
            actualMaxRotationSpeed = rotationMaxSpeed * new Vector2( playerInput.rightJoystickX, playerInput.rightJoystickY ).magnitude;
            rotationSpeed += rotationAcceleration * new Vector2( playerInput.rightJoystickX, playerInput.rightJoystickY ).magnitude;
            if( rotationSpeed > actualMaxRotationSpeed )
            {
                rotationSpeed = actualMaxRotationSpeed;
            }

            if( stickAngle == 0f )
            {
                stickAngle = 360f;
            }

            if( currentAngle == 0f )
            {
                currentAngle = 360f;
            }

            if( stickAngle > currentAngle )
            {
                if( stickAngle - currentAngle >= 180 )
                {
                    rotateClockwise = true;
                }
                else
                {
                    rotateClockwise = false;
                }
            }
            else
            {
                if( currentAngle - stickAngle >= 180 )
                {
                    rotateClockwise = false;
                }
                else
                {
                    rotateClockwise = true;
                }
            }

            if( currentAngle > 270 && ((360 - currentAngle) + stickAngle) < rotationSpeed )
            {
                currentAngle = stickAngle;
            }
            else if( stickAngle > 270 && ((360 - stickAngle) + currentAngle) < rotationSpeed )
            {
                currentAngle = stickAngle;
            }
            else
            {
                if( rotateClockwise )
                {
                    float targetAngle = currentAngle - rotationSpeed;
                    if( rotationSpeed < 0 )
                    {
                        rotationSpeed += 360f;
                    }
                    if( targetAngle < stickAngle)
                    {
                        currentAngle = stickAngle;
                    }
                    else
                    {
                        currentAngle = targetAngle;
                    }
                }
                else
                {
                    float targetAngle = currentAngle + rotationSpeed;
                    if( rotationSpeed > 360 )
                    {
                        rotationSpeed -= 360f;
                    }
                    if( targetAngle > stickAngle )
                    {
                        currentAngle = stickAngle;
                    }
                    else
                    {
                        currentAngle = targetAngle;
                    }
                }
            }

            if( Mathf.Abs(stickAngle - currentAngle) <= rotationZeroThreshold )
            {
                currentAngle = stickAngle;
            }

            cursorDirection = currentAngle.ToVector2();
            cursorDirection = new Vector2( cursorDirection.x * ( playerInput.inverted ? -1 : 1 ), cursorDirection.y ).normalized;
            hookCursor.transform.localPosition = cursorDirection * cursorDistance;
            hookCursor.transform.eulerAngles = new Vector3( hookCursor.transform.eulerAngles.x, hookCursor.transform.eulerAngles.y, currentAngle );

        }
        else
        {
            actualMaxRotationSpeed = 0f;
            rotationSpeed = 0f;
        }

        farMarkerRay = Physics2D.Raycast( hookCursor.transform.position.In2D(), new Vector2(cursorDirection.x * (playerInput.inverted ? -1 : 1), cursorDirection.y), 100f,  markableLayer.value );
        if( farMarkerRay.collider != null )
        {
            GameObject hitObject = farMarkerRay.collider.gameObject;
            WallAttributes wall = hitObject.GetComponent<WallAttributes>();
            CharacterAttributes hitCharacterAttributes = hitObject.GetComponent<CharacterAttributes>();
            if( wall != null || ( hitCharacterAttributes != null && hitObject != transform.gameObject ) )
            {
                farMarker.SetActive( true );
                Vector2 offset = (transform.position.In2D() - farMarkerRay.point).normalized * farMarkerOffset;
                farMarker.transform.position = farMarkerRay.point + offset; ;
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

		if( inputReceived && inputReleased && !attributes.Hooked && !attributes.HookTraveling && !attributes.HookLaunched && !attributes.Swept && !attributes.Sweeping && !attributes.newlySpawned)
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
