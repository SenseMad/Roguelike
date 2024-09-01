using UnityEngine;

public class ExplosiveProjectile : BaseProjectile
{
  [SerializeField] private float _explosionRadius;

  //====================================

  protected override void Collide(Collision collision)
  {
    Explode();

    base.Collide(collision);

    Destroy(gameObject);
  }

  //====================================

  private void Explode()
  {
    Collider[] hitColliders = Physics.OverlapSphere(transform.position, _explosionRadius);
    foreach (var hitCollider in hitColliders)
    {
      if (hitCollider.gameObject == owner)
        continue;

      if (hitCollider.TryGetComponent(out IDamageable parIDamageable))
        parIDamageable.TakeDamage(damage);
    }
  }

  //====================================
}