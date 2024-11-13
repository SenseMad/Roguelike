using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using Sirenix.OdinInspector;

public class Enemy : MonoBehaviour
{
  [SerializeField] private EnemyAttackData _attackData;
  [SerializeField, MinValue(0)] private float _rotationSpeed;

  [SerializeField] private WeaponInventory _weaponInventory;

  [SerializeField] private Health _health;

  //------------------------------------

  public NavMeshAgent NavMeshAgent { get; private set; }

  public Animator Animator { get; private set; }

  public Character Character { get; private set; }

  //====================================

  public EnemyAttackData AttackData => _attackData;

  public WeaponInventory WeaponInventory => _weaponInventory;

  public Health Health => _health;

  //====================================

  public event Action<Enemy> OnDied;

  //====================================

  private void Awake()
  {
    NavMeshAgent = GetComponent<NavMeshAgent>();

    Animator = GetComponent<Animator>();

    Character = Character.Instance;
  }

  private void Update()
  {
    
  }

  //====================================

  public virtual void Attack() { }

  //====================================

  public void OnDiedInvoke()
  {
    OnDied?.Invoke(this);
  }

  public void RotateTarget()
  {
    Vector3 direction = (Character.transform.position - transform.position).normalized;
    Quaternion lookRotataion = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));

    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotataion, Time.deltaTime * _rotationSpeed);
  }

  public bool IsPossibleTarget()
  {
    NavMeshPath path = new NavMeshPath();
    NavMesh.CalculatePath(transform.position, Character.transform.position, NavMesh.AllAreas, path);

    return path.status == NavMeshPathStatus.PathComplete;
  }

  public bool CharacterSearch(float parRangeValue)
  {
    return Physics.CheckSphere(transform.position, parRangeValue, _attackData.TargetMask);
  }

  //====================================

  private void OnDrawGizmosSelected() // Selected
  {
    Gizmos.color = Color.yellow;
    Gizmos.DrawWireSphere(transform.position, _attackData.VisibilityRange);

    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(transform.position, _attackData.AttackRange);

    Gizmos.color = Color.green;
    Gizmos.DrawWireSphere(transform.position, _attackData.PatrolRange);
  }

  //====================================
}