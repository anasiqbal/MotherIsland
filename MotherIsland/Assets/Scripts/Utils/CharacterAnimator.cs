using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class CharacterAnimator : MonoBehaviour {

	public float cheerDuration;
	public List<Transform> characters;
	
	private AudioSource cheeringSource;

	[Header("Audio Clips")] 
	[SerializeField] private AudioClip cheering;
	
	private void OnEnable()
	{
		ShipController.OnShipDestroyed += ShipController_OnShipDestroyed;
		cheeringSource = GetComponent<AudioSource>();
	}

	private void ShipController_OnShipDestroyed()
	{
		cheeringSource.PlayOneShot(cheering);
		for (int i = 0; i < characters.Count; i++)
		{
			characters[i].DOPunchPosition(Vector3.up * 0.5f, cheerDuration / 3f).SetLoops(3, LoopType.Yoyo).SetDelay(i * 0.1f).Play();
		}
	}

	private void OnDisable()
	{
		ShipController.OnShipDestroyed += ShipController_OnShipDestroyed;
	}
}
