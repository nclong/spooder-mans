using UnityEngine;
using System.Collections;

public class CharacterAttributes : MonoBehaviour {
	
	public bool OnWall = true;
	public bool Hooked;
	public bool Jumping;
	public bool Swept;
	public bool Sweeping;
	public bool SweepingActive;
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
	public int framesBeforeRespawn;
	public Animator anim;
	public GameObject gameStateManagerObject;
	private int framesSwept = 0;
	private int framesSpawned = 0;
	private PlayerInput playerInput;
	private GameStateManager gameStateManager;
	private int framesSinceDeath = 0;
	private bool respawning = false;
	private SoundManager soundManager;
	public SoundManager theSoundManager;
	private LineRenderer line;
	
	// Use this for initialization
	void Start () {
		OnWall = true;
		playerInput = InputManager.PlayerInputs[playerNum];
		gameStateManager = gameStateManagerObject.GetComponent<GameStateManager>();
		soundManager = (SoundManager)theSoundManager.GetComponent<SoundManager>();
		anim = GetComponent<Animator>();
		line = GetComponent<LineRenderer>();
		line.enabled = false;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if( theHook.activeSelf )
		{
			line.enabled = true;
			line.SetPosition( 0, transform.position );
			line.SetPosition( 1, theHook.transform.position );
		}
		else
		{
			line.enabled = false;
		}
		
		if( Swept )
		{
			theHook.SetActive( false );
			framesSwept++;
			if( framesSwept <= SweepStunFrames )
			{
				float scale = 1f - (float)framesSwept / (float)SweepStunFrames;
				float powerScale = Mathf.Pow( scale, 0.9f );
				GetComponent<Rigidbody2D>().velocity = new Vector2( GetComponent<Rigidbody2D>().velocity.x * powerScale, 0f );
			}
			else
			{
				Swept = false;
				GetComponent<Collider2D>().isTrigger = false;
				GetComponent<Rigidbody2D>().isKinematic = false;
				framesSwept = 0;
			}
		}
		
		if( transform.position.y <= -10 )
		{
			KillPlayer();
			anim.SetBool("Down", false);
			anim.SetBool("Stunned", false);
			anim.SetBool("Jumped", true);
			anim.SetBool("Idle", false);
			anim.SetBool("Hooked", false);
		}
		
		if( newlySpawned )
		{
			
			++framesSpawned;
			if( framesSpawned > framesNewlySpawned + framesBeforeRespawn)
			{
				newlySpawned = false;
				framesSpawned = 0;
			}
		}
		
		if( respawning )
		{
			++framesSinceDeath;
			if( framesSinceDeath >= framesBeforeRespawn )
			{
				GetComponent<Renderer>().enabled = true;
				GetComponent<Rigidbody2D>().velocity = new Vector2( SpawnVector.x + Random.Range( -SpawnVariance, SpawnVariance ), SpawnVector.y/* + Random.Range( -SpawnVariance, SpawnVariance ) */);
				framesSinceDeath = 0;
				newlySpawned = true;
				respawning = false;
				foreach( Transform child in transform )
				{
					if( child != theHook )
					{
						child.gameObject.SetActive( true ); 
					}
				}
			}
		}
		
		
		//hook launched audio
		//		if(HookLaunched){
		//			hookLaunchAudio.Play ();
		//		}
		//		else{
		//			hookLaunchAudio.Stop ();
		//		}
		
		if( transform.position.y > 9f )
		{
			transform.position = new Vector3( transform.position.x, 9f, 0f );
		}
		
		if( transform.localScale.x != 0 )
		{
			if( Mathf.Sign( transform.localScale.x ) > 0 )
			{
				playerInput.inverted = false;
			}
			else
			{
				playerInput.inverted = true;
			}
		}
		
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
				collisionObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
				GetComponent<Rigidbody2D>().velocity = Vector2.zero;
				Hooked = false;
				HookTraveling = false;
				HookLaunched = false;
				character.HookTraveling = false;
				character.HookLaunched = false;
				character.Hooked = false;
				theHook.SetActive( false );
				character.theHook.SetActive( false );
			}
			
			if( OnWall )
			{
				GetComponent<Rigidbody2D>().velocity = Vector2.zero;
				Hooked = false;
				HookTraveling = false;
				HookLaunched = false;
				
				character.HookTraveling = false;
				character.HookLaunched = false;
				character.Hooked = false;
				theHook.SetActive( false );
				character.theHook.SetActive( false );
				
			}
			
			if( character.OnWall )
			{
				collisionObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
				Hooked = false;
				HookTraveling = false;
				HookLaunched = false;
				character.HookTraveling = false;
				character.HookLaunched = false;
				character.Hooked = false;
				theHook.SetActive( false );
				character.theHook.SetActive( false );
			}
			
		}
		if (wall != null) 
		{
			OnWall = true;
			Jumping = false;
			Hooked = false;
			Swept = false;
			Sweeping = false;
			SweepingActive = false;
			HookTraveling = false;
			HookLaunched = false;
			currentWall = wall;
			
			theHook.SetActive( false );
			
			
			anim.SetBool("Stunned", false);
			anim.SetBool("Up", false);
			anim.SetBool("Down", false);
			anim.SetBool("AirCooldown", false);
			anim.SetBool("Cooldown", false);
			anim.SetBool( "Hooked", HookTraveling );
			anim.SetBool( "Jumped", Jumping );
			//anim.SetBool( "Hooked", false );
			//anim.SetBool( "Jumped", false );
			anim.SetBool("Idle", OnWall);		
		}
	}
	
	//public void LateUpdate()
	//{
	//    if( transform.position.y > 9f )
	//    {
	//        transform.position = new Vector3( transform.position.x, 9f, 0f );
	//    }
	
	//    if( transform.localScale.x != 0 )
	//    {
	//        if( Mathf.Sign( transform.localScale.x ) > 0 )
	//        {
	//            playerInput.inverted = false;
	//        }
	//        else
	//        {
	//            playerInput.inverted = true;
	//        } 
	//    }
	//}
	
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
		foreach( Transform child in transform )
		{
			child.gameObject.SetActive( false );
		}
		OnWall = false;
		Hooked = false;
		Jumping = false;
		Swept = false;
		Sweeping = false;
		SweepingActive = false;
		HookTraveling = false;
		HookLaunched = false;
		newlySpawned = true;
		framesSpawned = 0;
		theHook.SetActive( false );
		
		//Decrease Stock
		//play death sound
		//deathAudio.Play ();
		soundManager.playDeathAudio (playerNum);
		//Decrease Stock
		gameStateManager.LosePlayerLife( playerNum );
		
		if( gameStateManager.playerStock[playerNum] > 0 )
		{
			respawning = true;
			transform.position = Spawner.transform.position;
			GetComponent<Renderer>().enabled = false;
			GetComponent<Rigidbody2D>().velocity = Vector2.zero;
			newlySpawned = true;
			
		}
		else
		{
			Destroy( transform.gameObject );
		}
	}
}
