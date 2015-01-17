using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WallAttributes : MonoBehaviour {

	public string WhichWall;
	public bool Fire;
	public List<GameObject> Players;

	// Use this for initialization
	void Start () {
		Fire = false;
		Players = new List<GameObject>();
	}

	public void OnCollisionExit2D(Collision2D collision)
	{
		Players.Remove(collision.gameObject);
	}
	
	public void OnCollisionStay2D(Collision2D collision)
	{
		if(Players.Contains(collision.gameObject) == false)
		{
			Players.Add (collision.gameObject);
		}

		if (Fire == true){
			Players.Remove(collision.gameObject);
			Destroy(collision.gameObject);
		}
	}
}
