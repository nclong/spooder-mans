using UnityEngine;
using System.Collections;

public class Sweep : MonoBehaviour 
{
	public GameObject SweepDetector;
	private bool InputPressed;
	private bool InputReleased = true;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
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
