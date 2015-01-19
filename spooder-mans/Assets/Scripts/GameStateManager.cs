using UnityEngine;
using System.Collections;

public class GameStateManager : MonoBehaviour {


    public int StartingLives;
    public int[] playerStock { get; private set; }
    public int playersAlive;


	private SoundManager soundManager;
	public SoundManager theSoundManager;
	private bool victoryFound = false;

	// Use this for initialization
	void Start () {
        playerStock = new int[4];
        for(int i = 0; i < 4; ++i )
        {
            playerStock[i] = StartingLives;
            playersAlive = 4;
        }


		soundManager = (SoundManager)theSoundManager.GetComponent<SoundManager>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        playersAlive = 4;
        for( int i = 0; i < 4; ++i )
        {
            if( playerStock[i] <= 0 )
            {
                playersAlive--;

				if( playersAlive == 1 )
				{

					//Victory Stuff
					int victorIndex = -1;
					for(int j = 0; j < 4;j++){
						if(playerStock[j]!=0){
							victorIndex = j;

							break;
						}
					}
					if(victoryFound == false)
					{
						Debug.Log("victor index " + victorIndex);
						soundManager.playVictorAudio(victorIndex);
						victoryFound = true;
					}
				}

		    }
        }


	}

    public void LosePlayerLife(int player)
    {
        playerStock[player]--;
    }
}
