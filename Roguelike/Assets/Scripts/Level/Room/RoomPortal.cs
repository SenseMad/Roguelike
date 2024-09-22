using System;
using UnityEngine;

public class RoomPortal : MonoBehaviour
{
  [SerializeField] private bool _isPortalOpen;

  //------------------------------------

  private Collider colliderPortal;

  //====================================

  public event Action OnOpenPortal;

  public event Action OnEnteredPortal;

  //====================================

  private void Awake()
  {
    colliderPortal = GetComponent<Collider>();
  }

  private void Start()
  {
    colliderPortal.enabled = _isPortalOpen;
  }

  //====================================

  public void OpenPortalInvoke()
  {
    _isPortalOpen = true;

    colliderPortal.enabled = _isPortalOpen;

    OnOpenPortal?.Invoke();
  }

  //====================================

  private void OnTriggerEnter(Collider other)
  {
    if (!_isPortalOpen)
      return;

    if (other.GetComponent<Character>() == null)
      return;

    OnEnteredPortal?.Invoke();
  }

  //====================================
}