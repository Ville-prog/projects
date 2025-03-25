using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProjectileScript : MonoBehaviour
{
    public float explosionRadius; 
    public int damage; 
    public float pushForce;
    public LayerMask layerMask; 
    public GameObject explosionPrefab;
    public float fuseTimer;
    
    private AudioSource _fuseSound;
    private bool hasExploded = false;
    
    // SIIRRÄ!
    private List<GameObject> blocksToDestroy = new List<GameObject>();
    private List<Vector2> raycastDirections = new List<Vector2>();
    private List<Rigidbody2D> objectsToPush = new List<Rigidbody2D>();
    private List<Vector2> pushDirections = new List<Vector2>();

    void Start()
    {
        _fuseSound = GetComponent<AudioSource>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collided object has a Block component
        Block block = collision.gameObject.GetComponent<Block>();
        if (block != null)
        {
            // Start the timer coroutine
            StartCoroutine(TimerCoroutine());
        }
    }

    IEnumerator TimerCoroutine()
    {
        // Wait for the fuse timer before initiating the explosion
        yield return new WaitForSeconds(fuseTimer);
    
        if (!hasExploded)
        {
            hasExploded = true;
    
            // Play explosion animation
            GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            explosion.transform.localScale = new Vector3(explosionRadius * 0.7f, explosionRadius * 0.7f, 1f);
            _fuseSound.Stop();
            Renderer renderer = GetComponent<Renderer>();
            renderer.enabled = false;
    
            // Shake effect for camera
            HandleCamera handleCamera = FindObjectOfType<HandleCamera>();
            handleCamera.animate_explosion = explosion;
            handleCamera.ShakeCamera();
            handleCamera.playEarRing();

            // Cast rays around to bomb in a circle
            int segments = 36; 
            float angleStep = 360f / segments; 
    
            // Raycasts exclude other bombs and background blocks
            int bombLayer = LayerMask.NameToLayer("BombLayer");
            int backgroundLayer = LayerMask.NameToLayer("Background");
            int excludeLayer = ~(1 << bombLayer | 1 << backgroundLayer);

            for (int i = 0; i < segments; i++)
            {
                // Calculate the direction for the current ray
                float angle = angleStep * i;
                Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
    
                // Perform the raycast in the calculated direction
                RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, direction, explosionRadius, excludeLayer);
    
                bool blockHit = false;
    
                foreach (RaycastHit2D hit in hits)
                {
                    if (hit.collider != null)
                    {
                        // Check if the hit object is a block
                        Block block = hit.collider.GetComponent<Block>();
                        if (block != null)
                        {
                            blockHit = true; // Mark that a block has been hit
    
                            // Apply push-away force
                            Rigidbody2D rb = hit.collider.GetComponent<Rigidbody2D>();
                            if (rb != null)
                            {
                                Vector2 pushDirection = (rb.transform.position - transform.position).normalized;
                                objectsToPush.Add(rb);
                                pushDirections.Add(pushDirection);
                            }
                            block.hp -= damage;
                        }
    
                        // Check if the hit object is a player by its layer
                        if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
                        {
                            if (!blockHit) // Only apply damage if no block has been hit
                            {
                                // Apply push-away force
                                Rigidbody2D rb = hit.collider.GetComponent<Rigidbody2D>();
                                if (rb != null)
                                {
                                    Vector2 pushDirection = (rb.transform.position - transform.position).normalized;
                                    rb.AddForce(pushDirection * pushForce * 2, ForceMode2D.Force);
    
                                    // Set the external force flag on the player
                                    TarodevController.PlayerController player = hit.collider.GetComponent<TarodevController.PlayerController>();
                                    int currentHp = player.getHp();
                                    player.setHp(currentHp - damage);
                                }
                            }
                        }
                    }
                }
            }
    
            // Apply push force to all collected objects
            for (int i = 0; i < objectsToPush.Count; i++)
            {
                if (objectsToPush[i] != null)
                {
                    objectsToPush[i].AddForce(pushDirections[i] * pushForce, ForceMode2D.Impulse);
                }
            }
    
            Destroy(gameObject);
        }
    }
}
