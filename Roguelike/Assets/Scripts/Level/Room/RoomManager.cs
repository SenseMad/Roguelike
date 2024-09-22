using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using Zenject;

public class RoomManager : SingletonInSceneNoInstance<RoomManager>
{
  [SerializeField] private float _roomLoadingTime = 2.0f;

  //------------------------------------

  private Character character;

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

  protected new void Awake()
  {
    base.Awake();

    character = Character.Instance;
  }

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

  //====================================

  public void CreateRoom(Room parRoom)
  {
    CurrentRoom = Instantiate(parRoom, new Vector3(40, 10, 0), Quaternion.identity);

    CurrentRoom.RoomPortal.OnEnteredPortal += RemoveRoom;

    character.Controller.enabled = false;
    character.transform.position = CurrentRoom.SpawnPointCharacter.position;
    character.transform.rotation = Quaternion.Euler(Vector3.zero);
    character.Controller.enabled = true;
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
      StartCoroutine(ChangeRoom(newRoom));
      //OnCreatedRoom?.Invoke(newRoom);
    }
  }

  public void RemoveRoom()
  {
    if (CurrentRoom != null)
    {
      CurrentRoom.RoomPortal.OnEnteredPortal -= RemoveRoom;
      Destroy(CurrentRoom.gameObject);
    }

    TryPrefabRoom();
  }

  /*public void ChangeRoom()
  {
    CurrentIndexRoom++;

    OnChangeRoom?.Invoke(CurrentIndexRoom);
  }*/

  //====================================

  public IEnumerator ChangeRoom(Room parRoom)
  {
    // Затемнение экрана
    character.Controller.enabled = false;

    yield return new WaitForSeconds(_roomLoadingTime);

    OnCreatedRoom?.Invoke(parRoom);
    // Убрать затемнение экрана
  }

  //====================================
}