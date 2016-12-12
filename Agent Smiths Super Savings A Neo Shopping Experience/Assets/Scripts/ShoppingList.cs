using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;

public class ShoppingList : MonoBehaviour
{
  public static readonly Product[] PossibleItems =
  {
    new Product(ItemType.DietSodaOrange, 0.99f),
    new Product(ItemType.DietSodaCherry, 0.99f),
    new Product(ItemType.DietSodaGrape, 0.99f),
    new Product(ItemType.DietSodaBlueberry, 0.99f),
    new Product(ItemType.CocoPops, 1.99f),
    new Product(ItemType.CornPops, 2.99f),
    new Product(ItemType.Shreddies, 3.99f),
    new Product(ItemType.CatFood, 5.99f),
    new Product(ItemType.ChipsBoulder, 15.99f),
    new Product(ItemType.ChipsDoritos, 1.99f),
    new Product(ItemType.ChipsLays, 1.99f),
    new Product(ItemType.ChipsLaysBbq, 1.99f),
    new Product(ItemType.ChipsLaysLimon, 1.99f),
    new Product(ItemType.ChipsSunChips, 2.99f)
  };

  private Clock _clock;

  private List<Product> _toBuy;

  private List<Product> _bought = new List<Product>();

  private float _budget;
	private float _budgetSurpluss = 3f; // Allows for impluse buys without penalty

  public float Budget { get { return _budget; } }

  public float BudgetSurplus { get { return _budgetSurpluss; } }

  private List<Product> _impulseBuys = new List<Product>();

  private float _impulseTotal;
  public float ImpulseTotal { get { return _impulseTotal; } }

  private TextMesh _text;

  private float _wallet;
  public float Wallet { get { return _wallet; } }

  public bool WinnaWinnaChickenDinna { get { return !_toBuy.Any(); } }

  void Start()
  {
    _clock = FindObjectOfType<Clock>();
    _text = GetComponentInChildren<TextMesh>();
    _text.richText = true;

    GenerateShoppingList();
    UpdateText();
  }

  public void Bought(Target target)
  {
    var product = PossibleItems.FirstOrDefault(x => x.ItemType == target.ItemType);
    if (product == null)
    {
      Debug.LogError(string.Format("Bought item {0} does not exist in PossibleItems", target.ItemType));
      return;
    }

    if (_toBuy.Contains(product))
    {
      _bought.Add(product);
      _toBuy.Remove(product);
    }
    else
    {
      _impulseBuys.Add(product);
    }

    if (!_toBuy.Any())
    {
      _clock.Stop();
    }
    
    UpdateText();
  }

  private void GenerateShoppingList()
  {
    Array.Sort(PossibleItems, (x, y) => Random.Range(-1, 1));
    _toBuy = PossibleItems.Take(5).ToList();
		_budget = _toBuy.Sum(x => x.Price) + _budgetSurpluss; 
  }

  private void UpdateText()
  {
    var stringBuilder = new StringBuilder();

    stringBuilder.AppendLine("<b>Shopping List</b>");

    if (_clock.Stopped)
    {
      stringBuilder.AppendLine(string.Format("You finished in {0}!", _clock.Timer));
    }

		if (_toBuy.Any())
		{
			stringBuilder.AppendLine(string.Format("\n{0}", string.Join("\n", _toBuy.Select(x => x.ToString()).ToArray())));
		}

		if (_bought.Any())
		{
			stringBuilder.AppendLine(string.Format("<color=teal>{0}</color>", string.Join("\n", _bought.Select(x => x.ToString()).ToArray())));
		}

		// budget line
    stringBuilder.AppendLine(string.Format("\n<b>Budgeted: {0}</b>", _budget.ToString("C")));

		// impluse line
    _impulseTotal = _impulseBuys.Sum(x => x.Price);
		stringBuilder.AppendLine(string.Format("<b>Impluse Buys: {0}</b>", _impulseTotal.ToString("C")));

		// wallet line = budget - purchases
		_wallet = _budget - (_bought.Sum(x => x.Price) + _impulseBuys.Sum(x => x.Price));

		if (_wallet < 1) {
			stringBuilder.AppendLine(string.Format("<color=red><b>Wallet: {0}</b></color>", _wallet.ToString("C")));
		} else {
			stringBuilder.AppendLine(string.Format("<b>Wallet: {0}</b>", _wallet.ToString("C")));
		}    

    /*if (_impulseBuys.Any())
    {
      stringBuilder.AppendLine("\n<b>Impulse Buys</b>");

      stringBuilder.AppendLine(string.Format("<color=red>{0}</color>", string.Join("\n", _impulseBuys.Select(x => x.ToString()).ToArray())));
    }*/

    _text.text = stringBuilder.ToString();
  }
}