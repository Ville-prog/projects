using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PortalController : MonoBehaviour
{   

    public Sprite portalClosed;
    public Sprite portalOpen;

    private SpriteRenderer spriteRenderer;

    bool puzzleSolved;
    public CaesarScript script;

    private BoxCollider2D portalCollider;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {   

        // Get Objects to manipulate
        spriteRenderer = GetComponent<SpriteRenderer>();
        portalCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if Player has solved the puzzle
        puzzleSolved = script.getSolvedorNot();

        if (puzzleSolved){

            // Open the Portal / Door
            spriteRenderer.sprite = portalOpen;
            portalCollider.isTrigger = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other){

        if (other.tag == "Player"){

            spriteRenderer.sortingOrder = 11;
        }
    }

    private void OnTriggerExit2D(Collider2D other){

        if (other.tag == "Player"){

            spriteRenderer.sortingOrder = 5;
        }
    }
}
