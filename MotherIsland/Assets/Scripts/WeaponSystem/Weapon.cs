using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Weapon : MonoBehaviour
{
	public Transform muzzle;
	public Transform shootPoint;
	public Projectile projectile;

	public float timeBetweenShots;	// in seconds
	public float muzzleVelocity;

	bool canShoot=true;
	
	//UI References
	public Image loading;

	public void Shoot(Vector3 _target)
	{
		if(canShoot)
		{
			canShoot = false;

			loading.fillAmount = 0;
			loading.color = Color.white;

			loading.DOFillAmount(1f, timeBetweenShots).OnComplete(() =>
			{
				loading.DOColor(Color.green, timeBetweenShots).OnComplete(() =>
				{
					canShoot = true;
				});
			});

			Projectile instantiatedProjectile = Instantiate (projectile, shootPoint.position, shootPoint.rotation);
			instantiatedProjectile.configureProjectile(_target);
			//instantiatedProjectile.SetSpeed (muzzleVelocity);
		}
	}

}
