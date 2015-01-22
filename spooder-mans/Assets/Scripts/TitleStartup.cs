using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TitleStartup : MonoBehaviour {

    public bool[] buttonsPressed;
    private bool allPressed = false;
    public Text player1Pressed;
    public Text player2Pressed;
    public Text player3Pressed;
    public Text player4Pressed;

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
           Application.LoadLevel( "NickScene2" );
        }

        for( int i = 0; i < 4; ++i )
        {
            switch( i )
            {
                case 0:
                    if( buttonsPressed[i] )
                    {
                        player1Pressed.enabled = true;
                        player1Pressed.active = true;
                    }
                    break;
                case 1:
                    if( buttonsPressed[i] )
                    {
                        player2Pressed.enabled = true;
                        player2Pressed.active = true;
                    }
                    break;
                case 2:
                    if( buttonsPressed[i] )
                    {
                        player3Pressed.enabled = true;
                        player3Pressed.active = true;
                    }
                    break;
                case 3:
                    if( buttonsPressed[i] )
                    {
                        player4Pressed.enabled = true;
                        player4Pressed.active = true;
                    }
                    break;
                default:
                    Debug.Log( "This shouldn't happen." );
                    break;
            }
        }
	
	}

    void OnGUI()
    {

    }
}
