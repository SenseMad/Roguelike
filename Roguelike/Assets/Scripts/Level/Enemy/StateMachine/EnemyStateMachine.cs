using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : MonoBehaviour
{
  private EnemyBaseState currentState;

  //====================================



  //====================================

  private void Awake()
  {

  }

  public void Start()
  {
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