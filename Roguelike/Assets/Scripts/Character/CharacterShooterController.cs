using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class CharacterShooterController : MonoBehaviour
{
  [SerializeField] private Rig _aimRig;
  [SerializeField] private Transform _point;

  //------------------------------------

  private Character character;

  private Quaternion targetRotation;
  private bool isRotating;

  private float aimRigWeight;

  //====================================

  private void Awake()
  {
    character = GetComponent<Character>();
  }

  private void Update()
  {
    if (character.IsAiming)
    {
      character.Animator.SetLayerWeight(1, Mathf.Lerp(character.Animator.GetLayerWeight(1), 1.0f, Time.deltaTime * 10.0f));

      Vector2 screenCenterPosition = new Vector2(Screen.width / 2f, Screen.height / 2f);
      Ray ray = Camera.main.ScreenPointToRay(screenCenterPosition);
      RaycastHit hit;

      if (Physics.Raycast(ray, out hit))
        _point.position = hit.point;

      aimRigWeight = 1;
    }
    else
    {
      character.Animator.SetLayerWeight(1, Mathf.Lerp(character.Animator.GetLayerWeight(1), 0.0f, Time.deltaTime * 10.0f));

      aimRigWeight = 0;
    }

    _aimRig.weight = Mathf.Lerp(_aimRig.weight, aimRigWeight, Time.deltaTime * 10.0f);

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