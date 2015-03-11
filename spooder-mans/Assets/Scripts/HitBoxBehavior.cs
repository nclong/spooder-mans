using UnityEngine;
using System.Collections;

public class HitBoxBehavior : MonoBehaviour
{

	public float power = 20f;
	public bool useHitAngle = true;
	public Vector2 dir = Vector2.up;

	private Rigidbody2D physicalBody;
	private CharacterControls myControls;
	private bool prevActive;
	private static float MID_THRESHOLD = 20;
	private static float MAX_THRESHOLD = 40;
	private static float CRIT_THRESHOLD = 60;

	void Awake()
	{
		physicalBody = GetComponentInParent <Rigidbody2D> ();
		myControls = GetComponentInParent <CharacterControls> ();
		prevActive = false;
	}

	void OnDisable ()
	{
		prevActive = false;
	}

	void FixedUpdate()
	{
		if ( !prevActive )
		{
			if ( power > MAX_THRESHOLD )
				myControls.sound.playHitLargeAudio ();
			else if ( power > MID_THRESHOLD )
				myControls.sound.playHitMidAudio ();
			else
				myControls.sound.playWhooshAudio ();
			prevActive = true;
		}
	}

	void OnTriggerEnter2D (Collider2D collision)
	{
		if ( collision.gameObject != null && collision.gameObject.transform != null && !myControls.IsLagged() )
		{
			CharacterControls controls = collision.gameObject.GetComponent<CharacterControls> ();
			HitBoxBehavior hitbox = collision.gameObject.GetComponent<HitBoxBehavior> ();

			Vector2 myAttack = GetAttackVector ( collision.gameObject.transform.position );

			if ( controls != null )
			{
				// You hit a player's collider
				controls.GetHit ( myAttack.normalized, myAttack.magnitude );
				myControls.SetLag ( ( int ) ( myAttack.magnitude / 3f ) );
				PlayAudio ( myControls.sound, myAttack.magnitude );
			}
			else if ( hitbox != null )
			{
				// You hit a player's attack hitbox
				Vector2 enemyAttack = hitbox.GetAttackVector ( transform.position );
				CharacterControls parentControls = collision.gameObject.GetComponentInParent<CharacterControls> ();

				// Whoever hits the hardest lands the hit
				if ( myAttack.magnitude > enemyAttack.magnitude )
				{
					float diff = myAttack.magnitude - enemyAttack.magnitude;
					parentControls.GetHit ( myAttack.normalized, diff );
					myControls.SetLag ( ( int ) ( diff / 3f ) );
					PlayAudio ( myControls.sound, diff );
				}
			}
		}
	}

	public Vector2 GetAttackVector ( Vector3 targetPos )
	{
		if ( useHitAngle )
			dir = targetPos - transform.position;
		return new Vector2 ( dir.x, dir.y ).normalized * ( power + ( physicalBody.velocity.magnitude / 2f ) );
	}

	private void PlayAudio ( SoundManager sound, float magnitude )
	{
		if ( magnitude > CRIT_THRESHOLD )
			sound.playImpactCritAudio ();
		else if ( magnitude > MAX_THRESHOLD )
			sound.playImpactLargeAudio ();
		else if ( magnitude > MID_THRESHOLD )
			sound.playImpactMidAudio ();
		else
			sound.playImpactWeakAudio ();
	}
}
