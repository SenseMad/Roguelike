using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterWalkState : CharacterBaseState
{
  public override void EnterState(CharacterStateMachine parState)
  {
    
  }

  public override void UpdateState(CharacterStateMachine parState)
  {
    parState.Movement.Move(parState.InputHandler.IInput);
  }
}