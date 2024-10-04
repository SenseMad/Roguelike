using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;

public class WeaponInventory : MonoBehaviour
{
  [SerializeField, MinValue(0)] private int _maxAmountStoredWeapons = 2;

  [SerializeField] private Transform _container;

  [SerializeField] private List<Weapon> _listWeapons = new List<Weapon>();

  //====================================

  public Weapon ActiveWeapon { get; private set; }

  //====================================

  public Weapon GetLastWeapon()
  {
    if (_listWeapons.Count == 0)
      return null;

    int previousWeaponIndex = _listWeapons.IndexOf(ActiveWeapon) - 1;
    if (previousWeaponIndex < 0)
      previousWeaponIndex = _listWeapons.Count - 1;

    return _listWeapons[previousWeaponIndex];
  }

  public Weapon GetNextWeapon()
  {
    if (_listWeapons.Count == 0)
      return null;

    int nextWeaponIndex = _listWeapons.IndexOf(ActiveWeapon) + 1;
    if (nextWeaponIndex > _listWeapons.Count - 1)
      nextWeaponIndex = 0;

    return _listWeapons[nextWeaponIndex];
  }

  //====================================

  public Weapon Equip(Weapon parWeapon)
  {
    if (parWeapon == null)
      return ActiveWeapon;

    if (_listWeapons.Count == 0)
      return ActiveWeapon;

    if (!_listWeapons.Contains(parWeapon))
      return ActiveWeapon;

    if (parWeapon == ActiveWeapon)
      return ActiveWeapon;

    if (ActiveWeapon != null)
      ActiveWeapon.gameObject.SetActive(false);

    ActiveWeapon = parWeapon;
    ActiveWeapon.gameObject.SetActive(true);
    ActiveWeapon.WeaponInteractable.SetCollider(false);
    ActiveWeapon.Selected(true);

    return ActiveWeapon;
  }

  public void Add(Weapon parWeapon)
  {
    if (parWeapon == null)
      return;

    if (_listWeapons.Count >= _maxAmountStoredWeapons)
    {
      ReplaceActive(parWeapon);
      return;
    }

    parWeapon.transform.SetParent(_container);
    parWeapon.WeaponPosition.SetPosition();
    _listWeapons.Add(parWeapon);

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
    ActiveWeapon.Selected(false);

    parWeapon.transform.SetParent(_container);
    parWeapon.WeaponPosition.SetPosition();

    int currentWeaponIndex = _listWeapons.IndexOf(ActiveWeapon);
    _listWeapons[currentWeaponIndex] = parWeapon;

    var previousWeapon = ActiveWeapon;

    Equip(parWeapon);

    if (!previousWeapon.gameObject.activeSelf)
      previousWeapon.gameObject.SetActive(true);
  }

  public void Remove(Weapon parWeapon)
  {
    if (parWeapon == null)
      return;

    if (!_listWeapons.Contains(parWeapon))
      return;

    _listWeapons.Remove(parWeapon);
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
    ActiveWeapon.Selected(false);

    _listWeapons.Remove(ActiveWeapon);
    ActiveWeapon = null;

    EquipLastWeapon();
  }

  //====================================

  private void EquipLastWeapon()
  {
    if (_listWeapons.Count == 0)
      return;

    var lastWeapon = _listWeapons[_listWeapons.Count - 1];
    Equip(lastWeapon);
  }

  //====================================
}