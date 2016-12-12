﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using UnityEngine;

public static class Extensions
{
  public static string GetEnumDescription(this Enum value)
  {
    FieldInfo fi = value.GetType().GetField(value.ToString());

    DescriptionAttribute[] attributes =
        (DescriptionAttribute[])fi.GetCustomAttributes(
        typeof(DescriptionAttribute),
        false);

    if (attributes != null &&
        attributes.Length > 0)
      return attributes[0].Description;
    else
      return value.ToString();
  }
}