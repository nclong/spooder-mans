using UnityEngine;
using System.Collections;

public class FireManager : MonoBehaviour {

	public GameObject LeftWall;
	public GameObject RightWall;

	public float Timer;
	public float FireTime;
	public bool FireOn;

	public float TimeForStage1;
	public float TimeForStage2;
	public float TimeForStage3;

	public int RandomTimerMin;
	public int RandomTimerMax;

	public float WhichWall;
	public float RandomTimer;

	private WallAttributes LeftAttributes;
	private WallAttributes RightAttributes;


	// Use this for initialization
	void Start () {
		FireOn = false;

		LeftAttributes = LeftWall.GetComponent<WallAttributes>();
		RightAttributes = RightWall.GetComponent<WallAttributes> ();

		WhichWall = Random.Range(1,10);
		RandomTimer = Random.Range(RandomTimerMin, RandomTimerMax);
	
	}
	
	// Update is called once per frame
	void Update () {
		if(FireOn == false){
			Timer += Time.deltaTime;
		}
		else if(FireOn == true){
			FireTime += Time.deltaTime;
		}

		if (Timer > RandomTimer)
		{
			FireOn = true;
			Timer = 0;
		}

		if (FireOn == true){
			if(FireTime < TimeForStage1 )
			{
				//stage 1
				if (WhichWall <=5){
					LeftWall.renderer.material.color = new Color(1, .61f, 1);
				}
				else if(WhichWall >5) {
					RightWall.renderer.material.color = new Color(1, .61f, 1);
				}
			}
			else if(FireTime < TimeForStage2)
			{
				//stage 2
				if (WhichWall <= 5){
					LeftWall.renderer.material.color = new Color(1, .129f, .5255f);
				}
				else if(WhichWall > 5) {
					RightWall.renderer.material.color = new Color(1, .129f, .5255f);
				}
			}
			else if(FireTime < TimeForStage3)
			{
				//stage 3
				if (WhichWall <=5){
					LeftWall.renderer.material.color = Color.red;
					LeftAttributes.Fire = true;
				}
				else if(WhichWall >5) {
					RightWall.renderer.material.color = Color.red;
					RightAttributes.Fire = true;
				}
			}
			else 
			{
				LeftWall.renderer.material.color = Color.white;
				RightWall.renderer.material.color = Color.white;
				LeftAttributes.Fire = false;
				RightAttributes.Fire = false;
				FireOn = false;
				FireTime = 0;
				RandomTimer = Random.Range(RandomTimerMin, RandomTimerMax);
				WhichWall = Random.Range(1,10);
			}
		}


	} 
}
