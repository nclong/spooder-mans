using UnityEngine;
using System.Collections;

public class SweepLauncher : MonoBehaviour 
{
	public float Strength;
    public int windUpFrames;
    public int activeFrames;
    public int cooldownFrames;
    
    private int currentFrames = 0;
    private CharacterAttributes attributes;
    private int playerNum;
    private PlayerInput playerInput;
    private bool activated = false;
    private int cumulativeActiveFrames;
    private int cumulativeCooldownFrames;
    private Vector3 startSweepPos;



	// Use this for initialization
	void Start ()
	{
        attributes = transform.parent.GetComponent<CharacterAttributes>();
        playerNum = attributes.playerNum;
        playerInput = InputManager.PlayerInputs[playerNum];
        collider2D.enabled = false;
        cumulativeActiveFrames = windUpFrames + activeFrames;
        cumulativeCooldownFrames = cumulativeActiveFrames + cooldownFrames;
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{

        if( playerInput.sweep && !attributes.Sweeping )
        {
            attributes.Sweeping = true;
            startSweepPos = transform.position;
        }

        if( attributes.Sweeping )
        {
            transform.position = startSweepPos;
            currentFrames++;
            Debug.Log( "Sweep Frame: " + currentFrames.ToString() );
            if( currentFrames <= windUpFrames )
            {
                collider2D.enabled = false;
                //Animation Things
            }
            else if( currentFrames <= cumulativeActiveFrames )
            {
                collider2D.enabled = true;
                //More Animation Things
            }
            else if( currentFrames <= cumulativeCooldownFrames )
            {
                collider2D.enabled = false;
                //More Animation things or a stall
            }
            else
            {
                attributes.Sweeping = false;
                currentFrames = 0;
                collider2D.enabled = false;
            }
        }
	}

	public void OnTriggerEnter2D (Collider2D collider)
	{
	// launch Players
		GameObject CollisionObject = collider.gameObject;
        CharacterAttributes character = CollisionObject.GetComponent<CharacterAttributes>();
		if (character != null && character != attributes ) 
		{
            CollisionObject.rigidbody2D.isKinematic = true;
            CollisionObject.collider2D.isTrigger = true;

            character.OnWall = false;
            Vector2 tossVector;
            tossVector = CollisionObject.transform.position.x > 0
                ? new Vector2( -Strength, 0f )
                : new Vector2( Strength, 0f );
            CollisionObject.rigidbody2D.velocity = tossVector;
            character.Swept = true;
		}
	}
}
