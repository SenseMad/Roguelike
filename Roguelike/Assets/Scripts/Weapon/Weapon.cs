using System.Collections;
using UnityEngine;

using Sirenix.OdinInspector;

public abstract class Weapon : MonoBehaviour
{
  [SerializeField, MinValue(0)] private int _damage;

  //====================================

  public bool CanShoot { get; private set; }

  //====================================

  public abstract void Attack();

  public void TryShoot(bool parValue)
  {
    CanShoot = parValue;
  }

  //====================================
}