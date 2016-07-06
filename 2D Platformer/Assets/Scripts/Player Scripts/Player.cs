using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Controller2D))]
[RequireComponent (typeof (PhysicsTest))]
public class Player : MonoBehaviour {

	#region Variables

	public float jumpHeight = 4; //Number of units player can jump 
	public float timeToJumpApex = 0.4f; // Time taken to reach height
	public float moveSpeed = 6;

	float jumpVel;
	float gravity;
	Vector3 velocity;

	public int playerHealth = 100;
	public bool isAlive;


	Controller2D controller; 
	#endregion




	void Start () {
		controller = GetComponent<Controller2D> ();

		gravity = -(2 * jumpHeight) / Mathf.Pow (timeToJumpApex, 2);
		jumpVel = Mathf.Abs(gravity) * timeToJumpApex;
	}


	void Update(){

		if (controller.collisions.above || controller.collisions.below) {
			velocity.y = 0;
		}
		Vector2 input = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));

		if ((Input.GetKeyDown (KeyCode.Space) || Input.GetButtonDown("Jump"))&& controller.collisions.below) {
			velocity.y = jumpVel;
		}

        if (controller.collisions.right && controller.collisions.collMask == 10)
        {
            playerHealth -= 5;
            Debug.Log(playerHealth);
            if (playerHealth == 0)
            {
                isAlive = false;
            }
        }

        if (controller.collisions.right && controller.collisions.collMask == 12)
        {
            velocity = new Vector3(0, 0, 0);
        }

		if (!isAlive) {
			Respawn();
		}
        
		velocity.x = input.x * moveSpeed;
		velocity.y += gravity * Time.deltaTime;
		controller.Move (velocity * Time.deltaTime);
	}

	void Respawn()
	{

	}

}
