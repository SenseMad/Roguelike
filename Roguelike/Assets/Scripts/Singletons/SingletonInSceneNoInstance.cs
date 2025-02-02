using UnityEngine;

public abstract class SingletonInSceneNoInstance<T> : MonoBehaviour where T : MonoBehaviour
{
  private static T _instance;

  //=========================================

  public static T Instance => _instance ?? FindAnyObjectByType<T>();

  //=========================================

  protected void Awake()
  {
    if (_instance != null && _instance != this)
    {
      Destroy(this);
      return;
    }

    _instance = GetComponent<T>();
  }

  //=========================================
}