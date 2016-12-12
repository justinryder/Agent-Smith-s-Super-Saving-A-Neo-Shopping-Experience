using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour
{
  public float SpawnDelay = 10.0f;

  public List<GameObject> ItemPrefabs;

  private float _lastSpawnTime;

  private int _spawnCount = 2;

  private int _spawnIncrement = 2;

  private int _maxSpawnCount = 10;

  private readonly Dictionary<ItemType, GameObject> _itemPrefabs = new Dictionary<ItemType, GameObject>();

  private SpawnPoint[] _spawnPoints;

  private Queue<ItemType> _spawnOrder = new Queue<ItemType>();

  private readonly ItemType[] _itemTypes = Enum.GetValues(typeof(ItemType)).Cast<ItemType>().ToArray();

  void Awake()
  {
    foreach (var itemPrefab in ItemPrefabs)
    {
      var target = itemPrefab.GetComponent<Target>();
      if (target == null)
      {
        Debug.LogError(string.Format("SpawnPointManager {0}'s ItemPrefab {1} does not have a Target script.", name, itemPrefab.name));
        continue;
      }

      if (_itemPrefabs.ContainsKey(target.ItemType))
      {
        Debug.LogError(string.Format("SpawnPointManager {0}'s ItemPrefabs contains multiple prefabs for ItemType {1}", name, target.ItemType));
        continue;
      }

      _itemPrefabs.Add(target.ItemType, itemPrefab);
    }
  }

  void Start()
  {
    _spawnPoints = FindObjectsOfType<SpawnPoint>();
    SpawnItems();
  }

  void Update()
  {
    if (Time.time - _lastSpawnTime > SpawnDelay)
    {
      SpawnItems();
    }
  }

  void SpawnItems()
  {
    Debug.Log(string.Format("Spawning {0} items", _spawnCount));
    Array.Sort(_spawnPoints, (x, y) => Random.Range(-1, 1));
    foreach (var spawnPoint in _spawnPoints.Take(_spawnCount))
    {
      if (!_spawnOrder.Any())
      {
        Array.Sort(_itemTypes, (x, y) => Random.Range(-1, 1));
        foreach (var itemType in _itemTypes)
        {
          _spawnOrder.Enqueue(itemType);
        }
      }

      spawnPoint.Spawn(_itemPrefabs[_spawnOrder.Dequeue()]);
    }

    _lastSpawnTime = Time.time;
    if (_spawnCount + _spawnIncrement > _maxSpawnCount)
    {
      _spawnCount = _maxSpawnCount;
    }
    else
    {
      _spawnCount += _spawnIncrement;
    }
  }
}