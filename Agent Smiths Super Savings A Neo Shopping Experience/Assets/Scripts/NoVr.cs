using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoVr : MonoBehaviour
{
    void Start()
    {
        Debug.Log("NoVr script Start");
        if (SteamVR.enabled)
        {
            Debug.Log(string.Format("VR enabled, destroying {0}", gameObject.name));
            Destroy(gameObject);
        }
    }
}