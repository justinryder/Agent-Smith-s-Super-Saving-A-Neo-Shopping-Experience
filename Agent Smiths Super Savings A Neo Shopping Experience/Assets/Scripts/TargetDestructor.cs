using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDestructor : MonoBehaviour {
  public bool FallOver = false;

  // shrink vars
	private Vector3 initialScale;
	private float startTime;
	private float shrinkDuration = 0.2f;
	private float targetScale = 0.1f;
	private bool isShrinking = false;

  // fall over vars
  private Vector3 _lookAtStart;
  private Vector3 _lookAtEnd;

	void Start (){
		initialScale = transform.localScale;
	}

	public void Destroy () {
		startTime = Time.time;
		isShrinking = true;
    _lookAtStart = transform.forward;
    _lookAtEnd = transform.up;
	}

	void Update() {
		if (isShrinking) {
      var alpha = (Time.time - startTime) / shrinkDuration;
      
      if (FallOver)
      {
        var lookAt = Vector3.Lerp(_lookAtStart, _lookAtEnd, alpha);
        transform.LookAt(transform.position + lookAt);
      }
      else
      {
        transform.localScale = Vector3.Lerp(initialScale, Vector3.zero, alpha);
      }

      if (alpha > 1.0f)
      {
        Destroy(gameObject);
      }
		}
	}
}
