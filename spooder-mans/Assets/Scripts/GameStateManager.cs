using UnityEngine;
using System.Collections;

public class GameStateManager : MonoBehaviour {


    public int StartingLives;
    public int[] playerStock { get; private set; }
    public int playersAlive;
	// Use this for initialization
	void Start () {
        playerStock = new int[4];
        for(int i = 0; i < 4; ++i )
        {
            playerStock[i] = StartingLives;
            playersAlive = 4;
        }
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        playersAlive = 4;
        for( int i = 0; i < 4; ++i )
        {
            if( playerStock[i] <= 0 )
            {
                playersAlive--;
            }
        }

        if( playersAlive == 1 )
        {
            //Victory Stuff
        }
	}

    public void LosePlayerLife(int player)
    {
        playerStock[player]--;
    }
}
