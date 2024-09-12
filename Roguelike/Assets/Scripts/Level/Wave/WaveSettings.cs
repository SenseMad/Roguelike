using System;
using UnityEngine;

[Serializable]
public class WaveSettings
{
  [SerializeField] private Enemy _enemy;
  [SerializeField] private int _number;

  //====================================
  
  public Enemy Enemy => _enemy;
  public int Number => _number;

  //====================================

  public void ReduceNumber()
  {
    if (_number == 0)
      return;

    _number--;
  }

  //====================================
}