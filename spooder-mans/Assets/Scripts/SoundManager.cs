using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

	//jump Audios
	public AudioSource jumpAudio;


	//sweep Audios
	public AudioSource sweepAudio0_0;
	public AudioSource sweepAudio0_1;
	public AudioSource sweepAudio0_2;
	public AudioSource sweepAudio1_0;
	public AudioSource sweepAudio1_1;
	public AudioSource sweepAudio1_2;
	public AudioSource sweepAudio2_0;
	public AudioSource sweepAudio2_1;
	public AudioSource sweepAudio2_2;
	public AudioSource sweepAudio3_0;
	public AudioSource sweepAudio3_1;
	public AudioSource sweepAudio3_2;
	private AudioSource[] sweepAudios0;
	private AudioSource[] sweepAudios1;
	private AudioSource[] sweepAudios2;
	private AudioSource[] sweepAudios3;
	private AudioSource[,] arrayOfPlayersSweep;

	//grapple Audios
	public AudioSource grappleAudio0;
	//clank Audios
	public AudioSource clankAudio0;
	//death Audios
	public AudioSource deathAudio0;
	public AudioSource deathAudio1;
	public AudioSource deathAudio2;
	public AudioSource deathAudio3;
	private AudioSource[] deathAudios;

	//victory Audios
	public AudioSource victorAudio0;
	public AudioSource victorAudio1;
	public AudioSource victorAudio2;
	public AudioSource victorAudio3;
	private AudioSource[] victorAudios;

	// Use this for initialization
	void Start () {
		sweepAudios0 = new AudioSource[]{sweepAudio0_0,sweepAudio0_1,sweepAudio0_2};
		sweepAudios1 = new AudioSource[]{sweepAudio1_0,sweepAudio1_1,sweepAudio1_2};
		sweepAudios2 = new AudioSource[]{sweepAudio2_0,sweepAudio2_1,sweepAudio2_2};
		sweepAudios3 = new AudioSource[]{sweepAudio3_0,sweepAudio3_1,sweepAudio3_2};
		arrayOfPlayersSweep = new AudioSource[4,3]{{sweepAudio0_0,sweepAudio0_1,sweepAudio0_2},{sweepAudio1_0,sweepAudio1_1,sweepAudio1_2},{sweepAudio2_0,sweepAudio2_1,sweepAudio2_2},{sweepAudio3_0,sweepAudio3_1,sweepAudio3_2}};


		deathAudios = new AudioSource[]{deathAudio0,deathAudio2,deathAudio2,deathAudio3};
		victorAudios = new AudioSource[]{victorAudio0,victorAudio1,victorAudio2,victorAudio3};
	}
	// Update is called once per frame
	void Update () {
	
	}

	public void playJumpAudio (){
		Debug.Log ("played jump sound");
		jumpAudio.Play ();
	}

	public void playSweepAudio (int player){
		Debug.Log ("played sweep sound");
		arrayOfPlayersSweep[player,Random.Range (0, 3)].Play ();
		//sweepAudios [Random.Range (0, 3)].Play ();
	}

	public void playGrappleAudio(){
		Debug.Log("played grapple sound");
		grappleAudio0.Play ();
	}	

	public void playClankAudio(){
		Debug.Log("played grapple sound");
		clankAudio0.Play ();
	}	


	public void playDeathAudio(int player){
		Debug.Log ("played death sound");
		deathAudios[player].Play ();
	}

	public void playVictorAudio(int player){
		victorAudios [player].Play ();
	}

}
