using UnityEngine;

public class DirectProjectile : BaseProjectile
{
  protected override void Collide(Collision collision)
  {
    if (collision.gameObject != owner)
    {
      if (collision.collider.TryGetComponent(out IDamageable parIDamageable))
      {
        parIDamageable.TakeDamage(damage);
      }
    }

    base.Collide(collision);

    Destroy(gameObject);
  }

  //====================================
}