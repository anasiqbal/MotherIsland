using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (WeaponController))]
public class Player : LivingEntity
{
	#region Member Variables

	[Header ("Visual")]
	public Transform modelParent;
	public GameObject prefab_PlayerModel;

	// References
	Camera mainCamera;
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

		IsDead = false;
		IsActive = true;
	}

	void Update()
	{
		if(IsActive && !IsDead)
		{
			// Get aim/ look direction and pass the look point to the controller
			Ray ray = mainCamera.ScreenPointToRay (Input.mousePosition);
			Plane groundPlane = new Plane (Vector3.up, Vector3.zero);
			float rayDistance;

			if (groundPlane.Raycast (ray, out rayDistance))
			{
				Vector3 point = ray.GetPoint (rayDistance);
				point.y = transform.position.y;
				transform.LookAt (point);
			}

			// Trigger weapon to shoot
			if (Input.GetMouseButton (0))
				weaponController.Shoot ();
		}
	}

	#endregion

	#region Inherited Methods
	public override void Initialize()
	{
		base.Initialize ();
	}

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
