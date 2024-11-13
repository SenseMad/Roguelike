using System.Collections;
using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
  public override void EnterState(EnemyStateMachine parState)
  {
    parState.Enemy.NavMeshAgent.isStopped = true;
    //Debug.Log("Атака игрока");
  }

  public override void UpdateState(EnemyStateMachine parState)
  {
    if (!parState.Enemy.IsPossibleTarget())
      parState.SwithState(parState.PatrolState);

    if (!parState.Enemy.CharacterSearch(parState.Enemy.AttackData.AttackRange))
      parState.SwithState(parState.FollowState);

    parState.Enemy.RotateTarget();
  }
}