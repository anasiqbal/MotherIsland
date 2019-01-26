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
	public Transform shipParent;
	public Ship shipPrefab;

	public float shipSpawnDelay = 1;

	[Header("Waypoints Info")]
	public int maxPointSelectionIndex;
	public List<WaypointsInfo> spawnPoints;

	List<WaypointsInfo> shuffledSpawnPoints;

	bool isActive;
	WaypointsInfo pointInfo;

	private void Start()
	{
		shuffledSpawnPoints = spawnPoints;
		Initialize ();
	}

	private void OnDisable()
	{
		StopSpawning ();
	}

	public void Initialize()
	{
		isActive = true;
		StartCoroutine (SpawnShip ());
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
			spawnedShip.Initialize (pointInfo);

			shuffledSpawnPoints.RemoveAt (waypointIndex);
			shuffledSpawnPoints.Add (pointInfo);

			yield return new WaitForSeconds (shipSpawnDelay);
		}
	}
}
