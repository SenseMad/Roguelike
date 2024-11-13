using System;
using UnityEngine;

using Sirenix.OdinInspector;

[Serializable]
public sealed class EnemyAttackData
{
  [SerializeField, MinValue(0)] private float _attackRange;

  [SerializeField, MinValue(0)] private float _visibilityRange;

  [SerializeField, MinValue(0)] private float _patrolRange;

  [SerializeField] private LayerMask _targetMask;

  //====================================

  public float AttackRange => _attackRange;

  public float VisibilityRange => _visibilityRange;

  public float PatrolRange => _patrolRange;

  public LayerMask TargetMask => _targetMask;

  //====================================
}