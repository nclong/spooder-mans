using UnityEngine;
using System.Collections;

public class CharacterAttributes : MonoBehaviour {

	public bool OnWall {get; private set; }
	public bool Stunned { get;  set; }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnCollisionEnter2D(Collision2D collision)
	{
		GameObject collisionObject = collision.gameObject;
		if( collisionObject.tag == "Player" && Stunned )
		{
			Stunned = false;
			collisionObject.rigidbody2D.velocity = Vector2.zero;
		}
	}
}
