using System.Collections;
using UnityEngine;

using Sirenix.OdinInspector;

public abstract class Weapon : MonoBehaviour
{
  [SerializeField, MinValue(0)] private int _damage;

  //====================================

  public abstract void Attack();

  //====================================
}