using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {

	public static PlayerInput[] PlayerInputs;
    public int currentPlayers;
    public float joystickMonitorLeftX;
    public float joystickMonitorLeftY;
    public float joystickMonitorRightX;
    public float joystickMonitorRightY;


	// Use this for initialization
	void Start () {
		PlayerInputs = new PlayerInput[4];
		PlayerInputs [0] = new PlayerInput ();
        PlayerInputs[1] = new PlayerInput();
        PlayerInputs[2] = new PlayerInput();
        PlayerInputs[3] = new PlayerInput();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		for( int i = 0; i < currentPlayers; ++i )
		{
			PlayerInputs[i].leftJoystickX = joystickMonitorLeftX=  Input.GetAxis ("LeftJoystickX" + (i+1).ToString());
            PlayerInputs[i].leftJoystickY = joystickMonitorLeftY = Input.GetAxis( "LeftJoystickY" + ( i + 1 ).ToString() );
            PlayerInputs[i].rightJoystickX = joystickMonitorRightX = Input.GetAxis( "RightJoystickX" + ( i + 1 ).ToString() );
            PlayerInputs[i].rightJoystickY = joystickMonitorRightY = Input.GetAxis( "RightJoystickY" + ( i + 1 ).ToString() );

			PlayerInputs[i].jump = Input.GetButton ("Jump" + (i+1).ToString());
			PlayerInputs[i].sweep = Input.GetButton ("Sweep" + (i+1).ToString());
			PlayerInputs[i].hook = !(Input.GetAxis("Hook" + (i+1).ToString()).IsWithin( 0f, 0.01f));
		}
	}
}
