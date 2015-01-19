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
    public bool newlySpawned;
    public int framesNewlySpawned;
	public WallAttributes currentWall;
    public int SweepStunFrames;
	public int playerNum;
    public GameObject Spawner;
    public Vector2 SpawnVector;
    public float SpawnVariance;
    public GameObject theHook;

    private int framesSwept = 0;
    private int framesSpawned = 0;

	//public AudioSource deathAudio;

	private SoundManager soundManager;
	public SoundManager theSoundManager;
	//public AudioSource hookLaunchAudio;

	// Use this for initialization
	void Start () {
		OnWall = true;
		soundManager = (SoundManager)theSoundManager.GetComponent<SoundManager>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if( Swept )
        {
            framesSwept++;
            if( framesSwept <= SweepStunFrames )
            {
                float scale = 1f - (float)framesSwept / (float)SweepStunFrames;
                float powerScale = Mathf.Pow( scale, 0.9f );
                rigidbody2D.velocity = new Vector2( rigidbody2D.velocity.x * powerScale, 0f );
            }
            else
            {
                Swept = false;
                collider2D.isTrigger = false;
                rigidbody2D.isKinematic = false;
                framesSwept = 0;
            }
        }

        if( transform.position.y <= -10 )
        {
            KillPlayer();
        }

        if( newlySpawned )
        {
            ++framesSpawned;
            if( framesSpawned > framesNewlySpawned)
            {
                newlySpawned = false;
                framesSpawned = 0;
            }
        }


		//hook launched audio
//		if(HookLaunched){
//			hookLaunchAudio.Play ();
//		}
//		else{
//			hookLaunchAudio.Stop ();
//		}
		
	}
	
	public void OnCollisionEnter2D(Collision2D collision)
	{
		GameObject collisionObject = collision.gameObject;
		WallAttributes wall = (WallAttributes)collisionObject.GetComponent<WallAttributes>();
        CharacterAttributes character = collisionObject.GetComponent<CharacterAttributes>();
		if( character != null )
		{
			if( Hooked || HookTraveling)
			{
				collisionObject.rigidbody2D.velocity = Vector2.zero;
				rigidbody2D.velocity = Vector2.zero;
				Hooked = false;
                HookTraveling = false;
                HookLaunched = false;
                character.HookTraveling = false;
                character.HookLaunched = false;
                character.Hooked = false;
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

    public void LateUpdate()
    {
        if( transform.position.y > 9f )
        {
            transform.position = new Vector3( transform.position.x, 9f, 0f );
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
            currentWall = null;
		}
	}

    public void KillPlayer()
    {
        transform.position = Spawner.transform.position;
        rigidbody2D.velocity = new Vector2( SpawnVector.x + Random.Range( -SpawnVariance, SpawnVariance ), SpawnVector.y + Random.Range( -SpawnVariance, SpawnVariance ) );
        OnWall = false;
        Hooked = false;
        Jumping = false;
        Swept = false;
        Sweeping = false;
        HookTraveling = false;
        HookLaunched = false;
        newlySpawned = true;
        framesSpawned = 0;
        theHook.SetActive( false );

		//play death sound
		//deathAudio.Play ();
		Debug.Log (playerNum);
		soundManager.playDeathAudio (playerNum);
        //Decrease Stock
    }
}
