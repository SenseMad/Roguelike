using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using Zenject;

public class RoomManager : MonoBehaviour
{
  private List<Room> listRoomsCreated = new List<Room>();

  //====================================

  public LevelManager LevelManager { get; private set; }

  public int CurrentIndexRoom { get; private set; }

  public Room CurrentRoom { get; private set; }

  //====================================

  public event Action<Room> OnCreatedRoom;

  public event Action<int> OnChangeRoom;

  //====================================

  [Inject]
  private void Construct(LevelManager parLevelManager)
  {
    LevelManager = parLevelManager;
  }

  //====================================

  private void Start()
  {
    CurrentIndexRoom = 0;
  }

  private void OnEnable()
  {
    OnCreatedRoom += CreateRoom;
  }

  private void OnDisable()
  {
    OnCreatedRoom -= CreateRoom;
  }

  private void Update()
  {
    if (Input.GetKeyDown(KeyCode.L))
    {
      RemoveRoom();
    }
  }

  //====================================

  public void CreateRoom(Room parRoom)
  {
    CurrentRoom = Instantiate(parRoom, new Vector3(40, 10, 0), Quaternion.identity);
  }

  public void TryPrefabRoom()
  {
    List<Room> roomPrefabs = LevelManager.CurrentLocationData.ListRoomPrefabs;
    HashSet<Room> createdRooms = new HashSet<Room>(listRoomsCreated);

    if (createdRooms.Count >= roomPrefabs.Count)
    {
      Debug.Log("All rooms have been created");
      return;
    }

    Room newRoom;
    int newIndexRoom;
    int numberAttempts = 0;

    do
    {
      newIndexRoom = Random.Range(0, roomPrefabs.Count);
      newRoom = roomPrefabs[newIndexRoom];
      numberAttempts++;

    } while (createdRooms.Contains(newRoom) && numberAttempts <= roomPrefabs.Count);

    if (!createdRooms.Contains(newRoom))
    {
      listRoomsCreated.Add(newRoom);
      OnCreatedRoom?.Invoke(newRoom);
    }
  }

  public void RemoveRoom()
  {
    if (CurrentRoom != null)
      Destroy(CurrentRoom.gameObject);

    TryPrefabRoom();
  }

  public void ChangeRoom()
  {
    CurrentIndexRoom++;

    OnChangeRoom?.Invoke(CurrentIndexRoom);
  }

  //====================================
}