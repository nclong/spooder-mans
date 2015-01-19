using UnityEngine;
using System.Collections;

public class Sweep : MonoBehaviour 
{
	public GameObject SweepDetector;
	private bool InputPressed;
	private bool InputReleased = true;

	public AudioSource sweepAudio;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		sweepAudio.Play ();
		if (Input.GetButton ("Sweep1"))
			{
				
				InputPressed = true;
			} 
		else 
			{
				InputPressed = false;
				InputReleased = true;
			}
		if (InputPressed && InputReleased) 
			{
				SweepDetector.SetActive(true);
			}
	}
}
