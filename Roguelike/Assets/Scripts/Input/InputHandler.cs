using UnityEngine;

public class InputHandler : MonoBehaviour
{
  public AI_Player AI_Player { get; private set; }

  public IInput IInput { get; private set; }

  //====================================

  private void Awake()
  {
    AI_Player = new AI_Player();

    SetCursor(false);
  }

  private void OnEnable()
  {
    AI_Player.Enable();

    InitializeInput();
  }

  private void OnDisable()
  {
    AI_Player.Disable();
  }

  //====================================

  public void SetCursor(bool parValue)
  {
    Cursor.visible = parValue;
    Cursor.lockState = parValue ? CursorLockMode.None : CursorLockMode.Locked;
  }

  //====================================

  private void InitializeInput()
  {
    if (SystemInfo.deviceType != DeviceType.Handheld)
      IInput = new DesktopInput(this);
  }

  //====================================
}