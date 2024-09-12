using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
  [SerializeField] private LocationData _locationData;

  //====================================

  public RoomManager RoomManager { get; private set; }

  //====================================



  //====================================



  //====================================

  public void Initialize(LocationData parLocationData)
  {
    _locationData = parLocationData;
  }

  //====================================
}