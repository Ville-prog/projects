using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleSpawner : MonoBehaviour
{
    public GameObject bubblePrefab;
    public float spawnInterval = 5f;
    public float spawnRangeX = 6f;

    private List<string> sameOriginUrls = new List<string> { "https://www.cyberquest.com:443/about", "https://www.cyberquest.com", "https://www.cyberquest.com/contact", "https://www.cyberquest.com/products" };
    private List<string> crossOriginUrls = new List<string> { "http://www.cyberquest.com:443", "https://www.cybercat.com:443", "https://www.cyberquest.com:33", "http://www.verysecure.org", "https://www.frogs.net" };

    private void Start()
    {
        StartCoroutine(SpawnBubbles());
    }

    IEnumerator SpawnBubbles()
    {
        while (true)
        {   
            // Create a new bubble in a random position within the range after specified time interval
            yield return new WaitForSeconds(spawnInterval);
            float spawnX = Random.Range(-spawnRangeX, spawnRangeX);
            Vector3 spawnPosition = new Vector3(spawnX, transform.position.y, 0);
            GameObject newBubble = Instantiate(bubblePrefab, spawnPosition, Quaternion.identity);

            // Choose either correct or incorrect origin from one of the lists
            bool isCorrect = Random.value > 0.5f;
            string selectedUrl = isCorrect ? sameOriginUrls[Random.Range(0, sameOriginUrls.Count)] 
                                           : crossOriginUrls[Random.Range(0, crossOriginUrls.Count)];

            // Set data for the spawned bubble
            Bubble bubbleScript = newBubble.GetComponent<Bubble>();

            if (bubbleScript != null)
            {
                bubbleScript.SetBubbleData(selectedUrl, isCorrect);
            }


        }
    }
}
