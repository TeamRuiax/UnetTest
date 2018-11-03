using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
public class health : NetworkBehaviour {

	public const int maxHealth = 100;
	public bool destoryOnDeath;
	[SyncVar(hook = "OnChangeHealth")]

	public int currentHealth = maxHealth;

	public Slider healthSlider;

	private NetworkStartPosition[] spawnPoints;

	void Start()
	{
		if(isLocalPlayer)
		{
			spawnPoints = FindObjectsOfType<NetworkStartPosition> ();
		}
	}

	public void TakeDamage(int amount)
	{
		if (!isServer) 
		{
			return;
		}

		currentHealth -= amount;
		if (currentHealth <= 0) 
		{
			if (destoryOnDeath) {
				Destroy (gameObject);
			} else {
				currentHealth = maxHealth;
				RpcRespawn ();
				//RPC; 서에서 발동하면->모든클라이언트에서 발동
			}
		}
	}

	void OnChangeHealth(int health)
	{
		healthSlider.value = health;
	}

	[ClientRpc]
	void RpcRespawn()
	{
		if (isLocalPlayer)
		{
			Vector3 spawnPoint = Vector3.zero;
			if (spawnPoints != null && spawnPoints.Length > 0) 
			{
				spawnPoint = spawnPoints [Random.Range (0, spawnPoints.Length)].transform.position;
			}
			transform.position = spawnPoint;
		}

	}
}