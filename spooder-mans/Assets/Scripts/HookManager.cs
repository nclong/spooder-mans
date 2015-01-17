using UnityEngine;
using System.Collections;

public class HookManager : MonoBehaviour {

	private LineRenderer line;
	private float hookSpeed;
	private float playerSpeed;
	private Vector3 dir;
	// Use this for initialization
	void Start () {
		//line = (LineRenderer)renderer;
	}
	
	// Update is called once per frame
	void Update () {
		if( enabled )
		{

			//line.SetPosition(1, transform.position);
		}
		//line.SetPosition(0, transform.parent.position);
	}

	public void Launch( Vector3 dir, float distance, float hookSpeed, float playerSpeed )
	{
		this.hookSpeed = hookSpeed;
		this.playerSpeed = playerSpeed;
		this.dir = dir;

		transform.localPosition = this.dir * distance;
		transform.gameObject.SetActive(true);
		rigidbody2D.velocity = this.dir.In2D() * this.hookSpeed;
		//line.SetPosition(0, transform.parent.position );
	}

	public void OnTriggerEnter2D(Collider2D collision)
	{
		GameObject collisionObject = collision.gameObject;
		WallAttributes wall = (WallAttributes)collisionObject.GetComponent<WallAttributes>();
		CharacterAttributes character = (CharacterAttributes)collisionObject.GetComponent<CharacterAttributes>();
		if( wall != null )
		{
			rigidbody2D.velocity = Vector2.zero;
			transform.parent.rigidbody2D.velocity = dir * playerSpeed;
		}

		if( character != null )
		{
			rigidbody2D.velocity = Vector2.zero;
			transform.gameObject.SetActive(false);
		}
	}
}
