using UnityEngine;
using System.Collections;

public class HookLauncher : MonoBehaviour {

	public GameObject theHook;
	public GameObject hookCursor;
	public float cursorDistance;
	public float hookLaunchSpeed;
	public float playerSpeed;

	private bool inputReceived;
	private bool inputReleased = true;
	private HookManager hookManager;

	private Vector2 mousePosition;
	// Use this for initialization
	void Start () {
		theHook.SetActive(true);
		hookManager = (HookManager)theHook.GetComponent<HookManager>();
		theHook.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
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

		if( inputReceived && inputReleased )
		{
			Vector3 hookDirection = (mouseInWorld3D - transform.position).normalized;
			hookManager.Launch( hookDirection, cursorDistance, hookLaunchSpeed, playerSpeed, transform );

			inputReleased = false;
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
		}
	}
}
