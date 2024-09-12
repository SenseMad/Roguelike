using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SingleWave : MonoBehaviour
{
  [SerializeField] private List<WaveSettings> _waveSettings;

  //------------------------------------

  private List<WaveSettings> tempWaveSettings = new List<WaveSettings>();

  private int numberEnemies;

  //====================================

  private void Start()
  {
    tempWaveSettings = _waveSettings;

    numberEnemies = GetNumberEnemies();
  }

  //====================================

  public event Action OnWaveIsOver;

  //====================================

  private void WaveIsOver()
  {

  }

  //====================================

  public void CreateEnemy(Vector3 parPosition)
  {
    Enemy randomEnemy = GetRandomEnemy();

    if (randomEnemy == null)
      return;

    Enemy enemy = Instantiate(randomEnemy, parPosition, Quaternion.identity);
  }

  private Enemy GetRandomEnemy()
  {
    int indexWaveSettings = Random.Range(0, tempWaveSettings.Count);
    int numberAttempts = 0;

    while (tempWaveSettings[indexWaveSettings].Number <= 0)
    {
      indexWaveSettings = Random.Range(0, tempWaveSettings.Count);

      if (numberAttempts > numberEnemies)
        return null;

      numberAttempts++;
    }

    tempWaveSettings[indexWaveSettings].ReduceNumber();

    return tempWaveSettings[indexWaveSettings].Enemy;
  }

  public int GetNumberEnemies()
  {
    int count = 0;
    foreach (var waveSettings in _waveSettings)
    {
      count += waveSettings.Number;
    }

    return count;
  }

  //====================================
}