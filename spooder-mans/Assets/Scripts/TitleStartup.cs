using UnityEngine;
using System.Collections;

public class TitleStartup : MonoBehaviour {

    public bool[] buttonsPressed;
    private bool allPressed = false;

	// Use this for initialization
	void Start () {
        buttonsPressed = new bool[4] { false, false, false, false };
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        for(int i = 0; i < 4; ++i )
        {
            if( InputManager.PlayerInputs[i].jump)
            {
                buttonsPressed[i] = true;
            }
        }

        for( int i = 0; i < 4; ++i )
        {
            if( buttonsPressed[i] == false)
            {
                allPressed = false;
            }
        }

        if( buttonsPressed[0] &&  buttonsPressed[2] && buttonsPressed[1] && buttonsPressed[3])
        {
           Application.LoadLevel( "MasterScene" );
        }

	
	}
}
