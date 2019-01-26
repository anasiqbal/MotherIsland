using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
	#region Member Variables
	//public Transform weaponHold;
	//public Weapon startingWeapon;
	public Weapon equipedWeapon;

	#endregion

	#region Unity Methods
	private void Start()
	{
		//if (startingWeapon != null)
		//	EquipWeapon (startignWeapon);
	}

	#endregion

	#region Helper Methods
/*	public void EquipWeapon(Weapon weapon)
	{
		if (equipedWeapon != null)
			Destroy (equipedWeapon.gameObject);

		equipedWeapon = Instantiate(weapon, weaponHold.position, weaponHold.rotation, weaponHold);
	}
*/
	public void Shoot(Vector3 _target)
	{
		if (equipedWeapon != null)
			equipedWeapon.Shoot (_target);
	}

	#endregion
}
