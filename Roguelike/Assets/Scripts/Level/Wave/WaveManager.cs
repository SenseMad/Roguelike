using System;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
  [SerializeField] private SingleWave[] _singleWaves;

  //====================================

  public int CurrentWaveIndex { get; private set; } = -1;

  public SingleWave CurrentActiveWave { get; private set; }

  //====================================

  public event Action<int> OnWaveBegun;

  public event Action<int> OnWaveCompleted;

  public event Action OnWavesIsOver;

  //====================================

  private void Awake()
  {
    Initialize();
  }

  private void OnEnable()
  {
    foreach (var singleWave in _singleWaves)
    {
      singleWave.OnWaveIsOver += NextWave;
    }
  }

  private void OnDisable()
  {
    foreach (var singleWave in _singleWaves)
    {
      singleWave.OnWaveIsOver -= NextWave;
    }
  }

  //====================================

  public void Initialize()
  {
    _singleWaves = GetComponentsInChildren<SingleWave>(true);

    for (int i = 0; i < _singleWaves.Length; i++)
    {
      _singleWaves[i].gameObject.SetActive(false);
    }
  }

  public void StartWave()
  {
    CurrentWaveIndex++;

    _singleWaves[CurrentWaveIndex].gameObject.SetActive(true);

    OnWaveBegun?.Invoke(CurrentWaveIndex);
  }

  public void NextWave()
  {
    if (CurrentWaveIndex < _singleWaves.Length - 1)
    {
      OnWaveCompleted?.Invoke(CurrentWaveIndex);
      return;
    }

    OnWavesIsOver?.Invoke();
  }

  public int GetNumberEnemiesWaves()
  {
    int count = 0;
    foreach (var singleWave in _singleWaves)
    {
      count += singleWave.GetNumberEnemies();
    }

    return count;
  }

  //====================================
}