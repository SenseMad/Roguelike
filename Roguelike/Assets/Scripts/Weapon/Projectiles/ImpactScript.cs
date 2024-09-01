using UnityEngine;

using Sirenix.OdinInspector;

public class ImpactScript : MonoBehaviour
{
  [FoldoutGroup("Time")]
  [SerializeField] private float _timeDestroyEffect = 10.0f;

  [FoldoutGroup("Sound")]
  [SerializeField] private AudioClip _soundImpact;
  [SerializeField] private AudioSource _audioSource;

  //====================================

  private void Start()
  {
    CreateImpact();
  }

  //====================================

  private void CreateImpact()
  {
    if (_audioSource != null && _soundImpact != null)
    {
      _audioSource.clip = _soundImpact;
      _audioSource.Play();
    }

    Destroy(gameObject, _timeDestroyEffect);
  }

  //====================================
}