using UnityEngine;
using System.Collections;

public class EnemyAnimation : MonoBehaviour
{
	public float deadZone = 5f;					// The number of degrees for which the rotation isn't controlled by Mecanim.
	
	
	Transform player;					// Reference to the player's transform.
	NavMeshAgent nav;					// Reference to the nav mesh agent.
	Animator anim;						// Reference to the Animator.
	HashIDs hash;					// Reference to the HashIDs script.
	AnimatorSetup animSetup;		// An instance of the AnimatorSetup helper class.
	float speed;
	float angle;


	void Awake ()
	{
		nav = GetComponent<NavMeshAgent>();
		anim = GetComponent<Animator>();
		hash = GameObject.FindGameObjectWithTag("GameController").GetComponent<HashIDs>();
		
		nav.updateRotation = false;
		animSetup = new AnimatorSetup(anim, hash);
		deadZone *= Mathf.Deg2Rad;
	}
	
	
	void Update () 
	{
		NavAnimationUpdate();
	}
	
	
	void OnAnimatorMove()
    {
		// Set the NavMeshAgent's velocity to the change in position since the last frame, by the time it took for the last frame.
        nav.velocity = anim.deltaPosition / Time.deltaTime;
		transform.rotation = anim.rootRotation;
    }
	
	
	void NavAnimationUpdate ()
	{

		speed = Vector3.Project(nav.desiredVelocity, transform.forward).magnitude;
		
		// angle is the angle between forward and the desired velocity.
		angle = FindAngle(transform.forward, nav.desiredVelocity, transform.up);
		
		// If the angle is within the deadZone
		// set the direction to be along the desired direction and set the angle to be zero.
		if(Mathf.Abs(angle) < deadZone) 
		{
			transform.LookAt(transform.position + nav.desiredVelocity);
			angle = 0f;
		}

		animSetup.Setup(speed, angle);
	}
	
	
	float FindAngle (Vector3 fromVector, Vector3 toVector, Vector3 upVector)
	{
		// If the vector the angle is being calculated to is 0...
		if(toVector == Vector3.zero) return 0f;	// ... the angle between them is 0.

		// Create a float to store the angle between the facing of the enemy and the direction it's travelling.
		float angle = Vector3.Angle(fromVector, toVector);
		
		// Find the cross product of the two vectors (this will point up if the velocity is to the right of forward).
		Vector3 normal = Vector3.Cross(fromVector, toVector);
		
		// The dot product of the normal with the upVector will be positive if they point in the same direction.
		angle *= Mathf.Sign(Vector3.Dot(normal, upVector));
		
		// We need to convert the angle we've found from degrees to radians.
		angle *= Mathf.Deg2Rad;

		return angle;
	}
}
