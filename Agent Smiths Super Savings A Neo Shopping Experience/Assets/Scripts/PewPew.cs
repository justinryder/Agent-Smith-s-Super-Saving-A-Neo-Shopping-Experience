using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PewPew : MonoBehaviour
{
    public bool VR = true;

    public string InputAxis = "Fire1";

    void Start()
    {
        if (VR)
            enabled = SteamVR.enabled;
        else
            enabled = !SteamVR.enabled;
    }
    
    void Update()
    {
        if (Input.GetAxis(InputAxis) == 1)
        {
            Debug.Log("Firing");
            var hits = Physics.RaycastAll(transform.position, transform.forward);
            var hitTarget = hits.Select(x => x.transform.GetComponent<Target>()).FirstOrDefault(x => x);
            Debug.Log(string.Format(
                "Raycast hit {0} objects ({1}) {2}",
                hits.Length,
                string.Join(",", hits.Select(x => x.transform.gameObject.name).ToArray()),
                hitTarget ? "got Target" : ""));

            if (hitTarget)
            {
                Debug.Log(string.Format("Hit {0} for {1}.", hitTarget.name, hitTarget.Cost.ToString("C")));
                Destroy(hitTarget.gameObject);
            }
        }
    }
}