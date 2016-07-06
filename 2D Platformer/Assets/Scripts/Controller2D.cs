using UnityEngine;
using System.Collections;

[RequireComponent (typeof (BoxCollider2D))]
public class Controller2D : MonoBehaviour {

	#region Variables
	public LayerMask collisionMask;

	const float skinWidth = 0.015f;
	public int hRayCount = 4;
	public int vRayCount = 4;

	float hRaySpacing;
	float vRaySpacing; 
	
	BoxCollider2D pCollider;
	RayCastOrigins rayCastOrigins;

	public CollisionInfo collisions; 
	#endregion
	void Start () {
		pCollider = GetComponent<BoxCollider2D> ();
		CalculateRaySpacing ();

	}	

	public void Move(Vector3 velocity)
	{
		UpdateRayCastOrigins ();
		collisions.Reset ();
		if (velocity.x != 0) {
			HCollisions(ref velocity);
		}
		if (velocity.y != 0) {
			VCollisions (ref velocity);
		}


		transform.Translate (velocity);
	}

	public void HCollisions(ref Vector3 velocity){
		float directionX  = Mathf.Sign (velocity.x);
		float rayLength = Mathf.Abs (velocity.x) + skinWidth;
		
		
		
		for (int i = 0; i < hRayCount; i++) {
			Vector2 rayOrigin = (directionX == -1)?rayCastOrigins.bottomLeft:rayCastOrigins.bottomRight;
			rayOrigin += Vector2.up * (hRaySpacing * i);
			RaycastHit2D hit = Physics2D.Raycast (rayOrigin, Vector2.right * directionX, rayLength, collisionMask);
			
			Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);
			
			if(hit){
				velocity.x = (hit.distance - skinWidth) * directionX; 
				rayLength = hit.distance;

				collisions.left = directionX == -1;
				collisions.right = directionX == 1;
				string temp = collisionMask.ToString();
				//Debug.Log (temp);
			}
		}
	}



	public void VCollisions(ref Vector3 velocity){
		float directionY = Mathf.Sign (velocity.y);
		float rayLength = Mathf.Abs (velocity.y) + skinWidth;



		for (int i = 0; i < vRayCount; i++) {
			Vector2 rayOrigin = (directionY == -1)?rayCastOrigins.bottomLeft:rayCastOrigins.topLeft;
			rayOrigin += Vector2.right * (vRaySpacing * i + velocity.x);
			RaycastHit2D hit = Physics2D.Raycast (rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

			Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.blue);

			if(hit){
				velocity.y = (hit.distance - skinWidth) * directionY; 
				rayLength = hit.distance;

				
				collisions.below = directionY == -1;
				collisions.above = directionY == 1;

			}
		}
	}



	void UpdateRayCastOrigins()
	{
		Bounds bounds = pCollider.bounds;
		bounds.Expand (skinWidth * -2);

		rayCastOrigins.bottomLeft = new Vector2 (bounds.min.x, bounds.min.y);
		rayCastOrigins.bottomRight = new Vector2 (bounds.max.x, bounds.min.y);
		rayCastOrigins.topLeft = new Vector2 (bounds.min.x, bounds.max.y);
		rayCastOrigins.topRight = new Vector2 (bounds.max.x, bounds.max.y);
	}

	void CalculateRaySpacing(){
		Bounds bounds = pCollider.bounds;
		bounds.Expand (skinWidth * -2);

		hRayCount = Mathf.Clamp (hRayCount, 2, int.MaxValue);
		vRayCount = Mathf.Clamp (vRayCount, 2, int.MaxValue);

		hRaySpacing = bounds.size.y / (hRayCount - 1);
		vRaySpacing = bounds.size.x / (vRayCount - 1);
	}

	struct RayCastOrigins {
		public Vector2 topLeft, topRight;
		public Vector2 bottomLeft, bottomRight; 
	}

	public struct CollisionInfo {
		public bool above, below;
		public bool left, right;
		public int collMask;

		public void Reset(){
			above = below = false;
			left = right = false;
			collMask = -1;
		}

	}
	

}
