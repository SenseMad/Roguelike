using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{


  //====================================

  public int CurrentIndexRoom { get; private set; }

  //====================================

  public event Action<int> OnChangeRoom;

  //====================================

  private void Start()
  {
    CurrentIndexRoom = 0;
  }

  //====================================

  public void ChangeRoom()
  {
    CurrentIndexRoom++;

    OnChangeRoom?.Invoke(CurrentIndexRoom);
  }

  //====================================
}