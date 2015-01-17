using UnityEngine;
using System.Collections;

public class SweepLauncher : MonoBehaviour 
{
	public float Lifetime;
	public float Strength;
	private float CurrentTimer;

	// Use this for initialization
	void Start ()
	{
		CurrentTimer = 0f;
	}
	
	// Update is called once per frame
	void FixedUpdate () 

	{
	if (enabled) 
		{
		CurrentTimer += Time.fixedDeltaTime;
			if (CurrentTimer >= Lifetime) 
			{
				CurrentTimer = 0f;
				transform.gameObject.SetActive (false);
			}
		}
	}

	public void OnTriggerEnter2D (Collider2D collider)
	{
	// launch Players
		GameObject CollisionObject = collider.gameObject;
		if (CollisionObject.tag == "Player" && CollisionObject != transform.parent.gameObject) 
		{
			CollisionObject.rigidbody2D.velocity = new Vector2 (Strength, 0f);
		}
	}
}
