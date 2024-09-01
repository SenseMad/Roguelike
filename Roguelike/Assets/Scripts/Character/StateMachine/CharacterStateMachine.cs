using UnityEngine;
using Zenject;

public class CharacterStateMachine : MonoBehaviour
{
  private CharacterBaseState currentState;

  //====================================

  public CharacterMovement Movement { get; private set; }

  public CharacterIdleState IdleState { get; private set; }
  public CharacterWalkState WalkState { get; private set; }

  public InputHandler InputHandler { get; private set; }

  //====================================

  [Inject]
  private void Construct(InputHandler parInputHandler)
  {
    InputHandler = parInputHandler;
  }

  //====================================

  private void Awake()
  {
    Movement = GetComponent<CharacterMovement>();

    IdleState = new CharacterIdleState();
    WalkState = new CharacterWalkState();
  }

  private void Start()
  {
    currentState = WalkState;

    currentState.EnterState(this);
  }

  private void Update()
  {
    currentState.UpdateState(this);
  }

  //====================================

  public void SwithState(CharacterBaseState parState)
  {
    currentState = parState;
    parState.EnterState(this);
  }

  //====================================
}