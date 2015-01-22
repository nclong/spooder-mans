using UnityEngine;
using UnityEngine.UI;
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

    public Text redStock;
    public Text greenStock;
    public Text purpleStock;
    public Text yellowStock;

    private int victorIndex = -1;

    public Text redVictory;
    public Text greenVictory;
    public Text purpleVictory;
    public Text yellowVictory;

	// Use this for initialization
	void Start () {
        playerStock = new int[4];
        for(int i = 0; i < 4; ++i )
        {
            playerStock[i] = StartingLives;
            playersAlive = 4;
        }
        cameraStartingPos = new Vector3( 0f, 0f, -10f );		
        soundManager = (SoundManager)theSoundManager.GetComponent<SoundManager>();	}
	
	// Update is called once per frame
	void FixedUpdate () {
        redStock.text = "x " + playerStock[0].ToString();
        greenStock.text = "x " + playerStock[1].ToString();
        purpleStock.text = "x " + playerStock[2].ToString();
        yellowStock.text = "x " + playerStock[3].ToString();
        playersAlive = 4;
        for( int i = 0; i < 4; ++i )
        {
            if( playerStock[i] <= 0 )
            {
                playersAlive--;

				if( playersAlive == 1 )
				{

					for(int j = 0; j < 4;j++){
						if(playerStock[j]!=0){
							victorIndex = j;

							break;
						}
					}
					if(victoryFound == false)
					{
                        Debug.Log( victorIndex );
                        switch( victorIndex )
                        {
                            case 0:
                                redVictory.enabled = true;
                                redVictory.active = true;
                                break;
                            case 1:
                                greenVictory.enabled = true;
                                greenVictory.active = true;
                                break;
                            case 2:
                                purpleVictory.enabled = true;
                                purpleVictory.active = true;
                                break;
                            case 3:
                                yellowVictory.enabled = true;
                                yellowVictory.active = true;
                                break;
                            default:
                                Debug.Log( "This shouldn't happen!" );
                                break;
                        }
						soundManager.playVictorAudio(victorIndex);
						victoryFound = true;
					}
				}

		    }

            if( victoryFound )
            {

                
                if( Input.GetButton("StartOver"))
                {
                    Application.LoadLevel( "NickScene2" );
                }

                if( Input.GetButton("Quit"))
                {
                    Application.Quit();
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
