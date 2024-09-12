using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class LevelManager : MonoBehaviour
{
  [SerializeField] private LocationData _currentLocationData;

  //====================================

  public RoomManager RoomManager { get; private set; }

  public LocationData CurrentLocationData { get => _currentLocationData; private set => _currentLocationData = value; }

  //====================================

  [Inject]
  private void Construct(RoomManager parRoomManager)
  {
    RoomManager = parRoomManager;
  }

  //====================================

  private void Start()
  {
    RoomManager.TryPrefabRoom();
  }

  //====================================

  public void Initialize(LocationData parLocationData)
  {
    CurrentLocationData = parLocationData;
  }

  //====================================
}