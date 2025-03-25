using UnityEngine;
using System.Collections;

public class Block : MonoBehaviour
{
    public Color blockColor;
    public int hp;
    private  bool isBackround = false;

    public void InitializeBlock(Sprite sprite, int hp, float mass, bool isBackround)
    {
        GetComponent<SpriteRenderer>().sprite = sprite;
        this.hp = hp;
        this.isBackround = isBackround;
        
        // Set the mass of the Rigidbody2D component
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.mass = mass;
        
        if (isBackround)
        {
            gameObject.layer = LayerMask.NameToLayer("Background");
            rb.bodyType = RigidbodyType2D.Static;

            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sortingLayerName = "Background";
        }
    }


    // Update is called once per frame
        void Update()
    {
        if (hp <= 0) {
            Destroy(gameObject);
        }
    }

    private void SetBodyTypeToDynamic()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collided object has a ProjectileScript component
        ProjectileScript projectile = collision.gameObject.GetComponent<ProjectileScript>();
        if (projectile != null)
        {
           activateNearbyBlockPhysics();
        }
    }

    void activateNearbyBlockPhysics() 
    {
        Vector2 blockPosition = transform.position;

        Block[] allBlocks = FindObjectsOfType<Block>();

        foreach (Block block in allBlocks)
        {
            float distance = Vector2.Distance(block.transform.position, blockPosition);
            bool isOnSameRowOrOneRowDown = Mathf.Abs(block.transform.position.y - blockPosition.y) <= 1;

            if (!block.isBackround && distance <= 5 && isOnSameRowOrOneRowDown)
            {
                block.SetBodyTypeToDynamic();
            }
            // This is used to clear up the backround blocks, so they dont 
            // stick around forever (caves, surfacedecorations etc.)
            else if (block.isBackround && distance <= 6.5)
            {
                StartCoroutine(DestroyBlockAfterDelay(block, 1.5f));
            }
        }    
    }

    private IEnumerator DestroyBlockAfterDelay(Block block, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (block != null)
        {
            Destroy(block.gameObject);    
        }
    }
}
