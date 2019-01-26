using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
	public Transform muzzle;
	public Projectile projectile;

	public float timeBetweenShots;	// in seconds
	public float muzzleVelocity;

	float nextShotTime;

	public void Shoot()
	{
		if(Time.time >= nextShotTime)
		{
			nextShotTime = Time.time + timeBetweenShots;

			Projectile instantiatedProjectile = Instantiate (projectile, muzzle.position, muzzle.rotation);
			instantiatedProjectile.SetSpeed (muzzleVelocity);
		}
	}

}
