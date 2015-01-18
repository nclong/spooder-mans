using UnityEngine;
using System.Collections;

public class Gravity : MonoBehaviour {

	public float Strength;
	public float TerminalVelocity;
	private CharacterAttributes attributes;
	// Use this for initialization
	void Start () {
		attributes = GetComponent<CharacterAttributes> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (!attributes.OnWall && !attributes.HookTraveling && !attributes.Hooked) {
			Debug.Log ("Applying Gravity");
			rigidbody2D.velocity -= new Vector2( 0f, Strength );
			if ( rigidbody2D.velocity.magnitude > TerminalVelocity )
			{
				rigidbody2D.velocity = rigidbody2D.velocity.normalized * TerminalVelocity;
			}
				}


	}
}
