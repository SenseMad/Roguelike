using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
  [SerializeField] private WeaponInventory _weaponInventory;

  [SerializeField] private Health _health;

  //------------------------------------

  public Animator Animator { get; private set; }

  //====================================

  public WeaponInventory WeaponInventory => _weaponInventory;

  public Health Health => _health;

  //====================================

  public event Action<Enemy> OnDied;

  //====================================

  private void Awake()
  {
    Animator = GetComponent<Animator>();
  }

  //====================================

  public void OnDiedInvoke()
  {
    OnDied?.Invoke(this);
  }

  //====================================
}