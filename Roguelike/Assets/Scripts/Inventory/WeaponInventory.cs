using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;

public class WeaponInventory : MonoBehaviour
{
  [SerializeField, MinValue(0)] private int _maxAmountStoredWeapons = 2;

  [SerializeField] private Transform _container;

  //------------------------------------

  private List<Weapon> listWeapons = new List<Weapon>();

  //====================================

  public Weapon ActiveWeapon { get; private set; }

  //====================================

  public Weapon GetLastWeapon()
  {
    if (listWeapons.Count == 0)
      return null;

    int previousWeaponIndex = listWeapons.IndexOf(ActiveWeapon) - 1;
    if (previousWeaponIndex < 0)
      previousWeaponIndex = listWeapons.Count - 1;

    return listWeapons[previousWeaponIndex];
  }

  public Weapon GetNextWeapon()
  {
    if (listWeapons.Count == 0)
      return null;

    int nextWeaponIndex = listWeapons.IndexOf(ActiveWeapon) + 1;
    if (nextWeaponIndex > listWeapons.Count - 1)
      nextWeaponIndex = 0;

    return listWeapons[nextWeaponIndex];
  }

  //====================================

  public Weapon Equip(Weapon parWeapon)
  {
    if (parWeapon == null)
      return ActiveWeapon;

    if (listWeapons.Count == 0)
      return ActiveWeapon;

    if (!listWeapons.Contains(parWeapon))
      return ActiveWeapon;

    if (parWeapon == ActiveWeapon)
      return ActiveWeapon;

    if (ActiveWeapon != null)
      ActiveWeapon.gameObject.SetActive(false);

    ActiveWeapon = parWeapon;
    ActiveWeapon.gameObject.SetActive(true);
    ActiveWeapon.WeaponInteractable.SetCollider(false);

    return ActiveWeapon;
  }

  public void Add(Weapon parWeapon)
  {
    if (parWeapon == null)
      return;

    if (listWeapons.Count >= _maxAmountStoredWeapons)
    {
      ReplaceActive(parWeapon);
      return;
    }

    parWeapon.transform.SetParent(_container);
    parWeapon.WeaponPosition.SetPosition();
    listWeapons.Add(parWeapon);

    Equip(parWeapon);
  }

  public void ReplaceActive(Weapon parWeapon)
  {
    if (parWeapon == null)
      return;

    Vector3 lastWeaponPosition = parWeapon.transform.position;
    Quaternion lastWeaponRotation = parWeapon.transform.rotation;

    ActiveWeapon.transform.SetParent(null);
    ActiveWeapon.transform.SetPositionAndRotation(lastWeaponPosition, lastWeaponRotation);
    ActiveWeapon.WeaponInteractable.SetCollider(true);

    parWeapon.transform.SetParent(_container);
    parWeapon.WeaponPosition.SetPosition();

    int currentWeaponIndex = listWeapons.IndexOf(ActiveWeapon);
    listWeapons[currentWeaponIndex] = parWeapon;

    var previousWeapon = ActiveWeapon;

    Equip(parWeapon);

    if (!previousWeapon.gameObject.activeSelf)
      previousWeapon.gameObject.SetActive(true);
  }

  public void Remove(Weapon parWeapon)
  {
    if (parWeapon == null)
      return;

    if (!listWeapons.Contains(parWeapon))
      return;

    listWeapons.Remove(parWeapon);
    Destroy(ActiveWeapon.gameObject);
    ActiveWeapon = null;

    EquipLastWeapon();
  }

  public void DropActive(Vector3 parDropPosition)
  {
    if (ActiveWeapon == null)
      return;

    ActiveWeapon.transform.SetParent(null);
    ActiveWeapon.transform.position = parDropPosition;
    ActiveWeapon.WeaponInteractable.SetCollider(true);

    listWeapons.Remove(ActiveWeapon);
    ActiveWeapon = null;

    EquipLastWeapon();
  }

  //====================================

  private void EquipLastWeapon()
  {
    if (listWeapons.Count == 0)
      return;

    var lastWeapon = listWeapons[listWeapons.Count - 1];
    Equip(lastWeapon);
  }

  //====================================
}