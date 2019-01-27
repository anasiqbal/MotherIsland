using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof (WeaponController))]
public class Player : LivingEntity
{
	#region Member Variables

	[Header ("Visual")]
	public Transform modelParent;
	public GameObject prefab_PlayerModel;

	[Header("Cross Hair Settings")]
	public GameObject crossHair;
	
	// References
	Camera mainCamera;
	private Vector3 point = Vector3.zero;
	WeaponController weaponController;
	GameObject instantiatedPlayer;

	#endregion

	#region Unity Behaviours
	protected override void Start()
	{
		base.Start ();

		mainCamera = Camera.main;
		weaponController = GetComponent<WeaponController> ();
		//SetupPlayerModel ();
	}
	

	void Update()
	{
		if(IsActive && !IsDead)
		{
			// Get aim/ look direction and pass the look point to the controller
			Ray ray = mainCamera.ScreenPointToRay (Input.mousePosition);
			point = Vector3.zero;
			Plane groundPlane = new Plane (Vector3.up, Vector3.zero);
			float rayDistance;

			if (groundPlane.Raycast (ray, out rayDistance))
			{
				point = ray.GetPoint (rayDistance);
				Vector3 mortarDirection = new Vector3 (point.x, transform.position.y, point.z);
				transform.LookAt (mortarDirection);
			}

			crossHair.transform.position = point + (Vector3.up * 0.8f);

			// Trigger weapon to shoot
			if (Input.GetMouseButton (0))
				weaponController.Shoot (point);
		}
	}


	#endregion
	
	#region Inherited Methods

	protected override void Die()
	{
		base.Die ();

	}

	#endregion

	#region Helper Methods
	
	void SetupPlayerModel()
	{
		instantiatedPlayer = Instantiate (prefab_PlayerModel, modelParent);
		instantiatedPlayer.transform.localScale = Vector3.one;
		instantiatedPlayer.transform.localPosition = new Vector3 (0, 1, 0);
	}

	#endregion
}
