using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
	public LayerMask collisionMask;
	float speed;
	float damage = 1;

	float lifeTime = 2;
	float collisionBufferDistance = 0.1f;

	#region Unity Methods
	void Start()
	{
		Destroy (gameObject, lifeTime);

		// bullet can spawn inside the enemy, so check for collisions
		Collider [] initialCollisions = Physics.OverlapSphere (transform.position, 0.1f, collisionMask);
		if(initialCollisions.Length > 0)
		{
			OnHitObject (initialCollisions [0]);
		}
	}

	void Update ()
	{
		// Collision detection
		float moveDistance = speed * Time.deltaTime;
		CheckCollisions (moveDistance);

		// movement
		transform.Translate (Vector3.forward * moveDistance);
	}

	#endregion

	#region Helper Methods
	public void SetSpeed(float _speed)
	{
		speed = _speed;
	}

	void CheckCollisions(float moveDistance)
	{
		Ray ray = new Ray (transform.position, transform.forward);
		RaycastHit hit;
		
		// both bullet and enemy are moving therefore adding buffer distance to collision detecction to compensate for enemy movement
		if(Physics.Raycast(ray, out hit, moveDistance + collisionBufferDistance, collisionMask, QueryTriggerInteraction.Collide))
		{
			OnHitObject (hit);
		}
	}

	void OnHitObject(RaycastHit hit)
	{
		IDamageable objectHit = hit.collider.GetComponent<IDamageable> ();
		if (objectHit != null)
			objectHit.TakeHit (damage, hit);

		Destroy (gameObject);
	}

	void OnHitObject(Collider collider)
	{
		IDamageable objectHit = collider.GetComponent<IDamageable> ();
		if (objectHit != null)
			objectHit.TakeDamage (damage);

		Destroy (gameObject);
	}

	#endregion
}
