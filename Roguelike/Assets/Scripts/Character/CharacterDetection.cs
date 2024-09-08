using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

using Sirenix.OdinInspector;

public class CharacterDetection : MonoBehaviour
{
  [SerializeField, MinValue(0)] private float _detectingRange = 2.0f;

  //====================================

  private InputHandler inputHandler;

  private Camera mainCamera;

  //====================================

  [Inject]
  private void Construct(InputHandler parInputHandler)
  {
    inputHandler = parInputHandler;
  }

  //====================================

  private void Start()
  {
    mainCamera = Camera.main;
  }

  private void OnEnable()
  {
    inputHandler.AI_Player.Player.Interact.performed += Detection;
  }

  private void OnDisable()
  {
    inputHandler.AI_Player.Player.Interact.performed -= Detection;
  }

  //====================================

  private void Detection(InputAction.CallbackContext obj)
  {
    Vector2 screenCenterPosition = new Vector2(Screen.width / 2f, Screen.height / 2f);
    Ray ray = mainCamera.ScreenPointToRay(screenCenterPosition);

    if (!Physics.Raycast(ray, out RaycastHit hit, _detectingRange))
      return;

    if (!hit.collider.TryGetComponent(out IInteractable parInteractable))
      return;

    parInteractable.Interact();
  }

  //====================================
}