using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameStateManager : MonoBehaviour {


	public SoundManager sound;
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

	public AudioSource bgm;

	// Use this for initialization
	void Start () {
        playerStock = new int[4];
        for(int i = 0; i < 4; ++i )
        {
            playerStock[i] = StartingLives;
            playersAlive = 4;
        }
        cameraStartingPos = new Vector3( 0f, 0f, -10f );
	}
	
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
                                break;
                            case 1:
                                greenVictory.enabled = true;
                                break;
                            case 2:
                                purpleVictory.enabled = true;
                                break;
                            case 3:
                                yellowVictory.enabled = true;
                                break;
                            default:
                                Debug.Log( "This shouldn't happen!" );
                                break;
                        }
						bgm.Stop();
						sound.playVictorAudio(victorIndex);
						sound.playVictorMusic (victorIndex);
						victoryFound = true;
					}
				}

		    }

            if( victoryFound )
            {

                
                if( Input.GetButton("StartOver"))
                {
                    Application.LoadLevel( "TedScene" );
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
