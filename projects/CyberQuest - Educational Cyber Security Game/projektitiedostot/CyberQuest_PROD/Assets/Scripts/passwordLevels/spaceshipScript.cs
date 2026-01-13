using UnityEngine;
using System.Collections;

public class spaceshipScript : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float tiltAmount = 14f;
    [SerializeField] private float tiltSmoothTime = 0.2f;
    [SerializeField] private Camera mainCamera;

    [SerializeField] private AudioSource audioSource;

    private Coroutine fadeOutCoroutine;
    private float currentTilt = 0f;
    private float tiltVelocity = 0f;
    private float minX, maxX;

    // Audio behavior settings
    private float baseVolume = 0.018f;
    private float basePitch = -0.59f;
    private float targetVolume = 0f;
    private float targetPitch = -0.59f;
    private float volumeLerpSpeed = 2f;
    private float pitchLerpSpeed = 2f;

    private float pitchNoiseSpeed = 1.5f;
    private float pitchNoiseAmount = 0.01f;  // Tiny subtle shift
    private float pitchNoiseTime = 0f;

    private Coroutine recoilCoroutine; // To track the recoil coroutine

    void Start()
    {
        Vector3 screenMin = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 screenMax = mainCamera.ViewportToWorldPoint(new Vector3(1, 0, 0));
        minX = screenMin.x + 1.5f;
        maxX = screenMax.x - 1.5f;

        // Initial audio state
        audioSource.volume = 0f;
        audioSource.pitch = basePitch;
        targetPitch = basePitch;
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        Move(horizontalInput);
        Tilt(horizontalInput);

        // While moving, fluctuate pitch subtly
        if (Mathf.Abs(horizontalInput) > 0.01f)
        {
            pitchNoiseTime += Time.deltaTime * pitchNoiseSpeed;
            float noise = Mathf.PerlinNoise(pitchNoiseTime, 0f);
            float fluctuation = Mathf.Lerp(-pitchNoiseAmount, pitchNoiseAmount, noise);
            targetPitch = basePitch + fluctuation;
        }
        else
        {
            pitchNoiseTime = 0f;
            targetPitch = basePitch;
        }
    }

    private void Move(float horizontalInput)
    {
        Vector3 newPosition = transform.position + Vector3.right * horizontalInput * speed * Time.deltaTime;
        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
        transform.position = newPosition;

        if (Mathf.Abs(horizontalInput) > 0.01f)
        {
            if (!audioSource.isPlaying)
                audioSource.Play();

            // Volume scales slightly with input but capped at baseVolume
            targetVolume = Mathf.Lerp(0.1f * baseVolume, baseVolume, Mathf.Abs(horizontalInput));

            if (fadeOutCoroutine != null)
            {
                StopCoroutine(fadeOutCoroutine);
                fadeOutCoroutine = null;
            }
        }
        else
        {
            if (audioSource.isPlaying && fadeOutCoroutine == null)
            {
                fadeOutCoroutine = StartCoroutine(FadeOutSound());
            }
        }

        // Smooth audio transitions
        audioSource.volume = Mathf.Lerp(audioSource.volume, targetVolume, Time.deltaTime * volumeLerpSpeed);
        audioSource.pitch = Mathf.Lerp(audioSource.pitch, targetPitch, Time.deltaTime * pitchLerpSpeed);
    }

    private IEnumerator FadeOutSound()
    {
        float fadeDuration = 0.5f;
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }

        audioSource.Stop();
        fadeOutCoroutine = null;
    }

    private void Tilt(float horizontalInput)
    {
        float targetTilt = -horizontalInput * tiltAmount;
        currentTilt = Mathf.SmoothDamp(currentTilt, targetTilt, ref tiltVelocity, tiltSmoothTime);
        transform.rotation = Quaternion.Euler(0, 0, currentTilt);
    }

    // Recoil function
    public void TriggerRecoil(float recoilDistance, float recoilDuration)
    {
        if (recoilCoroutine != null)
        {
            StopCoroutine(recoilCoroutine);
        }
        recoilCoroutine = StartCoroutine(Recoil(recoilDistance, recoilDuration));
    }

    private IEnumerator Recoil(float recoilDistance, float recoilDuration)
    {
        Vector3 originalPosition = transform.position;
        Vector3 recoilPosition = originalPosition + new Vector3(0, -recoilDistance, 0);

        Quaternion originalRotation = transform.rotation;
        Quaternion zeroRotation = Quaternion.Euler(0, 0, 0);

        // Move down to recoil position and set rotation to z = 0
        float elapsedTime = 0f;
        while (elapsedTime < recoilDuration / 2f)
        {
            transform.position = Vector3.Lerp(originalPosition, recoilPosition, elapsedTime / (recoilDuration / 2f));
            transform.rotation = Quaternion.Lerp(originalRotation, zeroRotation, elapsedTime / (recoilDuration / 2f));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure final position and rotation at halfway point
        transform.position = recoilPosition;
        transform.rotation = zeroRotation;

        // Move back to original position and restore rotation
        elapsedTime = 0f;
        while (elapsedTime < recoilDuration / 2f)
        {
            transform.position = Vector3.Lerp(recoilPosition, originalPosition, elapsedTime / (recoilDuration / 2f));
            transform.rotation = Quaternion.Lerp(zeroRotation, originalRotation, elapsedTime / (recoilDuration / 2f));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure final position and rotation at the end
        transform.position = originalPosition;
        transform.rotation = originalRotation;

        recoilCoroutine = null;
    }
}