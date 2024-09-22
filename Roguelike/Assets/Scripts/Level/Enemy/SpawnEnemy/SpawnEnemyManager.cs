using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public abstract class SpawnEnemyManager : MonoBehaviour
{
  [SerializeField] protected WaveManager waveManager;

  //------------------------------------

  private RoomManager roomManager;

  private readonly List<Enemy> listCreatedEnemies = new List<Enemy>();

  protected int currentNumberEnemies;

  //====================================

  public event Action<Enemy> OnEnemyCreated;

  public event Action OnAllEneliesKilled;

  //====================================

  private void Awake()
  {
    roomManager = RoomManager.Instance;
  }

  private void OnEnable()
  {
    OnEnemyCreated += EnemyCreated;

    waveManager.OnWaveStarted += WaveManager_OnWaveStarted;
  }

  private void OnDisable()
  {
    OnEnemyCreated -= EnemyCreated;

    waveManager.OnWaveStarted -= WaveManager_OnWaveStarted;
  }

  private void Update()
  {
    if (Input.GetKeyDown(KeyCode.M))
    {
      for (int i = 0; i < listCreatedEnemies.Count; i++)
      {
        listCreatedEnemies[i].OnDiedInvoke();
      }
    }
  }

  //====================================

  private void WaveManager_OnWaveStarted(Wave parWave)
  {
    CreateEnemy(Vector3.zero);
  }

  public virtual void CreateEnemy(Vector3 parPosition)
  {
    Enemy randomEnemy = GetRandomEnemy();

    if (randomEnemy == null)
      return;

    OnEnemyCreated?.Invoke(randomEnemy);

    //Enemy enemy = Instantiate(randomEnemy, parPosition, Quaternion.identity);

    currentNumberEnemies++;
  }

  private Enemy GetRandomEnemy()
  {
    var currentActiveWave = waveManager.CurrentActiveWave;
    int totalNumberEnemiesWave = currentActiveWave.NumberEnemiesWave;

    if (currentActiveWave.NumberEnemiesCreated >= totalNumberEnemiesWave)
      return null;

    var waveSettings = waveManager.CurrentActiveWave.WaveSettings;

    int numberAttempts = 0;
    Enemy newEnemy;

    WaveSettings selectedWaveSetting;

    do
    {
      int newIndexEnemy = Random.Range(0, waveSettings.Count);
      selectedWaveSetting = waveSettings[newIndexEnemy];

      newEnemy = selectedWaveSetting.Enemy;

      if (selectedWaveSetting.Number > 0)
        break;

      numberAttempts++;

      if (numberAttempts > totalNumberEnemiesWave)
        return null;

    } while (true);

    selectedWaveSetting.ReduceNumber();

    Debug.Log($"ActiveWave: {waveManager.CurrentActiveWave.name} Enemy: {newEnemy}");

    return newEnemy;
  }

  //====================================

  private void EnemyCreated(Enemy parEnemy)
  {
    listCreatedEnemies.Add(parEnemy);

    waveManager.CurrentActiveWave.AddNumberEnemiesCreated();

    parEnemy.OnDied += Enemy_OnDied;
  }

  protected virtual void Enemy_OnDied(Enemy parEnemy)
  {
    parEnemy.OnDied -= Enemy_OnDied;

    listCreatedEnemies.Remove(parEnemy);

    if (listCreatedEnemies.Count != 0)
      return;

    waveManager.OnWaveCompletedInvoke();
    AllEnemiesKilled();
  }

  //====================================

  /// <summary>
  /// It is triggered when killing all enemies on the wave
  /// </summary>
  public void AllEnemiesKilled()
  {
    var activeWave = waveManager.CurrentActiveWave;

    if (activeWave.NumberEnemiesCreated < activeWave.NumberEnemiesWave)
      return;

    if (listCreatedEnemies.Count != 0)
      return;

    roomManager.CurrentRoom.RoomPortal.OpenPortalInvoke();
  }

  //====================================
}