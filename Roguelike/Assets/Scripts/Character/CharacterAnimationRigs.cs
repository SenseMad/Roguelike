using UnityEngine;
using UnityEngine.Animations.Rigging;

public class CharacterAnimationRigs : MonoBehaviour
{
  [SerializeField] private Rig _aimRifleRig;
  [SerializeField] private Rig _idleRifleRig;
  [SerializeField] private Transform _point;
  [SerializeField] private float _speedWeight = 10.0f;

  //------------------------------------

  private Character character;

  private float aimRifleRigWeight;
  private float idleRifleRigWeight;

  //====================================

  private void Awake()
  {
    character = GetComponent<Character>();
  }

  private void Update()
  {
    AimRig();
  }

  //====================================

  private void AimRig()
  {
    if (character.WeaponInventory.ActiveWeapon == null)
    {
      aimRifleRigWeight = 0;
      idleRifleRigWeight = 0;

      if (character.Animator.GetLayerWeight(1) != 0)
        character.Animator.SetLayerWeight(1, Mathf.Lerp(character.Animator.GetLayerWeight(1), 0.0f, Time.deltaTime * _speedWeight));
    }
    else
    {
      if (character.IsAiming)
      {
        Vector2 screenCenterPosition = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPosition);
        RaycastHit hit;

        Vector3 targetPoint;

        if (Physics.Raycast(ray, out hit))
          targetPoint = hit.point;
        else
          targetPoint = ray.GetPoint(50);

        _point.position = Vector3.Lerp(_point.position, targetPoint, Time.deltaTime * _speedWeight);

        idleRifleRigWeight = 0;
        aimRifleRigWeight = 1;
      }
      else
      {
        idleRifleRigWeight = 1;
        aimRifleRigWeight = 0;
      }
      character.Animator.SetLayerWeight(1, Mathf.Lerp(character.Animator.GetLayerWeight(1), 1f, Time.deltaTime * _speedWeight));
    }

    _idleRifleRig.weight = Mathf.Lerp(_idleRifleRig.weight, idleRifleRigWeight, Time.deltaTime * _speedWeight);
    _aimRifleRig.weight = Mathf.Lerp(_aimRifleRig.weight, aimRifleRigWeight, Time.deltaTime * _speedWeight);

    character.Animator.SetBool("IsAiming", character.IsAiming);
  }

  //====================================
}