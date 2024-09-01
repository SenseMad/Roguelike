using UnityEngine;

public class DesktopInput : IInput
{
  private InputHandler inputHandler;

  //====================================

  public DesktopInput(InputHandler parInputHandler)
  {
    inputHandler = parInputHandler;
  }

  //====================================

  public Vector2 Move()
  {
    return inputHandler.AI_Player.Player.Move.ReadValue<Vector2>();
  }

  public Vector2 Look()
  {
    return inputHandler.AI_Player.Player.Look.ReadValue<Vector2>();
  }

  //====================================
}