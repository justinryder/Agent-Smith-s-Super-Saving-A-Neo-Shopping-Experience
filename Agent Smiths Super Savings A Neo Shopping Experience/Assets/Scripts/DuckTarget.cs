using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckTarget : MonoBehaviour
{
    public Vector3 MoveDirection = Vector3.right;
    public float MoveSpeed = 2;
    private float _lifeTime = 0;
    private Vector3 _startPosition;

    void Start()
    {
        _startPosition = transform.position;
        _lifeTime = Random.value;
    }
    
    void Update()
    {
        _lifeTime += Time.deltaTime;
        transform.position = _startPosition + transform.TransformVector(MoveDirection * Mathf.Sin(_lifeTime * MoveSpeed));
    }
}