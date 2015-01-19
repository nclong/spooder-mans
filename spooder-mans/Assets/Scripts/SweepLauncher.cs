using UnityEngine;
using System.Collections;

public class SweepLauncher : MonoBehaviour 
{
	public float Strength;
    public int windUpFrames;
    public int activeFrames;
    public int cooldownFrames;
    public GameObject theHook;
    
    private int currentFrames = 0;
    private CharacterAttributes attributes;
    private int playerNum;
    private PlayerInput playerInput;
    private bool activated = false;
    private int cumulativeActiveFrames;
    private int cumulativeCooldownFrames;
    private Vector3 startSweepPos;
    private bool InAir = false;



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
        transform.localPosition = Vector3.zero;

        if( playerInput.sweep && !attributes.Sweeping && !attributes.Hooked )
        {
            if (attributes.Jumping || attributes.HookTraveling)
            {
                InAir = true;
            }
            else
            {
                InAir = false;
            }

            attributes.Sweeping = true;
            startSweepPos = transform.position;
            transform.parent.rigidbody2D.velocity = Vector2.zero;
            attributes.HookLaunched = false;
            attributes.HookTraveling = false;
            attributes.Jumping = false;
            theHook.SetActive( false );

            


            

        }

        if( attributes.Sweeping )
        {
            transform.position = startSweepPos;
            currentFrames++;
            if( currentFrames <= windUpFrames )
            {
                collider2D.enabled = false;

                if (InAir)
                {
                    attributes.anim.SetBool("Jumped", false);
                    attributes.anim.SetBool("AirWindup", true);
                }
                else
                {
                    attributes.anim.SetBool("Up", false);
                    attributes.anim.SetBool("Down", false);
                    attributes.anim.SetBool("Idle", false);
                    attributes.anim.SetBool("Windup", true);
                }
            }
            else if( currentFrames <= cumulativeActiveFrames )
            {
                collider2D.enabled = true;

                if (InAir)
                {
                    attributes.anim.SetBool("AirWindup", false);
                    attributes.anim.SetBool("AirSweep", true);
                }
                else
                {
                    attributes.anim.SetBool("Up", false);
                    attributes.anim.SetBool("Down", false);
                    attributes.anim.SetBool("Windup", false);
                    attributes.anim.SetBool("Sweep", true);
                }
                //More Animation Things
            }
            else if( currentFrames <= cumulativeCooldownFrames )
            {
                collider2D.enabled = false;
                if (InAir)
                {
                    attributes.anim.SetBool("AirSweep", false);
                    attributes.anim.SetBool("AirCooldown", true);
                }
                else
                {
                    attributes.anim.SetBool("Up", false);
                    attributes.anim.SetBool("Down", false);
                    attributes.anim.SetBool("Sweep", false);
                    attributes.anim.SetBool("Cooldown", true);
                }
                //More Animation things or a stall
            }
            else
            {
                attributes.Sweeping = false;
                currentFrames = 0;
                collider2D.enabled = false;

                if (InAir)
                {
                    attributes.anim.SetBool("Cooldown", false);
                    attributes.anim.SetBool("Jumped", true);
                }
                else
                {
                    attributes.anim.SetBool("Up", false);
                    attributes.anim.SetBool("Down", false);
                    attributes.anim.SetBool("Cooldown", false);
                    attributes.anim.SetBool("Idle", true);
                }

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
            Debug.Log( "Sweeping Character" );
            CollisionObject.rigidbody2D.isKinematic = true;
            CollisionObject.collider2D.isTrigger = true;

            character.OnWall = false;
            character.HookLaunched = false;
            character.HookTraveling = false;
            character.Hooked = false;
            character.Jumping = false;
            character.Sweeping = false;
            character.Swept = true;
            Vector2 tossVector;
            tossVector = CollisionObject.transform.position.x > 0
                ? new Vector2( -Strength, 0f )
                : new Vector2( Strength, 0f );
            CollisionObject.rigidbody2D.velocity = tossVector;
            
            
		}
	}
}
