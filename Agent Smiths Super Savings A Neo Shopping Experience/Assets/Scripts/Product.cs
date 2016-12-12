using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Product
{
  public Product(ItemType itemType, float price)
  {
    ItemType = itemType;
    Price = price;
  }

  public ItemType ItemType;

  public float Price;

  public override string ToString()
  {
    return string.Format("{0}\t{1}", ItemType.GetEnumDescription(), Price.ToString("C"));
  }
}