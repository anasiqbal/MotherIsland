using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[System.Serializable]
public enum ProjectilePath
{
	High,
	Average,
	Low,
}

public class Weapon : MonoBehaviour
{
	public Transform muzzle;
	public Projectile projectile;

	public float timeBetweenShots;	// in seconds
	public float muzzleVelocity;

	public ProjectilePath projectilePath = ProjectilePath.Average;

	bool canShoot = true;
	Vector3 targetPosition;

	//UI References
	public Image loading;

	public void Shoot(Vector3 _target)
	{
		targetPosition = _target;
		if(canShoot)
		{
			canShoot = false;

			loading.gameObject.SetActive (true);
			loading.fillAmount = 0;
			loading.color = Color.white;

			loading.DOFillAmount(1f, timeBetweenShots).OnComplete(() =>
			{
				loading.gameObject.SetActive (false);
				canShoot = true;
			});

			Projectile instantiatedProjectile = Instantiate (projectile, muzzle.position, muzzle.rotation);
			instantiatedProjectile.ApplyForce (CalculateAngle (_target));
		}
	}

	Vector3 CalculateAngle(Vector3 targetPosition)
	{
		Vector3 toTarget = targetPosition - transform.position;

		// Set up the terms we need to solve the quadratic equations.
		float gSquared = Physics.gravity.sqrMagnitude;
		float b = (muzzleVelocity * muzzleVelocity) + Vector3.Dot (toTarget, Physics.gravity);
		float discriminant = (b * b) - (gSquared * toTarget.sqrMagnitude);

		// Check whether the target is reachable at max speed or less.
		if (discriminant < 0)
		{
			// Target is too far away to hit at this speed.
			// Abort, or fire at max speed in its general direction?
		}

		float discRoot = Mathf.Sqrt (discriminant);

		// Highest shot with the given max speed:
		float T_max = Mathf.Sqrt ((b + discRoot) * 2f / gSquared);

		// Most direct shot with the given max speed:
		float T_min = Mathf.Sqrt ((b - discRoot) * 2f / gSquared);

		// Lowest-speed arc available:
		float T_lowEnergy = Mathf.Sqrt (Mathf.Sqrt (toTarget.sqrMagnitude * 4f / gSquared));

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
