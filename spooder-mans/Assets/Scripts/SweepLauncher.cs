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

	//public AudioSource sweepAudio0;
	//public AudioSource sweepAudio1;
	//private AudioSource[] sweepAudios;
	private SoundManager soundManager;
	public SoundManager theSoundManager;


	// Use this for initialization
	void Start ()
	{
        attributes = transform.parent.GetComponent<CharacterAttributes>();
        playerNum = attributes.playerNum;
        playerInput = InputManager.PlayerInputs[playerNum];
        collider2D.enabled = false;
        cumulativeActiveFrames = windUpFrames + activeFrames;
        cumulativeCooldownFrames = cumulativeActiveFrames + cooldownFrames;


		//sweepAudios = new AudioSource[]{sweepAudio0 , sweepAudio1};
		soundManager = (SoundManager)theSoundManager.GetComponent<SoundManager>();
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
        transform.localPosition = Vector3.zero;

        if( playerInput.sweep && !attributes.Sweeping && !attributes.Hooked )
        {
			//sweepAudios[Random.Range(0,2)].Play ();
			soundManager.playSweepAudio(playerNum);

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
