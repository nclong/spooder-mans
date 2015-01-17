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

	public void Launch( Vector3 dir, float distance, float hookSpeed, float playerSpeed, Transform parent )
	{
		this.hookSpeed = hookSpeed;
		this.playerSpeed = playerSpeed;
		this.dir = dir;

		transform.parent = parent;
		transform.localPosition = this.dir * distance;
		transform.gameObject.SetActive(true);
		rigidbody2D.velocity = this.dir.In2D() * this.hookSpeed;
		//line.SetPosition(0, transform.parent.position );
	}

	public void OnCollisionEnter2D(Collision2D collision)
	{
		GameObject collisionObject = collision.gameObject;
		WallAttributes wall = (WallAttributes)collisionObject.GetComponent<WallAttributes>();
		if( wall != null )
		{
			transform.parent.rigidbody2D.velocity = dir * playerSpeed;
		}
		transform.gameObject.SetActive( false );
	}
}
