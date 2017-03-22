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
		if (!attributes.OnWall && !attributes.HookTraveling && !attributes.Hooked && !attributes.newlySpawned && !attributes.Sweeping ) {
			GetComponent<Rigidbody2D>().velocity -= new Vector2( 0f, Strength );
			if ( GetComponent<Rigidbody2D>().velocity.magnitude > TerminalVelocity )
			{
				GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity.normalized * TerminalVelocity;
			}
		}


	}
}
