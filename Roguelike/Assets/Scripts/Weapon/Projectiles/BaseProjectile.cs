using UnityEngine;

using Sirenix.OdinInspector;

[RequireComponent(typeof(Rigidbody))]
public class BaseProjectile : MonoBehaviour
{
  [FoldoutGroup("Time")]
  [SerializeField] private float _destroyAfter = 15.0f;

  [FoldoutGroup("Effect")]
  [SerializeField] private Transform _hitEffectPrefab;

  //------------------------------------

  protected GameObject owner;
  protected int damage;

  //====================================

  public Rigidbody BodyRB { get; private set; }

  //====================================

  private void Awake()
  {
    BodyRB = GetComponent<Rigidbody>();
  }

  protected virtual void Start()
  {
    Destroy(gameObject, _destroyAfter);
  }

  protected virtual void OnCollisionEnter(Collision collision)
  {
    Collide(collision);
  }

  //====================================

  /// <summary>
  /// 
  /// </summary>
  /// <param name="parSpeed">Projectile velocity</param>
  /// <param name="parDamage">Projectile damage</param>
  /// <param name="parOwner">Who fired the projectile</param>
  public virtual void Initialize(int parDamage, GameObject parOwner)
  {
    damage = parDamage;
    owner = parOwner;
  }

  protected virtual void Collide(Collision collision)
  {
    CreateHitEffect(collision);
  }

  //====================================

  private void CreateHitEffect(Collision parCollision)
  {
    if (_hitEffectPrefab == null)
      return;

    Instantiate(_hitEffectPrefab, transform.position, Quaternion.LookRotation(parCollision.contacts[0].normal));
  }

  //====================================
}