using UnityEngine;

public class WeaponInteractable : MonoBehaviour, IInteractable
{
  private Weapon weapon;

  private Character character;

  private Collider interactCollider;

  //====================================

  private void Awake()
  {
    character = Character.Instance;

    weapon = GetComponentInParent<Weapon>();

    interactCollider = GetComponent<Collider>();
  }

  //====================================

  public void Interact()
  {
    character.WeaponInventory.Add(weapon);

    weapon.Selected(true);

    SetCollider(false);
  }

  public void SetCollider(bool parValue)
  {
    interactCollider.enabled = parValue;
  }

  //====================================
}