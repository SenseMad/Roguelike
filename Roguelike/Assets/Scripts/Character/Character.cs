using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class Character : MonoBehaviour
{
  private InputHandler inputHandler;

  //====================================

  public CharacterController Controller { get; private set; } // CharacterShooterController

  public CameraController CameraController { get; private set; }

  public WeaponInventory WeaponInventory { get; private set; }

  public Animator Animator { get; private set; }

  public bool IsRunning { get; private set; }

  public bool IsShoot { get; private set; }

  public bool IsAiming { get; private set; }

  //====================================

  [Inject]
  private void Construct(InputHandler parInputHandler)
  {
    inputHandler = parInputHandler;
  }

  //====================================

  private void Awake()
  {
    Controller = GetComponent<CharacterController>();

    CameraController = GetComponentInChildren<CameraController>();

    WeaponInventory = GetComponent<WeaponInventory>();

    Animator = GetComponent<Animator>();
  }

  private void OnEnable()
  {
    inputHandler.AI_Player.Player.Run.started += OnRun;
    inputHandler.AI_Player.Player.Run.canceled += OnRun;

    inputHandler.AI_Player.Player.Aim.started += OnAim;
    inputHandler.AI_Player.Player.Aim.canceled += OnAim;

    inputHandler.AI_Player.Player.Shoot.started += OnShoot;
    inputHandler.AI_Player.Player.Shoot.performed += OnShoot;
    inputHandler.AI_Player.Player.Shoot.canceled += OnShoot;
  }

  private void OnDisable()
  {
    inputHandler.AI_Player.Player.Run.started -= OnRun;
    inputHandler.AI_Player.Player.Run.canceled -= OnRun;

    inputHandler.AI_Player.Player.Aim.started -= OnAim;
    inputHandler.AI_Player.Player.Aim.canceled -= OnAim;

    inputHandler.AI_Player.Player.Shoot.started -= OnShoot;
    inputHandler.AI_Player.Player.Shoot.performed -= OnShoot;
    inputHandler.AI_Player.Player.Shoot.canceled -= OnShoot;
  }

  //====================================

  private void OnRun(InputAction.CallbackContext context)
  {
    switch (context.phase)
    {
      case InputActionPhase.Started:
        IsRunning = true;
        break;
      case InputActionPhase.Canceled:
        IsRunning = false;
        break;
    }
  }

  private void OnShoot(InputAction.CallbackContext context)
  {
    if (WeaponInventory.ActiveWeapon == null)
      return;

    switch (context.phase)
    {
      case InputActionPhase.Started:
        IsShoot = true;
        break;
      case InputActionPhase.Canceled:
        IsShoot = false;
        break;
    }
  }

  private void OnAim(InputAction.CallbackContext context)
  {
    switch (context.phase)
    {
      case InputActionPhase.Started:
        IsAiming = true;
        break;
      case InputActionPhase.Canceled:
        IsAiming = false;
        break;
    }

    CameraController.AimCamera.gameObject.SetActive(IsAiming);
  }

  //====================================
}