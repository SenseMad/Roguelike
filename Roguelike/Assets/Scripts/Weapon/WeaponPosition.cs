using UnityEngine;

public class WeaponPosition : MonoBehaviour
{
  [SerializeField] private Vector3 _position;
  [SerializeField] private Vector3 _rotation;

  //====================================

  public Vector3 Position => _position;

  public Vector3 Rotation => _rotation;

  //====================================

  private void Awake()
  {
    //transform.localPosition = _position;
    //transform.localRotation = Quaternion.Euler(_rotation);
  }

  //====================================
}