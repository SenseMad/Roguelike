using System.Collections;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
  [SerializeField] private MonoBehaviour[] _scriptsToExecute;

  //---------------------------------

  private IBootstrap[] bootstraps;

  //=================================

  private void Awake()
  {
    bootstraps = new IBootstrap[_scriptsToExecute.Length];

    for (int i = 0; i < _scriptsToExecute.Length; i++)
    {
      _scriptsToExecute[i].enabled = false;
      bootstraps[i] = (IBootstrap)_scriptsToExecute[i];
    }

    foreach (var bootstrap in bootstraps)
    {
      bootstrap.CustomAwake();
    }

    for (int i = 0; i < _scriptsToExecute.Length; i++)
    {
      _scriptsToExecute[i].enabled = true;
    }
  }

  private IEnumerator Start()
  {
    foreach (var bootstrap in bootstraps)
    {
      bootstrap.CustomStart();
      yield return null;
    }
  }

  //=================================
}