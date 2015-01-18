using UnityEngine;
using System.Collections;

public class CharacterAttributes : MonoBehaviour {

	public bool OnWall = true;
	public bool Hooked;
	public bool Jumping;
	public bool Swept;
	public bool Sweeping;
	public bool HookTraveling;
    public bool HookLaunched;
	public WallAttributes currentWall;

	public int playerNum;

	// Use this for initialization
	void Start () {
		OnWall = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnCollisionEnter2D(Collision2D collision)
	{
		GameObject collisionObject = collision.gameObject;
		WallAttributes wall = (WallAttributes)collisionObject.GetComponent<WallAttributes>();
		Debug.Log (wall.ToString ());
		if( collisionObject.tag == "Player" )
		{
			if( Hooked || HookTraveling)
			{
				collisionObject.rigidbody2D.velocity = Vector2.zero;
				rigidbody2D.velocity = Vector2.zero;
				Hooked = false;
			}
		}
		if (wall != null) 
		{
			OnWall = true;
			Debug.Log ("OnWall True");
			Jumping = false;
			Hooked = false;
			Swept = false;
			HookTraveling = false;
            HookLaunched = false;
			Debug.Log("Hook Traveling False");
			currentWall = wall;
		}
	}

//	public void OnCollisionStay2D(Collision2D collision)
//	{
//		GameObject collisionObject = collision.gameObject;
//		WallAttributes wall = collisionObject.GetComponent<WallAttributes>();
//		if (wall != null) {
//			OnWall = true;
//			Jumping = false;
//			Hooked = false;
//			Swept = false;
//			HookTraveling = false;	
//		}
//	}

	public void OnCollisionExit2D(Collision2D collision)
	{
		GameObject collisionObject = collision.gameObject;
		WallAttributes wall = collisionObject.GetComponent<WallAttributes>();
		if( wall != null )
		{
			OnWall = false;
		}
	}
}
