﻿using UnityEngine;
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
	
	//public AudioSource sweepAudio0;
	//public AudioSource sweepAudio1;
	//private AudioSource[] sweepAudios;
	private SoundManager soundManager;
	public SoundManager theSoundManager;
	private bool inAirUsed;
	
	
	// Use this for initialization
	void Start ()
	{
		attributes = transform.parent.GetComponent<CharacterAttributes>();
		playerNum = attributes.playerNum;
		playerInput = InputManager.PlayerInputs[playerNum];
		GetComponent<Collider2D>().enabled = false;
		cumulativeActiveFrames = windUpFrames + activeFrames;
		cumulativeCooldownFrames = cumulativeActiveFrames + cooldownFrames;
		
		
		//sweepAudios = new AudioSource[]{sweepAudio0 , sweepAudio1};
		soundManager = (SoundManager)theSoundManager.GetComponent<SoundManager>();
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		transform.localPosition = Vector3.zero;
		
		if( playerInput.sweep && !attributes.Sweeping && !attributes.Hooked && !inAirUsed )
		{
			if (attributes.Jumping || attributes.HookTraveling)
			{
				InAir = true;
				inAirUsed = true;
			}
			else
			{
				InAir = false;
			}
			//sweepAudios[Random.Range(0,2)].Play ();
			soundManager.playSweepAudio(playerNum);
			
			attributes.Sweeping = true;
			startSweepPos = transform.position;
			transform.parent.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
			attributes.HookLaunched = false;
			attributes.HookTraveling = false;
			attributes.Jumping = false;
			theHook.SetActive( false );
			
			
			
			
			
			
		}
		else
		{
			if ( attributes.Jumping || attributes.HookTraveling || attributes.OnWall || attributes.Hooked || attributes.HookLaunched || attributes.newlySpawned)
			{
				inAirUsed = false;
			}
		}
		
		if( attributes.Sweeping )
		{
			transform.position = startSweepPos;
			currentFrames++;
			if( currentFrames <= windUpFrames )
			{
				GetComponent<Collider2D>().enabled = false;
				
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
				attributes.SweepingActive = true;
				GetComponent<Collider2D>().enabled = true;
				
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
				attributes.SweepingActive = false;
				GetComponent<Collider2D>().enabled = false;
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
				GetComponent<Collider2D>().enabled = false;
				
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
			CollisionObject.GetComponent<Rigidbody2D>().isKinematic = true;
			CollisionObject.GetComponent<Collider2D>().isTrigger = true;
			
			character.OnWall = false;
			character.HookLaunched = false;
			character.theHook.SetActive( false );
			character.HookTraveling = false;
			character.Hooked = false;
			character.Jumping = false;
			character.Sweeping = false;
			character.Swept = true;
			Vector2 tossVector;
			tossVector = CollisionObject.transform.position.x > 0
				? new Vector2( -Strength, 0f )
					: new Vector2( Strength, 0f );
			CollisionObject.GetComponent<Rigidbody2D>().velocity = tossVector;
			
			
		}
	}
}
