using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
	public float LifeDuration = 2;
	public Vector3 Direction = Vector3.forward;
	public float Speed = 10;
	private float _spawnTime;
	private VolumetricLines.VolumetricLineBehavior _lazer;

	// Use this for initialization
	void Start () {
		_spawnTime = Time.time;
	}

	void Awake(){
		_lazer = GetComponentInChildren<VolumetricLines.VolumetricLineBehavior> ();
		_lazer.LineWidth = 0.02f;
	}

	// Update is called once per frame
	void Update () {
		_lazer.LineWidth = 0.02f;

		if (Time.time - _spawnTime > LifeDuration) {
			Destroy (gameObject);
		}

		transform.Translate (Direction * Speed * Time.deltaTime, Space.Self);
	}
}