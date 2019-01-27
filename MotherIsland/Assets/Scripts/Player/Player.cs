using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;

[RequireComponent (typeof (WeaponController))]
public class Player : LivingEntity
{
	#region Member Variables

	[Header ("Visual")]
	public Transform tower;
	public GameObject prefab_PlayerModel;

	public Image healthBar;

	public float fortDestroyDuration = 4;
	public GameObject destroyEffect;

	[Header("Cross Hair Settings")]
	public GameObject crossHair;

	[Header("Audio Clips")] 
	[SerializeField] private AudioClip structureDestroyed;

	//Private variables
	private float projectionSize;
	
	// References
	Camera mainCamera;
	private Vector3 point = Vector3.zero;
	WeaponController weaponController;

	#endregion

	#region Unity Behaviours
	protected override void Start()
	{
		base.Start ();

		mainCamera = Camera.main;
		weaponController = GetComponent<WeaponController> ();
		mainCamera.orthographicSize = 15;
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
			if (Input.GetMouseButton(0))
			{
				weaponController.Shoot (point);
				mainCamera.orthographicSize = 16;
			}

			mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, 15f, Time.deltaTime * 2.5f);


		}
	}

	#endregion

	#region Inherited Methods

	public override void TakeDamage(float damage)
	{
		base.TakeDamage(damage);
		healthBar.fillAmount = health / startingHealth;
	}

	protected override void Die()
	{
		base.Die();
		destroyEffect.SetActive(true);
		tower.DOMoveY(-6, fortDestroyDuration).SetAutoKill(true).OnComplete(() =>
		{
			destroyEffect.SetActive(false);
			Gamemanager.Manager.TransitionGameState(Gamemanager.States.GAMEOVER);
		}).Play();
		mainCamera.DOShakePosition(fortDestroyDuration, 0.5f, 40, fadeOut: false);
		
		//Sound Effect.
		GameObject audio = new GameObject("Structure Destroyed Sound");
		AudioSource source = audio.AddComponent<AudioSource>();
		source.PlayOneShot(structureDestroyed);

	}

		#endregion
	}
