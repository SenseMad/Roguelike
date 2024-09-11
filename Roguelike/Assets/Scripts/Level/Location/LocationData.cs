using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LocationData", menuName = "Data/LocationData", order = 51)]
public class LocationData : ScriptableObject
{
  [SerializeField] private Location _location;

  [SerializeField] private List<GameObject> _listRoomPrefabs;

  //====================================

  public Location Location => _location;

  public List<GameObject> ListRoomPrefabs => _listRoomPrefabs;

  //====================================
}