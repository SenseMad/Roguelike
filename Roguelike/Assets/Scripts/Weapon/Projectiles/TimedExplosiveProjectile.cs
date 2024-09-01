using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedExplosiveProjectile : BaseProjectile
{
  [SerializeField] private float _explosionDelay = 2.0f;
  [SerializeField] private float _explosionRadius;

  //====================================

  protected override void Start()
  {
    base.Start();

    StartCoroutine(ExplosionDelay());
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

  private IEnumerator ExplosionDelay()
  {
    yield return new WaitForSeconds(_explosionDelay);

    Explode();
  }

  //====================================
}