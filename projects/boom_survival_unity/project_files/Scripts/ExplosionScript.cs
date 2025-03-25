using UnityEngine;
using System.Collections;

/*
 * This class is responsible for playing explosion animation and sound.
 */
public class ExplosionScript : MonoBehaviour
{
    // "[SerializeField]" allows private fields to be visible and editable in
    // the Unity Inspector.
    [SerializeField] private float lifetime;

    private AudioSource audioSource;
    private HelperFunctions helperFunctions;
    private GameObject player;

    /*
     * Start is called before the first frame update
     */ 
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        player = GameObject.FindWithTag("Player");

        // This combats audiochannels from getting overtasked when many sounds are playing
        if (player != null && !HelperFunctions.IsObjectNearPlayer(player.gameObject,
         this.gameObject, 27f))
        {
            audioSource.mute = true;
        }
        StartCoroutine(destroyAfter());
    }
    
    /*
     * Wait for the animation to play then destroy self
     */ 
    IEnumerator destroyAfter() {
        yield return new WaitForSeconds(lifetime);
        
        Renderer renderer = GetComponent<Renderer>();
        renderer.enabled = false;

        Destroy (gameObject, 1);
    }
}
