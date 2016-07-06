using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Controller2D))]
[RequireComponent (typeof (PhysicsTest))]
public class EnemyAI : MonoBehaviour {

	Controller2D eController;
	public float moveSpeed;
	public Vector3 velocity;
	public float gravity = 10;
	private bool seePlayer, chasing;
    private int aiState;
    public int enemyLvl; 
 

	// Use this for initialization
	void Start () {
		eController = GetComponent<Controller2D> ();
        
        if (enemyLvl == 0)
        {
            aiState = 0;
        }
        else
        {
            aiState = 4;
        }

   
	}
	
	// Update is called once per frame
	void Update () 
    {           
        checkCollision();
        int curState = aiState;
        if (curState == 0)
        {
            // Do nothing
        }
        if (curState == 4) // Patrol
        {
            velocity.x = moveSpeed;
            velocity.y -= gravity * Time.deltaTime;
            eController.Move(velocity * Time.deltaTime);
        }
        

	}

    void checkCollision()
    {
        if (eController.collisions.above || eController.collisions.below)
        {
            velocity.y = 0;

            if (eController.collisions.right)
            {
                moveSpeed = -3;
                velocity.x = moveSpeed;
            }
            if (eController.collisions.left)
            {
                moveSpeed = 3;
                velocity.x = moveSpeed;
            }
        }
    }


    /*
     * Chase Player
     * Reset to patrol
     * Attack
     */
}
