using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemyDelay : SpawnEnemyManager
{


  //====================================

  public override void CreateEnemy(Vector3 parPosition)
  {
    int totalNumberEnemiesWave = _waveManager.CurrentActiveWave.NumberEnemiesWave;

    for (int i = 0; i < totalNumberEnemiesWave; i++)
    {
      base.CreateEnemy(parPosition);
    }

    _waveManager.CurrentActiveWave.OnWaveAreOverInvoke();

    if (currentNumberEnemies >= totalNumberEnemiesWave)
    {
      currentNumberEnemies = 0;
      //OnWaveIsOver?.Invoke();
      //isWaveActive = false;
    }
  }

  //====================================



  //====================================
}