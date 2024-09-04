using UnityEngine;

public class CharacterShooterRotationController : MonoBehaviour
{
  private Character character;

  private Quaternion targetRotation;
  private bool isRotating;

  //====================================

  private void Awake()
  {
    character = GetComponent<Character>();
  }

  private void Update()
  {
    if ((character.IsAiming && !isRotating) || (character.IsShoot && !isRotating))
      StartRotation();

    RotateTowardsTarget();
  }

  //====================================

  private void StartRotation()
  {
    Vector3 forwardDirection = character.CameraController.MainCamera.transform.forward;
    forwardDirection.y = 0;
    targetRotation = Quaternion.LookRotation(forwardDirection);

    float angle = Quaternion.Angle(transform.rotation, targetRotation);

    bool shouldShoot = Mathf.Abs(angle) < 150.0f;

    isRotating = true;

    character.WeaponInventory.ActiveWeapon.TryShoot(shouldShoot && character.IsShoot && isRotating);
  }

  private void RotateTowardsTarget()
  {
    if (!isRotating)
      return;

    if (Quaternion.Angle(transform.rotation, targetRotation) > 60.0f)
    {
      transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10);
    }
    else
    {
      character.transform.rotation = targetRotation;

      isRotating = false;
    }
  }

  //====================================
}