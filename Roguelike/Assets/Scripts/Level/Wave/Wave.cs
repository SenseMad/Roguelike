using System;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour
{
  [SerializeField] protected List<WaveSettings> _waveSettings;

  //====================================

  public List<WaveSettings> WaveSettings { get => _waveSettings; private set => _waveSettings = value; }

  public int NumberEnemiesWave { get; private set; }

  public int NumberEnemiesCreated { get; private set; }

  //====================================

  public event Action OnWaveAreOver;

  //====================================

  private void OnEnable()
  {
    OnWaveAreOver += DeactivateWave;
  }

  private void OnDisable()
  {
    OnWaveAreOver -= DeactivateWave;
  }

  //====================================

  public int GetNumberEnemiesWave()
  {
    int count = 0;
    foreach (var waveSettings in _waveSettings)
    {
      count += waveSettings.Number;
    }

    return count;
  }

  //====================================

  public void ActivateWave()
  {
    gameObject.SetActive(true);

    NumberEnemiesWave = GetNumberEnemiesWave();
  }

  public void DeactivateWave()
  {
    gameObject.SetActive(false);
  }

  public void OnWaveAreOverInvoke()
  {
    OnWaveAreOver?.Invoke();
  }

  public void AddNumberEnemiesCreated()
  {
    NumberEnemiesCreated++;
  }

  //====================================
}