using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class enemySpawner : NetworkBehaviour {

	public GameObject enemyPrefabs;
	public int numberOfEnenies;
	// Use this for initialization

	public override void OnStartServer()
	{
		for (int i = 0; i < numberOfEnenies; i++) 
		{
			var spawnPosition = new Vector3 (
				                   Random.Range (-8f, 8f),
				                   0,
				                   Random.Range (-8f, 8f)
			                   );
			var spawnRotation = Quaternion.Euler (new Vector3 (0.0f, Random.Range (0, 100), 0.0f));
			var enemy = (GameObject)Instantiate (enemyPrefabs, spawnPosition, spawnRotation);

			NetworkServer.Spawn (enemy);
		}
	}

}
