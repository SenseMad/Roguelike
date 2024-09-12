using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LocationData", menuName = "Data/LocationData", order = 51)]
public class LocationData : ScriptableObject
{
  [SerializeField] private Location _location;

  [SerializeField] private int _numberRooms;

  [SerializeField] private List<Room> _listRoomPrefabs;

  //====================================

  public Location Location => _location;

  public int NumberRooms => _numberRooms;

  public List<Room> ListRoomPrefabs => _listRoomPrefabs;

  //====================================
}