using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
	public float damage = 5;

	public float maxLifeTime = 6;
	Rigidbody rb;

	private void Awake()
	{
		rb = GetComponent<Rigidbody> ();
	}
	private void Start()
	{
		Destroy (gameObject, maxLifeTime);
	}

	public void ApplyForce(Vector3 force)
	{
		rb.AddForce (force, ForceMode.VelocityChange);
	}

	private void OnTriggerEnter(Collider other)
	{
		IDamageable hitObject = other.gameObject.GetComponent<IDamageable> ();
		if (hitObject != null)
		{
			hitObject.TakeDamage (damage);
		}

		Destroy (gameObject);
	}

	//public void configureProjectile(Vector3 _targetPosition)
	//{
	//	targetPosition = _targetPosition;
	//	bTargetReady = true;
	//	bTouchingGround = false;
	//	initialPosition = transform.position;
	//	initialRotation = transform.rotation;
	//	gameObject.SetActive(true);
	//}

	//void Update ()
	//{
	//	if (bTargetReady)
	//	{
	//		Launch();
	//	}

	//	if (!bTouchingGround && !bTargetReady)
	//	{
	//		transform.rotation = Quaternion.LookRotation(rigid.velocity) * initialRotation;
	//	}
	//}
	//private void OnCollisionEnter(Collision collision)
	//{
	//	bTouchingGround = true;
	//	gameObject.SetActive(false);
	//	Destroy(this.gameObject,1.5f);
	//}

	//void Launch()
	//{
	//	Vector3 projectileXZPos = new Vector3(transform.position.x, 0.0f, transform.position.z);
	//	Vector3 targetXZPos = new Vector3(targetPosition.x, 0.0f, targetPosition.z);

	//	transform.LookAt(targetXZPos);

	//	float R = Vector3.Distance(projectileXZPos, targetXZPos);
	//	float G = Physics.gravity.y;
	//	float tanAlpha = Mathf.Tan(LaunchAngle * Mathf.Deg2Rad);
	//	float H = (targetPosition.y) - transform.position.y;

	//	float Vz = Mathf.Sqrt(G * R * R / (2.0f * (H - R * tanAlpha)) );
	//	float Vy = tanAlpha * Vz;

	//	Vector3 localVelocity = new Vector3(0f, Vy, Vz);
	//	Vector3 globalVelocity = transform.TransformDirection(localVelocity);

	//	rigid.velocity = globalVelocity;
	//	bTargetReady = false;
	//}
}
