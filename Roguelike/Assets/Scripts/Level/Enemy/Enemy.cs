using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
  [SerializeField] private Health _health;

  //====================================

  public Health Health => _health;

  //====================================

  public event Action<Enemy> OnDied;

  //====================================

  private void Start()
  {
    //OnDied?.Invoke(this);
  }

  //====================================

  public void OnDiedInvoke()
  {
    OnDied?.Invoke(this);
  }

  //====================================
}