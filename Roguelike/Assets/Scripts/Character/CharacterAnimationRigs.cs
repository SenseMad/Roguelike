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
  private Camera aimCamera;

  private float aimRifleRigWeight;
  private float idleRifleRigWeight;

  private bool isShootingHip;

  //====================================

  private void Awake()
  {
    character = GetComponent<Character>();
  }

  private void Start()
  {
    aimCamera = Camera.main;
  }

  private void Update()
  {
    if (character.WeaponInventory.ActiveWeapon == null)
    {
      character.Animator.SetLayerWeight(1, Mathf.Lerp(character.Animator.GetLayerWeight(1), 0.0f, Time.deltaTime * _speedWeight));

      isShootingHip = false;
    }
    else
    {
      character.Animator.SetLayerWeight(1, Mathf.Lerp(character.Animator.GetLayerWeight(1), 1f, Time.deltaTime * _speedWeight));

      isShootingHip = character.WeaponInventory.ActiveWeapon.CanShoot;

      AimingActions();
      ShootingActions();

      character.Animator.SetBool("IsAiming", (character.IsAiming || isShootingHip));
    }

    RifleRigWeight();
  }

  //====================================

  private void GetAimingPoint()
  {
    Vector2 screenCenterPosition = new Vector2(Screen.width / 2f, Screen.height / 2f);
    Ray ray = aimCamera.ScreenPointToRay(screenCenterPosition);
    RaycastHit hit;

    Vector3 targetPosition;

    if (Physics.Raycast(ray, out hit))
      targetPosition = hit.point;
    else
      targetPosition = ray.GetPoint(50);

    _point.position = Vector3.Lerp(_point.position, targetPosition, Time.deltaTime * _speedWeight * 2.0f);
  }

  private void AimingActions()
  {
    if (character.WeaponInventory.ActiveWeapon == null)
      return;

    if (character.IsAiming)
    {
      GetAimingPoint();
      return;
    }
  }

  private void ShootingActions()
  {
    if (character.WeaponInventory.ActiveWeapon == null)
      return;

    if (character.IsAiming)
      return;

    if (character.WeaponInventory.ActiveWeapon.CanShoot)
    {
      GetAimingPoint();
      return;
    }
  }

  private void RifleRigWeight()
  {
    var activeWeapon = character.WeaponInventory.ActiveWeapon;
    bool rifleWeight = (character.IsAiming || isShootingHip) && (activeWeapon != null);

    idleRifleRigWeight = rifleWeight ? 0 : 1;
    aimRifleRigWeight = !rifleWeight ? 0 : 1;

    if (activeWeapon == null || character.IsRecharge)
    {
      idleRifleRigWeight = 0;
      aimRifleRigWeight = 0;
    }

    _idleRifleRig.weight = Mathf.Lerp(_idleRifleRig.weight, idleRifleRigWeight, Time.deltaTime * _speedWeight);
    _aimRifleRig.weight = Mathf.Lerp(_aimRifleRig.weight, aimRifleRigWeight, Time.deltaTime * _speedWeight);
  }

  //====================================
}