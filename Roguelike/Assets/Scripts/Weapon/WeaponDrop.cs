using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDrop : MonoBehaviour
{
  [SerializeField] private Weapon _weaponInHand;
  [SerializeField] private GameObject _weaponPrefab;
  [SerializeField] private float _dropHeight = 0.5f;
  [SerializeField] private float _rayDistance = 5f;

  //====================================

  private void Update()
  {
    if (Input.GetKeyDown(KeyCode.P))
    {
      DropWeapon();
    }
  }

  //====================================

  private void DropWeapon()
  {
    if (_weaponInHand == null)
      return;

    _weaponInHand.gameObject.SetActive(false);

    Ray ray = new Ray(transform.position, Vector3.down);
    RaycastHit hit;

    if (Physics.Raycast(ray, out hit, _rayDistance))
    {
      // Если нашли поверхность под игроком, сбрасываем оружие над ней
      Vector3 dropPosition = hit.point + Vector3.up * _dropHeight;

      // Создаем копию оружия над поверхностью
      GameObject droppedWeapon = Instantiate(_weaponPrefab, dropPosition, Quaternion.identity);

      // Убедитесь, что скрипт вращения добавлен к префабу оружия
    }
    else
    {
      Debug.Log("Поверхность под игроком не найдена.");
    }
  }

  //====================================
}