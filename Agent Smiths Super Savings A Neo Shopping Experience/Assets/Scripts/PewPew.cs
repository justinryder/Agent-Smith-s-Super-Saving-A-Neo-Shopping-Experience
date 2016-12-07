using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HTC.UnityPlugin.Vive;
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
        var firing = false;
        if (VR)
            firing = ViveInput.GetPressDown(HandRole.RightHand, ControllerButton.Trigger);
        else
            firing = Input.GetMouseButtonDown(0);

        if (firing)
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