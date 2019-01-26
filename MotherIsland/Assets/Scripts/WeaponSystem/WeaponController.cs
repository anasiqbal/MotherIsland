using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
	#region Member Variables
	public Transform weaponHold;
	public Weapon startignWeapon;
	Weapon equipedWeapon;

	#endregion

	#region Unity Methods
	private void Start()
	{
		if (startignWeapon != null)
			EquipWeapon (startignWeapon);
	}

	#endregion

	#region Helper Methods
	public void EquipWeapon(Weapon weapon)
	{
		if (equipedWeapon != null)
			Destroy (equipedWeapon.gameObject);

		equipedWeapon = Instantiate(weapon, weaponHold.position, weaponHold.rotation, weaponHold);
	}

	public void Shoot()
	{
		if (equipedWeapon != null)
			equipedWeapon.Shoot ();
	}

	#endregion
}
