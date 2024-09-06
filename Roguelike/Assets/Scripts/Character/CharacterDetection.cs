using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterDetection : MonoBehaviour
{
  private Camera mainCamera;

  //====================================

  private void Start()
  {
    mainCamera = Camera.main;
  }

  private void Update()
  {
    Detection();
  }

  //====================================

  private void Detection()
  {
    Vector2 screenCenterPosition = new Vector2(Screen.width / 2f, Screen.height / 2f);
    Ray ray = mainCamera.ScreenPointToRay(screenCenterPosition);

    if (!Physics.Raycast(ray, out RaycastHit hit))
      return;

    if (!hit.collider.TryGetComponent(out IInteractable parInteractable))
      return;

    //parInteractable.Interact();
  }

  //====================================
}