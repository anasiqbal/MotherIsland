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
	public Ship[] shipPrefab;

	[Header("Waypoints Info")]
	public int maxPointSelectionIndex;
	public List<WaypointsInfo> spawnPoints;

	public static event System.Action OnShipDestroyed;

	[Header("Difficulty")]
	int difficultyIndex = 0;
	float ShipSpawnDelay {
		get
		{
			return maxShipSpawnDelay - ((maxShipSpawnDelay - minShipSpawnDelay) * (difficultyIndex / 100f));
		}
	}
	public float minShipSpawnDelay = 2;
	public float maxShipSpawnDelay = 4;
	int StartShipSelectionIndex {
		get
		{
			if (difficultyIndex >= 20)
			{
				return 1;
			}
			else if (difficultyIndex >= 40)
			{
				return 2;
			}
			else return 0;
			//return 1;
		}
	}
	int EndShipSelectionIndex {
		get
		{
			if (difficultyIndex >= 10)
			{
				return 1;
			}
			else if (difficultyIndex >= 30)
			{
				return 2;
			}
			else if (difficultyIndex >= 50)
			{
				return 3;
			}
			else return 0;

			//return 3;
		}
	}

	List<WaypointsInfo> shuffledSpawnPoints;
	private List<Ship> spawnedShips = new List<Ship>();

	bool isActive;
	WaypointsInfo pointInfo;

	Vector3 targetHitPosition;

	private void OnDisable()
	{
		StopSpawning ();
	}

	public void Initialize()
	{
		isActive = true;
		targetHitPosition = target.position + (Vector3.up * 2);

		difficultyIndex = 0;

		StartCoroutine (SpawnShip ());
	}

	public void StartAI()
	{
		shuffledSpawnPoints = spawnPoints;
		Initialize ();
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

			int shipIndex = Random.Range(StartShipSelectionIndex, EndShipSelectionIndex + 1);

			Ship spawnedShip = Instantiate (shipPrefab[shipIndex], pointInfo.StartingPoint.position, Quaternion.identity, shipParent);
			spawnedShip.Initialize (pointInfo, targetHitPosition);
			spawnedShip.OnDeath += SpawnedShip_OnDeath;

			shuffledSpawnPoints.RemoveAt (waypointIndex);
			shuffledSpawnPoints.Add (pointInfo);

			yield return new WaitForSeconds (ShipSpawnDelay);
		}
	}

	private void SpawnedShip_OnDeath()
	{
		if (OnShipDestroyed != null)
			OnShipDestroyed ();

		difficultyIndex = (difficultyIndex + 1) % 100;
	}
}
