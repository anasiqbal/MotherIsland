using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct WaypointsInfo
{
	public List<Transform> waypoints;

	public Transform StartingPoint
	{
		get { return waypoints [0]; }
	}
}

public class ShipController : MonoBehaviour
{
	public Transform target;

	public Transform shipParent;
	public Ship shipPrefab;

	public float shipSpawnDelay = 1;

	[Header("Waypoints Info")]
	public int maxPointSelectionIndex;
	public List<WaypointsInfo> spawnPoints;
	

	public event System.Action OnShipDestroyed;

	List<WaypointsInfo> shuffledSpawnPoints;
	private List<Ship> spawnedShips = new List<Ship>();

	bool isActive;
	WaypointsInfo pointInfo;

	Vector3 targetHitPosition;
	
	//References
	private AudioSource audioSource;

	[Header("Audio Clips")] 
	[SerializeField] private AudioClip shipSpawn;


	private void Start()
	{
		audioSource = GetComponent<AudioSource>();
	}

	private void OnDisable()
	{
		StopSpawning ();
	}

	public void Initialize()
	{
		isActive = true;
		targetHitPosition = target.position + (Vector3.up * 2);
		StartCoroutine (SpawnShip ());
	}

	public void StartAI()
	{
		shuffledSpawnPoints = spawnPoints;
		Initialize ();
		Debug.LogError("AI Started");
	}
	
	public void RemoveOnScreenShips()
	{
		spawnedShips.Clear();
		foreach (Transform children in shipParent)
		{
			Destroy(children.gameObject);
		}
	}

	public void StopSpawning()
	{
		isActive = false;
	}

	IEnumerator SpawnShip()
	{
		while(isActive)
		{
			int waypointIndex = Random.Range (0, maxPointSelectionIndex);
			pointInfo = shuffledSpawnPoints [waypointIndex];

			Ship spawnedShip = Instantiate (shipPrefab, pointInfo.StartingPoint.position, Quaternion.identity, shipParent);
			spawnedShip.Initialize (pointInfo, targetHitPosition);
			audioSource.PlayOneShot(shipSpawn);
			spawnedShip.OnDeath += SpawnedShip_OnDeath;

			shuffledSpawnPoints.RemoveAt (waypointIndex);
			shuffledSpawnPoints.Add (pointInfo);

			yield return new WaitForSeconds (shipSpawnDelay);
		}
	}

	private void SpawnedShip_OnDeath()
	{
		if (OnShipDestroyed != null)
			OnShipDestroyed ();
	}
}
