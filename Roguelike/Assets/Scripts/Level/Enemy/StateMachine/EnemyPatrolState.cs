using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemyPatrolState : EnemyBaseState
{
  private bool endPatrolPoint = false;
  private float tempTimeEndPatrolPoint = 0;
  private float timeDelayAfterPatrolling = 0;

  //====================================

  public override void EnterState(EnemyStateMachine parState)
  {
    //Debug.Log("Патрулирование");
    parState.Enemy.NavMeshAgent.isStopped = false;

    endPatrolPoint = false;
    tempTimeEndPatrolPoint = 0;
    timeDelayAfterPatrolling = 0;
  }

  public override void UpdateState(EnemyStateMachine parState)
  {
    if (parState.Enemy.IsPossibleTarget())
    {
      if (parState.Enemy.CharacterSearch(parState.Enemy.AttackData.VisibilityRange))
        parState.SwithState(parState.FollowState);
    }

    Patrolling(parState);
  }

  //====================================

  private void Patrolling(EnemyStateMachine parState)
  {
    if (endPatrolPoint)
    {
      tempTimeEndPatrolPoint += Time.deltaTime;
      if (tempTimeEndPatrolPoint >= timeDelayAfterPatrolling)
      {
        tempTimeEndPatrolPoint = 0;
        endPatrolPoint = false;

        RandomPatrolDirection(parState);
      }

      return;
    }

    if (!parState.Enemy.NavMeshAgent.pathPending && parState.Enemy.NavMeshAgent.remainingDistance < 0.5f && !endPatrolPoint)
    {
      endPatrolPoint = true;
      timeDelayAfterPatrolling = Random.Range(0.1f, 1.5f);
    }
  }

  private void RandomPatrolDirection(EnemyStateMachine parState)
  {
    Vector3 randomDirection;
    NavMeshPath path = new NavMeshPath();

    do
    {
      randomDirection = Random.insideUnitSphere * parState.Enemy.AttackData.PatrolRange;
      randomDirection += parState.Enemy.transform.position;
      randomDirection.y = parState.Enemy.transform.position.y;

      NavMesh.CalculatePath(parState.Enemy.transform.position, randomDirection, NavMesh.AllAreas, path);

    } while (path.status != NavMeshPathStatus.PathComplete);

    parState.Enemy.NavMeshAgent.SetDestination(randomDirection);
  }

  //====================================
}