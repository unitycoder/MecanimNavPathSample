using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
	public float patrolSpeed = 2f;					// The nav mesh agent's speed when patrolling.
	public float atTargetWaitTime = 1f;				// The amount of time to wait when the patrol way point is reached.
	public Transform[] patrolWayPoints;				// An array of transforms for the patrol route.
	
	NavMeshAgent nav;								// Reference to the nav mesh agent.
	Transform player;								// Reference to the player's transform.
	float chaseTimer;								// A timer for the chaseWaitTime.
	float waitAtTargetTimer;						// A timer for the patrolWaitTime.
	int wayPointIndex=0;							// A counter for the way point array
	
	
	void Awake ()
	{
		nav = GetComponent<NavMeshAgent>();
		InitAgent();
	}
	
	
	void Update ()
	{
		Patrol();
	}

	void Patrol ()
	{
		if(nav.remainingDistance > nav.stoppingDistance) return;

		waitAtTargetTimer += Time.deltaTime;
		if(waitAtTargetTimer <= atTargetWaitTime) return;

		waitAtTargetTimer = 0;
		wayPointIndex = ++wayPointIndex % patrolWayPoints.Length;
		SetNextTarget();
	}

	void InitAgent()
	{
		nav.speed = patrolSpeed;
		SetNextTarget();
	}
	
	void SetNextTarget()
	{
		nav.destination = patrolWayPoints[wayPointIndex].position;
	}

}