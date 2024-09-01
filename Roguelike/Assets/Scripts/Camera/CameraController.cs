using UnityEngine;
using Unity.Cinemachine;
using Zenject;

using Sirenix.OdinInspector;

public class CameraController : MonoBehaviour
{
  [FoldoutGroup("Camera")]
  [SerializeField] private CinemachineCamera _mainCamera;
  [FoldoutGroup("Camera")]
  [SerializeField] private CinemachineCamera _aimCamera;
  [FoldoutGroup("Camera")]
  [SerializeField] private GameObject _cinemachineCameraTarget;

  [FoldoutGroup("Settings")]
  [SerializeField] private int _sensitivity = 1;
  [FoldoutGroup("Settings")]
  [SerializeField] private Vector2 _angleRotation = new Vector2(-89, 89);

  //------------------------------------

  private InputHandler inputHandler;

  private float cinemachineTargetYaw;
  private float cinemachineTargetPitch;

  //====================================

  public CinemachineCamera MainCamera => _mainCamera;
  public CinemachineCamera AimCamera => _aimCamera;

  //====================================

  [Inject]
  private void Construct(InputHandler parInputHandler)
  {
    inputHandler = parInputHandler;
  }

  //====================================

  private void Start()
  {
    cinemachineTargetYaw = _cinemachineCameraTarget.transform.rotation.eulerAngles.y;
  }

  private void LateUpdate()
  {
    CameraRotation();
  }

  //====================================

  private void CameraRotation()
  {
    Vector2 look = inputHandler.IInput.Look();

    if (look.sqrMagnitude > 0.01f)
    {
      cinemachineTargetYaw += look.x * _sensitivity;
      cinemachineTargetPitch += look.y * _sensitivity;
    }

    cinemachineTargetYaw = ClampAngle(cinemachineTargetYaw, float.MinValue, float.MaxValue);
    cinemachineTargetPitch = ClampAngle(cinemachineTargetPitch, _angleRotation.x, _angleRotation.y);

    transform.rotation = Quaternion.Euler(cinemachineTargetPitch, cinemachineTargetYaw, 0.0f);
  }

  private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
  {
    if (lfAngle < -360f) lfAngle += 360f;
    if (lfAngle > 360f) lfAngle -= 360f;
    return Mathf.Clamp(lfAngle, lfMin, lfMax);
  }

  //====================================
}