using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {

	public static PlayerInput[] PlayerInputs;

	// Use this for initialization
	void Start () {
		PlayerInputs = new PlayerInput[4];
		PlayerInputs [0] = new PlayerInput ();
        PlayerInputs[1] = new PlayerInput();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		for( int i = 0; i < 1; ++i )
		{
			PlayerInputs[i].leftJoystickX = Input.GetAxis ("LeftJoystickX" + (i+1).ToString());
			PlayerInputs[i].leftJoystickY = Input.GetAxis ("LeftJoystickY" + (i+1).ToString());
			PlayerInputs[i].rightJoystickX = Input.GetAxis ("RightJoystickX" + (i+1).ToString());
			PlayerInputs[i].rightJoystickY = Input.GetAxis ("RightJoystickY" + (i+1).ToString());

			PlayerInputs[i].jump = Input.GetButton ("Jump" + (i+1).ToString());
			PlayerInputs[i].sweep = Input.GetButton ("Sweep" + (i+1).ToString());
			PlayerInputs[i].hook = !(Input.GetAxis("Hook" + (i+1).ToString()).IsWithin( 0f, 0.01f));
		}
	}
}
