using UnityEngine;
using System.Collections;

public class GameStateManager : MonoBehaviour {


    public int StartingLives;
    public int[] playerStock { get; private set; }
    public int playersAlive;
    public float cameraShakeIntensity;
    [Range(0f, 1f)]
    public float cameraShakeSpeed;
    public float cameraShakeDecay;
    public int cameraShakeFrames;

    private Vector3 cameraStartingPos;
    private Vector3 cameraTargetPos;
    private float currentCameraShake;
    private int framesShaking = 0;
    private bool shaking = false;
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
        cameraStartingPos = new Vector3( 0f, 0f, -10f );		soundManager = (SoundManager)theSoundManager.GetComponent<SoundManager>();	}
	
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



        if( shaking )
        {
            ++framesShaking;
            if( Camera.main.transform.position == cameraTargetPos )
            {
                cameraTargetPos = new Vector3( cameraStartingPos.x + Random.Range( -cameraShakeIntensity, cameraShakeIntensity ),
                                                cameraStartingPos.y + Random.Range( -cameraShakeIntensity, cameraShakeIntensity ),
                                                cameraStartingPos.z );
            }

            Camera.main.transform.position = Vector3.Lerp( Camera.main.transform.position, cameraTargetPos, cameraShakeSpeed );

            if( framesShaking >= cameraShakeFrames)
            {
                framesShaking = 0;
                shaking = false;
                Camera.main.transform.position = cameraStartingPos;
            }
        }
	}

    public void LosePlayerLife(int player)
    {
        playerStock[player]--;
        cameraTargetPos = Camera.main.transform.position;
        shaking = true;
    }
}
