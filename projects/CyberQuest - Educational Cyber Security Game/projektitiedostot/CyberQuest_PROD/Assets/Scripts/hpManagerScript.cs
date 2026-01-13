using UnityEngine;
using UnityEngine.SceneManagement;

public class hpManagerScript : MonoBehaviour
{
    [SerializeField] public int hp = 3;        // Current health
    private int maxHp = 3;    // Maximum health

    private SpriteRenderer jamppaRenderer;
    private SpriteMask spriteMask;
    [SerializeField] private Transform redSquare;
    private GameOver gameOver;

    public static hpManagerScript instance;

    void Awake()
    {
        // Ensure only one instance of this script exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Persist this GameObject across scenes

            // Subscribe to the sceneLoaded event
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }

    void Start()
    {
        // Initialize components for the first scene
        InitializeSceneComponents();
        UpdateMask(); // Update the mask based on current health
    }

    void Update() 
    {
        if (hp <= 0) 
        {
            Debug.Log("RESTARTING!");
            gameOver = FindFirstObjectByType<GameOver>();
            hp = 3;
            gameOver.EndMenu();
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Reinitialize components for the new scene
        InitializeSceneComponents();
        UpdateMask(); // Update the mask based on current health
    }

    private void InitializeSceneComponents()
    {
        // Reinitialize scene-specific components
        jamppaRenderer = GameObject.Find("Jamppa")?.GetComponent<SpriteRenderer>();
        spriteMask = GameObject.Find("Mask")?.GetComponent<SpriteMask>();
        redSquare = GameObject.Find("redMask")?.transform;

        if (redSquare != null)
        {
            // Set alpha to 50% transparency
            SpriteRenderer redSquareRenderer = redSquare.GetComponent<SpriteRenderer>();
            if (redSquareRenderer != null)
            {
                Color color = redSquareRenderer.color;
                color.a = 0.5f; // Set alpha to 50% transparency
                redSquareRenderer.color = color;
            }
        }
    }

    public void reduceHp()
    {
        if (hp > 0)
        {
            hp -= 1; // Decrease health
            Debug.Log("HP is: " + hp);
            AudioSource audioSource = GetComponent<AudioSource>();
            audioSource.Play();
            UpdateMask(); // Update the mask based on the new health
        }

    }

    public void UpdateMask()
    {
        if (jamppaRenderer == null || redSquare == null) return;

        // Calculate the fill amount based on health (from 0 to 1)
        float fillAmount = (float)(maxHp - hp) / maxHp;

        // Set the base Y position to -120 (or any value you prefer)
        float baseYPosition = -215f;

        // Calculate the offset based on the fill amount, but amplify the change using a factor
        float offset = jamppaRenderer.bounds.size.y * (fillAmount / 2);

        // Apply a scaling factor to make the increments bigger
        float scalingFactor = 2.0f;  // Increase this value for more noticeable increments
        offset *= scalingFactor;

        // Adjust the position of the red square
        redSquare.localPosition = new Vector3(
            redSquare.localPosition.x,
            baseYPosition + offset,
            redSquare.localPosition.z
        );
    }

    private void OnDestroy()
    {
        // Unsubscribe from the sceneLoaded event to avoid memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
