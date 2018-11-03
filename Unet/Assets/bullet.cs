using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class bullet : MonoBehaviour {


	// Use this for initialization
	void Start () {
		
	}
	void OnCollisionEnter(Collision collison)
	{
		var hit = collison.gameObject;
		var health = hit.GetComponent<health> ();

		if (health != null) {
			health.TakeDamage (10);
		}
		Destroy(gameObject);
	}
	// Update is called once per frame
	void Update () {
		
	}
}
