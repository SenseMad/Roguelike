using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : MonoBehaviour
{
  private EnemyBaseState currentState;

  //====================================

  public Enemy Enemy { get; private set; }

  public EnemyIdleState IdleState { get; private set; }
  public EnemyFollowState FollowState { get; private set; }
  public EnemyAttackState AttackState { get; private set; }
  public EnemyPatrolState PatrolState { get; private set; }

  //====================================

  private void Awake()
  {
    Enemy = GetComponent<Enemy>();

    IdleState = new EnemyIdleState();
    FollowState = new EnemyFollowState();
    AttackState = new EnemyAttackState();
    PatrolState = new EnemyPatrolState();
  }

  public void Start()
  {
    currentState = PatrolState;

    currentState.EnterState(this);
  }

  private void Update()
  {
    currentState.UpdateState(this);
  }

  //====================================

  public void SwithState(EnemyBaseState parState)
  {
    currentState = parState;
    parState.EnterState(this);
  }

  //====================================
}