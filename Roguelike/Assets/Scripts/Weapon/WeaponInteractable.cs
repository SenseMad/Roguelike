using UnityEngine;
using Zenject;

public class WeaponInteractable : MonoBehaviour, IInteractable
{
  private Weapon weapon;
  private Character character;

  private Collider interactCollider;

  //====================================

  private void Awake()
  {
    weapon = GetComponentInParent<Weapon>();

    interactCollider = GetComponent<Collider>();
  }

  //====================================

  [Inject]
  private void Construct(Character parCharacter)
  {
    character = parCharacter;
  }

  //====================================

  public void Interact()
  {
    character.WeaponInventory.Add(weapon);

    SetCollider(false);
  }

  public void SetCollider(bool parValue)
  {
    interactCollider.enabled = parValue;
  }

  //====================================
}