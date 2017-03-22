using UnityEngine;
using System.Collections;

public class AnimationFlip : MonoBehaviour {
	
	public float LastX;
	public bool PrevOnWall;
	
	private CharacterAttributes attributes;
	// public float LastXVelocity;
	
	// Use this for initialization
	void Start () {
		
		attributes = GetComponent<CharacterAttributes>();
		PrevOnWall = attributes.OnWall;
		// LastXVelocity = 0;
	}
	
	void FixedUpdate()
	{
		bool changeDirection = false;
		if( LastX != 0 && GetComponent<Rigidbody>().velocity.x != 0 )
		{
			changeDirection = Mathf.Sign( LastX ) != Mathf.Sign( GetComponent<Rigidbody2D>().velocity.x ); 
		}
		
		if (changeDirection)
		{
			transform.localScale = new Vector2(3 * Mathf.Sign(GetComponent<Rigidbody2D>().velocity.x), transform.localScale.y);
			foreach( Transform child in transform)
			{
				child.localScale = new Vector3( Mathf.Abs(child.localScale.x) * Mathf.Sign(GetComponent<Rigidbody2D>().velocity.x), child.localScale.y );
			}
			float sign = Mathf.Sign(transform.localScale.x);
		}
		
		PrevOnWall = attributes.OnWall;
		if( LastX != 0 && GetComponent<Rigidbody>().velocity.x != 0 )
		{
			LastX = GetComponent<Rigidbody2D>().velocity.x;
		}
		
		// Hook is also being flipped at the same time
		// resulting in the hook being flipped twice
		// fix here or in Nicks Code
	}
	
	void OnCollisionEnter2D(Collision2D collission)
	{
		
		WallAttributes wall = (WallAttributes)collission.gameObject.GetComponent<WallAttributes>();
		
		if( wall != null )
		{
			if( wall.WhichWall == "Left" )
			{
				transform.localScale = new Vector2( 3, transform.localScale.y );
				foreach( Transform child in transform )
				{
					child.localScale = new Vector3( Mathf.Abs( child.localScale.x ) , child.localScale.y );
				}
			}
			else if( wall.WhichWall == "Right" )
			{
				transform.localScale = new Vector2( -3, transform.localScale.y );
				foreach( Transform child in transform )
				{
					child.localScale = new Vector3( Mathf.Abs( child.localScale.x ) * -1, child.localScale.y );
				}
			} 
		}
		
	}
	
}
