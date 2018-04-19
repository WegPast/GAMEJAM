using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pool<T> : MonoBehaviour {

	protected List<T> pool;

	protected abstract void Execute ();
	
	// Update is called once per frame
	void Update () {
		Execute ();
	}

	public void add(T newPlayer) {
		if (pool == null) {
			pool = new List<T>();
		}
		pool.Add (newPlayer);
	}
}
