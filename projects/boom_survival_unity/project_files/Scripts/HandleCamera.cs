using UnityEngine;
using System.Collections;

/*
 * This class is responsible for managing the gamecamera's behavior.
 * Follows the player and initiates a screen shake effect when needed.
 * Also plays a ear ringing soundeffect.
 */
public class HandleCamera : MonoBehaviour
{
    public GameObject animate_explosion;

    // "[SerializeField]" allows private fields to be visible and editable in
    // the Unity Inspector.
    [SerializeField] private float baseShakeDuration; 
    [SerializeField] private float baseShakeMagnitude; 

    private GameObject player;
    private Camera mainCamera;
    private Vector3 shakeOffset;

    /*
     * Start is called before the first frame update
     */
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        mainCamera = Camera.main;
    }

    /*
     * Update is called once per frame
     */
    void Update()
    {
        // Used to follow player and shake camera when necessary
        transform.position = player.transform.position +
         new Vector3(0, 1, -15) + shakeOffset;
        
    }

    /*
     * Iniate screen shake based on distance to expolsion
     */ 
    public void ShakeCamera()
    {
        if (HelperFunctions.IsObjectNearPlayer(animate_explosion, player, 20f))
        {
            float distance = Vector3.Distance(player.transform.position,
             animate_explosion.transform.position);

            float duration = baseShakeDuration; 
            float magnitude = baseShakeMagnitude / Mathf.Max(distance, 1f);

            StartCoroutine(Shake(duration, magnitude));
        }
    }

    /*
     * Shake the camera for a specified duration and magnitude
     */  
    private IEnumerator Shake(float duration, float magnitude)
    {
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;
            shakeOffset = new Vector3(x, y, 0);
            elapsed += Time.deltaTime;
            yield return null;
        }
        shakeOffset = Vector3.zero;
    }

    /*
     * Plays ear ringing soundeffect
     */ 
    public void playEarRing() {
        if (HelperFunctions.IsObjectNearPlayer(animate_explosion, player, 3f)) 
        {
            AudioSource audioSource = this.GetComponent<AudioSource>();  
            audioSource.Play();      
        }    
    }
}
