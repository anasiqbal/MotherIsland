using System.Collections;
using UnityEngine;

using DG.Tweening;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class Ship : LivingEntity
{
	public float speed = 5;
	public float stopingDistance = 1;
	public float rotateDuration = 1;

	public Image healthBar;
	public float sinkDuration = 0.75f;

	[Header("Attack")]
	public Transform muzzle;
	public Projectile projectile;

	public float timeBetweenShots;  // in seconds
	public float muzzleVelocity;

	public ProjectilePath projectilePath = ProjectilePath.Average;
	Vector3 targetHitPosition;

	[Header("Path")]
	public WaypointsInfo waypoints;
	int targetWaypoint;
	Vector3 direction;

	bool isPlayerDead = false;

	bool hasReachedDestination = false;
	float startTime;

	Rigidbody rb;

	private void Awake()
	{
		rb = GetComponent<Rigidbody> ();
	}

	protected override void Start()
	{
		base.Start ();

		healthBar.gameObject.SetActive (true);
		healthBar.color = Color.green;
	}

	private void Update()
	{
		if(IsActive && !IsDead && !hasReachedDestination)
		{
			direction = waypoints.waypoints [targetWaypoint].position - transform.position;
			direction.y = transform.position.y;

			if (direction.magnitude < stopingDistance)
			{
				if (targetWaypoint < waypoints.waypoints.Count - 1)
				{
					targetWaypoint++;
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

	public void Initialize(WaypointsInfo points, Vector3 targetHitPoint)
	{
		targetWaypoint = 1;
		waypoints = points;

		targetHitPosition = targetHitPoint;

		hasReachedDestination = false;
		base.Initialize ();
	}

	public override void TakeDamage(float damage)
	{
		base.TakeDamage (damage);

		healthBar.fillAmount = health / startingHealth;
		if(health/ startingHealth < 0.3f)
		{
			healthBar.color = Color.red;
		}
	}

	protected override void Die()
	{
		base.Die ();

		rb.isKinematic = true;
		GetComponent<Collider> ().enabled = false;
		healthBar.gameObject.SetActive (false);

		transform.DOMoveY (-2, sinkDuration).SetAutoKill (true).OnComplete(() =>
		{
			Destroy (gameObject);
		}).Play ();
	}

	public void StopAttack()
	{
		isPlayerDead = true;
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

		StartCoroutine(Attack());
	}

	IEnumerator Attack()
	{
		Vector3 force = CalculateAngle(targetHitPosition);
		while (!IsDead && !isPlayerDead)
		{
			Projectile instantiatedProjectile = Instantiate(projectile, muzzle.position, muzzle.rotation);
			instantiatedProjectile.ApplyForce(force);

			yield return new WaitForSeconds(timeBetweenShots);
		}
	}

	Vector3 CalculateAngle(Vector3 targetPosition)
	{
		Vector3 toTarget = targetPosition - transform.position;

		// Set up the terms we need to solve the quadratic equations.
		float gSquared = Physics.gravity.sqrMagnitude;
		float b = (muzzleVelocity * muzzleVelocity) + Vector3.Dot(toTarget, Physics.gravity);
		float discriminant = (b * b) - (gSquared * toTarget.sqrMagnitude);

		// Check whether the target is reachable at max speed or less.
		if (discriminant < 0)
		{
			// Target is too far away to hit at this speed.
			// Abort, or fire at max speed in its general direction?
		}

		float discRoot = Mathf.Sqrt(discriminant);

		// Highest shot with the given max speed:
		float T_max = Mathf.Sqrt((b + discRoot) * 2f / gSquared);

		// Most direct shot with the given max speed:
		float T_min = Mathf.Sqrt((b - discRoot) * 2f / gSquared);

		// Lowest-speed arc available:
		float T_lowEnergy = Mathf.Sqrt(Mathf.Sqrt(toTarget.sqrMagnitude * 4f / gSquared));

		float T = T_max;// choose T_max, T_min, or some T in-between like T_lowEnergy

		switch (projectilePath)
		{
			case ProjectilePath.High:
				T = T_max;
				break;
			case ProjectilePath.Average:
				T = T_lowEnergy;
				break;
			case ProjectilePath.Low:
				T = T_min;
				break;
		}

		// Convert from time-to-hit to a launch velocity:
		Vector3 velocity = (toTarget / T) - (Physics.gravity * T / 2f);

		// Apply the calculated velocity (do not use force, acceleration, or impulse modes)
		return velocity;
	}
}
