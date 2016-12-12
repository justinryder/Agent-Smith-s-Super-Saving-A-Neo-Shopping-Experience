using System;
using System.Collections;
using System.Collections.Generic;
using Cyberconian.Unity;
using UnityEngine;

public class Clock : MonoBehaviour
{
  private SevenSegmentDriver[] _displays;

  private float _startTime;

  private float _endTime;

  public string Timer;

  public bool Stopped
  {
    get { return _endTime > 0; }
  }

  void Start()
  {
    _displays = GetComponentsInChildren<SevenSegmentDriver>();
    _startTime = Time.time;
  }

  void Update()
  {
    var seconds = Time.time - _startTime;
    if (_endTime > 0)
    {
      seconds = _endTime - _startTime;
    }
    var timeSpan = TimeSpan.FromSeconds(seconds);

    // _display.Data = timeSpan.ToString("mmss"); // WTF?! Why does this not exist? https://msdn.microsoft.com/en-us/library/dd992632(v=vs.110).aspx

    Timer = string.Format(
      "{0}{1}:{2}{3}",
      timeSpan.Minutes / 10,
      timeSpan.Minutes % 10,
      timeSpan.Seconds / 10,
      timeSpan.Seconds % 10);

    foreach (var display in _displays)
    {
      display.Data = string.Format(
      "{0}{1}{2}{3}",
      timeSpan.Minutes / 10,
      timeSpan.Minutes % 10,
      timeSpan.Seconds / 10,
      timeSpan.Seconds % 10);
    }
  }

  public void Stop()
  {
    if (Stopped)
    {
      return;
    }
    _endTime = Time.time;
  }
}
