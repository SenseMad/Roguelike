using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
  [SerializeField] private LocationData _currentLocationData;

  //====================================

  public RoomManager RoomManager { get; private set; }

  public LocationData CurrentLocationData { get => _currentLocationData; private set => _currentLocationData = value; }

  //====================================

  private void Awake()
  {
    RoomManager = RoomManager.Instance;
  }

  //====================================

  private void Start()
  {
    RoomManager.CreateRoom(_currentLocationData.InitialRoom);
  }

  //====================================

  public void Initialize(LocationData parLocationData)
  {
    CurrentLocationData = parLocationData;
  }

  //====================================
}