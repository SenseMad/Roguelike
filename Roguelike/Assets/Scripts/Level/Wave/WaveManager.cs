using System;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
  [SerializeField] private Wave[] _waves;

  [SerializeField] private float _timeStartNextWave = 0.0f; // Time until the start of the next wave

  //------------------------------------

  private float tempTimeStartNextWave;

  private bool isWaveStarted;
  private bool isWavesIsOver;

  //====================================

  public int CurrentWaveIndex { get; private set; } = -1;

  public Wave CurrentActiveWave { get; private set; }

  public Wave[] Waves { get => _waves; set => _waves = value; }

  //====================================

  public event Action<Wave> OnWaveBegun;

  public event Action<Wave> OnWaveCompleted;

  public event Action<Wave> OnWaveStarted;

  public event Action OnWavesAreOver;

  //====================================

  private void Awake()
  {
    Initialize();
  }

  private void Start()
  {
    StartWaves();
  }

  private void OnEnable()
  {
    OnWaveCompleted += WaveComplete;
  }

  private void OnDisable()
  {
    OnWaveCompleted -= WaveComplete;
  }

  private void Update()
  {
    NextWaveTime();
  }

  //====================================

  public void Initialize()
  {
    _waves = GetComponentsInChildren<Wave>(true);

    foreach (var wave in _waves)
    {
      wave.gameObject.SetActive(false);
    }
  }

  public void StartWaves()
  {
    isWaveStarted = true;

    NextWave();
  }

  public void NextWave()
  {
    if (isWavesIsOver)
      return;

    CurrentWaveIndex++;

    CurrentActiveWave?.DeactivateWave();

    if (CurrentWaveIndex > _waves.Length - 1)
    {
      OnWavesAreOver?.Invoke();
      isWavesIsOver = true;
      isWaveStarted = false;
      enabled = false;
      return;
    }

    CurrentActiveWave = _waves[CurrentWaveIndex];

    CurrentActiveWave.ActivateWave();
    OnWaveStarted?.Invoke(CurrentActiveWave);
    tempTimeStartNextWave = 0;
  }

  public void WaveComplete(Wave parWave)
  {
    parWave.OnWaveAreOverInvoke();

    NextWave();
  }

  public void NextWaveTime()
  {
    if (isWavesIsOver)
      return;

    if (!isWaveStarted)
      return;

    tempTimeStartNextWave += Time.deltaTime;
    if (tempTimeStartNextWave >= _timeStartNextWave)
    {
      WaveComplete(CurrentActiveWave);
      tempTimeStartNextWave = 0;
    }
  }

  public int GetNumberEnemiesWaves()
  {
    int count = 0;
    foreach (var wave in _waves)
    {
      count += wave.GetNumberEnemiesWave();
    }

    return count;
  }

  public void OnWaveCompletedInvoke()
  {
    OnWaveCompleted?.Invoke(CurrentActiveWave);
  }

  //====================================
}