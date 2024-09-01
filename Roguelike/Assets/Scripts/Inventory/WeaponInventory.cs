using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;

public class WeaponInventory : MonoBehaviour
{
  [SerializeField, MinValue(0)] private int _maxAmountStoredWeapons = 2;

  //------------------------------------

  private Weapon[] weapons;

  //====================================

  public Weapon ActiveWeapon { get; private set; }

  public int ActiveWeaponIndex { get; private set; } = -1;

  //====================================

  private void Start()
  {
    weapons = new Weapon[_maxAmountStoredWeapons];

    Initialize(0);
  }

  //====================================

  public void Initialize(int parIndexStartingWeapon = 0)
  {
    Weapon[] tempWeapons = GetComponentsInChildren<Weapon>(true);

    for (int i = 0; i < tempWeapons.Length; i++)
    {
      weapons[i] = tempWeapons[i];
    }

    Equip(parIndexStartingWeapon);
  }

  public Weapon Equip(int parIndex)
  {
    if (weapons == null)
      return ActiveWeapon;

    if (parIndex > weapons.Length - 1 || parIndex < 0)
      return ActiveWeapon;

    if (parIndex == ActiveWeaponIndex)
      return ActiveWeapon;

    ActiveWeaponIndex = parIndex;
    ActiveWeapon = weapons[parIndex];
    ActiveWeapon.gameObject.SetActive(true);

    return ActiveWeapon;
  }

  public void Add(Weapon parWeapon)
  {
    if (weapons.Length - 1 >= _maxAmountStoredWeapons)
    {
      ReplaceActive(parWeapon);
      return;
    }

    for (int i = 0; i < weapons.Length - 1; i++)
    {
      if (weapons[i] != null)
        continue;

      weapons[i] = parWeapon;
      Equip(i);
      break;
    }
  }

  public void Remove(Weapon parWeapon)
  {
    for (int i = 0; i < weapons.Length - 1; i++)
    {
      if (weapons[i] != parWeapon)
        continue;

      //weapons[i] = null;
      Destroy(weapons[i]);
      break;
    }

    for (int i = weapons.Length - 1; i > 0; i--)
    {
      if (weapons[i] == null)
        continue;

      Equip(i);
      break;
    }
  }

  public void ReplaceActive(Weapon parWeapon)
  {
    for (int i = 0; i < weapons.Length - 1; i++)
    {
      if (weapons[i] == null)
        continue;

      weapons[i] = parWeapon;
      Equip(i);
      break;
    }
  }

  //====================================
}