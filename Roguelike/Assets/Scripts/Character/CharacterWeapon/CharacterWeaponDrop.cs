using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CharacterWeaponDrop : MonoBehaviour
{
  [SerializeField] private float _dropHeight = 0.5f;
  [SerializeField] private float _rayDistance = 5f;

  //------------------------------------

  private Character character;

  private InputHandler inputHandler;

  //====================================

  [Inject]
  private void Construct(InputHandler parInputHandler)
  {
    inputHandler = parInputHandler;
  }

  //====================================

  private void Awake()
  {
    character = Character.Instance;
  }

  private void Update()
  {
    if (Input.GetKeyDown(KeyCode.P))
    {
      WeaponDrop();
    }
  }

  //====================================

  public void WeaponDrop()
  {
    Ray ray = new Ray(transform.position, Vector3.down);
    RaycastHit hit;

    if (Physics.Raycast(ray, out hit, _rayDistance))
    {
      Vector3 dropPosition = hit.point + Vector3.up * _dropHeight;

      character.WeaponInventory.DropActive(dropPosition);
    }
    else
    {
      Debug.Log("Поверхность под игроком не найдена.");
    }
  }

  //====================================



  //====================================



  //====================================



  //====================================
}