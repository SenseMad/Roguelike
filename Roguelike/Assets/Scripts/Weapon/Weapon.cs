using System.Collections;
using UnityEngine;

using Sirenix.OdinInspector;

public abstract class Weapon : MonoBehaviour
{
  [SerializeField, MinValue(0)] private int _damage;

  //====================================

  public bool CanShoot { get; private set; }

  public WeaponPosition WeaponPosition { get; private set; }
  public WeaponInteractable WeaponInteractable { get; private set; }

  //====================================

  protected virtual void Awake()
  {
    WeaponPosition = GetComponent<WeaponPosition>();
    WeaponInteractable = GetComponentInChildren<WeaponInteractable>();
  }

  //====================================

  public abstract void Attack();
  public abstract void Recharge();

  public void TryShoot(bool parValue)
  {
    CanShoot = parValue;
  }

  //====================================
}