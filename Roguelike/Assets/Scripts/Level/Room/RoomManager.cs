using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using Zenject;

using Sirenix.OdinInspector;

public class RoomManager : SingletonInSceneNoInstance<RoomManager>
{
  [SerializeField] private float _roomLoadingTime = 2.0f;

  [FoldoutGroup("UI")]
  [SerializeField] private RoomUI _roomUI;

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

  public event Action OnRoomAreOver;

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
      StartCoroutine(ChangeRoom(newRoom));
    }
  }

  public void RemoveRoom()
  {
    List<Room> roomPrefabs = LevelManager.CurrentLocationData.ListRoomPrefabs;

    if (CurrentRoom != null)
      CurrentRoom.RoomPortal.OnEnteredPortal -= RemoveRoom;

    if (listRoomsCreated.Count >= roomPrefabs.Count)
    {
      Debug.Log("All rooms have been created");
      OnRoomAreOver?.Invoke();
      return;
    }

    TryPrefabRoom();
  }

  //====================================

  public IEnumerator ChangeRoom(Room parRoom)
  {
    character.Controller.enabled = false;
    _roomUI.SetActive(true);

    while (CurrentRoom != null && _roomUI.IsActive)
      yield return null;

    Destroy(CurrentRoom.gameObject);

    listRoomsCreated.Add(parRoom);
    OnCreatedRoom?.Invoke(parRoom);

    _roomUI.SetActive(false);

    while (_roomUI.IsActive)
      yield return null;

    character.Controller.enabled = true;

    character.OnLoadedInvoke();
  }

  //====================================
}