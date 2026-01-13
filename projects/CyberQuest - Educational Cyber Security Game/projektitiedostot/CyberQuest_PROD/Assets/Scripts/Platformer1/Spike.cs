using UnityEngine;

public class Spike : MonoBehaviour
{
    public int damageAmount = 1;

  private void OnCollisionEnter2D(Collision2D collision)
  {
    Debug.Log("SPIKE HIT THE PLAYER");
    PlayerHealth playerHealth = collision.collider.GetComponent<PlayerHealth>();
    if(playerHealth != null){
      playerHealth.TakeDamage(damageAmount);
    }
  }
}
