using UnityEngine;
using System.Collections;

public class CharacterMovement : MonoBehaviour {

	public float playerSpeed;
	public float jump_force;
	public Vector2 jump_vector = new Vector2(1,1);
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		float movementVert = Input.GetAxisRaw ("LeftJoystickY1") * playerSpeed * Time.deltaTime;
		float movementHoriz = Input.GetAxisRaw ("RightJoystickX1") * playerSpeed * Time.deltaTime;

		transform.Translate(Vector2.up *movementVert);
		transform.Translate(Vector2.right *movementHoriz);
		if (Input.GetKeyDown ("space")){

			//jump, GOAL: no control for a bit then only left and right
			rigidbody2D.AddForce (jump_vector * jump_force);
			rigidbody2D.gravityScale = 1;
			Debug.Log ("jumped");
			Debug.Log ("gravity = " + rigidbody2D.gravityScale);
		} 
	}
	//preset this shit
	void OnTriggerEnter2D (Collider2D other){
		Debug.Log ("collided");
		if(other.tag == "WallLeft"){
			jump_vector = new Vector2(1,1);
			rigidbody2D.velocity = new Vector2(0,0);
			rigidbody2D.gravityScale = 0;
			Debug.Log ("Set grav to 0, gravity = " + rigidbody2D.gravityScale);

		}
		if(other.tag == "WallRight"){
			jump_vector = new Vector2(-1,1);
			rigidbody2D.velocity = new Vector2(0,0);
			rigidbody2D.gravityScale = 0;
			Debug.Log ("Set grav to 0, gravity = " + rigidbody2D.gravityScale);
			
		}
	}
}
