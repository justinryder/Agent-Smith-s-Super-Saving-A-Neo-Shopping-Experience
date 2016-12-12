using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
  void OnDrawGizmos()
  {
    var previousColor = Gizmos.color;

    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(transform.position, 0.1f);

    Gizmos.color = previousColor;
  }

  public GameObject Spawn(GameObject prefab)
  {
    var instance = Instantiate(prefab);
    instance.transform.position = transform.position;
    instance.transform.rotation = transform.rotation;

    var duckTarget = instance.GetComponent<DuckTarget>();
    if (duckTarget)
    {
      duckTarget.MoveDirection *= 2;
    }

    return instance;
  }
}