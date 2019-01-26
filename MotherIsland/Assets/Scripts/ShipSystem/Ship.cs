using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Ship : LivingEntity
{
	public float speed = 5;
	public float stopingDistance = 1;
	public float rotateDuration = 1;

	public WaypointsInfo waypoints;
	int targetPoint;
	Vector3 direction;

	bool hasReachedDestination = false;
	float startTime;

	Rigidbody rb;

	private void Awake()
	{
		rb = GetComponent<Rigidbody> ();
	}

	private void Update()
	{
		if(IsActive && !IsDead && !hasReachedDestination)
		{
			direction = waypoints.waypoints [targetPoint].position - transform.position;
			direction.y = transform.position.y;

			if (direction.magnitude < stopingDistance)
			{
				if (targetPoint < waypoints.waypoints.Count - 1)
				{
					targetPoint++;
				}
				else
				{
					// Ship has reached the destination
					hasReachedDestination = true;
					rb.velocity = Vector3.zero;
					startTime = Time.time;

					StartCoroutine (PrepareToAttack ());
				}
			}

			if(!hasReachedDestination)
			{
				direction = direction.normalized;

				transform.forward = direction;
				rb.velocity = direction * speed;
			}
		}
	}

	public void Initialize(WaypointsInfo points)
	{
		targetPoint = 1;
		waypoints = points;

		hasReachedDestination = false;
		base.Initialize ();
	}

	IEnumerator PrepareToAttack()
	{
		float angle = 0;
		float startingAngle = transform.rotation.eulerAngles.y;
		int rotationDirection = Random.Range (-1, 1) > 0 ? 1 : -1;

		float addedAngle = 75 * rotationDirection;

		while(true)
		{
			float lerpValue = (Time.time - startTime) / rotateDuration;
			angle = Mathf.Lerp (0, addedAngle, lerpValue);
			transform.rotation = Quaternion.Euler (new Vector3(0,  startingAngle + angle, 0));

			if (lerpValue >= 1)
				break;

			yield return null;
		}
	}
}
