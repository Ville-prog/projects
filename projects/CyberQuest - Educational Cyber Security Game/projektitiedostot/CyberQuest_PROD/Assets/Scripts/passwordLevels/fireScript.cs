using UnityEngine;

public class fireScript : MonoBehaviour
{
    [SerializeField] private spaceshipScript spaceship; // Reference to the spaceshipScript
    [SerializeField] private GameObject projectilePrefab; // The projectile prefab to instantiate
    [SerializeField] private Transform spawnPoint; // The spawn point for the projectile
   
    [SerializeField] private float projectileSpeed = 10f; // Speed of the projectile
    [SerializeField] private float fireCooldown = 0.3f; // Cooldown time in seconds

    [SerializeField] private AudioSource audioSource;

    private float lastFireTime = 0f; // Tracks the time of the last shot

    void Update()
    {
        // Check if the spacebar is pressed and the cooldown has elapsed
        if (Input.GetKeyDown(KeyCode.Space) && Time.time >= lastFireTime + fireCooldown)
        {
            FireProjectile();
            lastFireTime = Time.time; // Update the time of the last shot
        }
    }

    private void FireProjectile()
    {
        
        // Offset the spawn point's position by +1.5 on the Y-axis
        Vector3 offsetPosition = spawnPoint.position + new Vector3(0, 1.5f, 0);

        // Instantiate the projectile at the offset position
        GameObject projectile = Instantiate(projectilePrefab, offsetPosition, Quaternion.identity);

        // Set the projectile's rotation to z = 90
        projectile.transform.rotation = Quaternion.Euler(0, 0, 90);

        // Add upward velocity to the projectile
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        rb.linearVelocity = Vector2.up * projectileSpeed;
        
        // Trigger the recoil effect on the spaceship
        spaceship.TriggerRecoil(0.4f, 0.3f); // Adjust recoil distance and duration as needed
        
        audioSource.Play();

    }
}