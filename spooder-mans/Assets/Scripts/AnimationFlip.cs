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
        if( LastX != 0 && rigidbody.velocity.x != 0 )
        {
            changeDirection = Mathf.Sign( LastX ) != Mathf.Sign( rigidbody2D.velocity.x ); 
        }

        if (changeDirection)
        {
            transform.localScale = new Vector2(3 * Mathf.Sign(rigidbody2D.velocity.x), transform.localScale.y);
            float sign = Mathf.Sign(transform.localScale.x);
        }

        PrevOnWall = attributes.OnWall;
        if( LastX != 0 && rigidbody.velocity.x != 0 )
        {
            LastX = rigidbody2D.velocity.x;
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
            }
            else if( wall.WhichWall == "Right" )
            {
                transform.localScale = new Vector2( -3, transform.localScale.y );
            } 
        }

    }

}
