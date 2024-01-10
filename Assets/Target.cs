
using UnityEngine;

public class Target : MonoBehaviour
{
  public LayerMask layerMask;
  public float health = 50f;

  public void TakeDamage(float amount)
  {
    health -= amount;
    if (health <= 0f)
    {
      Die();
    }
  }

  public void Die()
  {
    Destroy(gameObject);
  }
}
