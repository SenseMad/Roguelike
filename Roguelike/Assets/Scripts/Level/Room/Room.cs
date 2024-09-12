using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Room : MonoBehaviour
{
  [SerializeField] private List<Transform> _spawnPointsEnemies;

  //------------------------------------

  private List<Transform> spawnPointUsed = new List<Transform>();

  //====================================

  public List<Transform> SpawnPointsEnemies => _spawnPointsEnemies;

  //====================================



  //====================================



  //====================================

  public Transform TrySpawnPoint()
  {
    int indexSpawnPoint = Random.Range(0, _spawnPointsEnemies.Count);
    var spawnPoint = _spawnPointsEnemies[indexSpawnPoint];
    int numberAttempts = 0;

    while (spawnPointUsed[indexSpawnPoint] != null)
    {
      indexSpawnPoint = Random.Range(0, _spawnPointsEnemies.Count);
      spawnPoint = _spawnPointsEnemies[indexSpawnPoint];

      if (numberAttempts >= _spawnPointsEnemies.Count)
        spawnPointUsed.Clear();

      numberAttempts++;
    }

    spawnPointUsed.Add(spawnPoint);
    return spawnPoint;
  }

  //====================================
}