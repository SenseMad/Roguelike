using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFollowState : EnemyBaseState
{
  public override void EnterState(EnemyStateMachine parState)
  {
    parState.Enemy.NavMeshAgent.isStopped = false;
    //Debug.Log("Преслодование игрока");
  }

  public override void UpdateState(EnemyStateMachine parState)
  {
    if (!parState.Enemy.CharacterSearch(parState.Enemy.AttackData.VisibilityRange))
      parState.SwithState(parState.PatrolState);

    if (parState.Enemy.IsPossibleTarget())
    {
      if (parState.Enemy.CharacterSearch(parState.Enemy.AttackData.AttackRange))
        parState.SwithState(parState.AttackState);
    }

    FollowCharacter(parState);
  }

  //====================================

  private void FollowCharacter(EnemyStateMachine parState)
  {
    if (!parState.Enemy.IsPossibleTarget())
    {
      parState.SwithState(parState.PatrolState);
      return;
    }

    var targetPosition = parState.Enemy.Character.transform.position;

    parState.Enemy.NavMeshAgent.SetDestination(targetPosition);
  }

  //====================================
}