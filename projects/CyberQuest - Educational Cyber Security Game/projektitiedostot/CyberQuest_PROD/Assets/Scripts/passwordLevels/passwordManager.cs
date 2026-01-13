using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class passwordManager : MonoBehaviour
{
    [SerializeField] private GameObject spaceBug; 
    [SerializeField] private float spawnInterval = 2f; 
    [SerializeField] private float bugSpeed = 1f; 

    private bool startSpawning = false;

    // Dictionary to hold passwords categorized by their tier
    private Dictionary<int, List<string>> passwordsByTier;

    void Start()
    {
        // Initialize passwords
        passwordsByTier = new Dictionary<int, List<string>>()
        {
            { 0, new List<string>() }, // VeryBad
            { 1, new List<string>() }, // Bad
            { 2, new List<string>() }, // Good
            { 3, new List<string>() }  // VeryGood
        };

        initializePasswords();

        startSpawning = true;
        StartCoroutine(SpawnSpaceBugs());
    }

    private IEnumerator SpawnSpaceBugs()
    {
        if (spaceBug == null)
            yield break;

        List<Vector3> recentSpawnPositions = new List<Vector3>();
        float minDistance = 2f; 
        int maxRecentPositions = 3; 

        while (startSpawning)
        {
            Vector3 spawnPosition = Vector3.zero;
            bool validPosition = false;

            const int maxAttempts = 50;
            int attempt = 0;

            while (!validPosition && attempt < maxAttempts)
            {
                attempt++;
                validPosition = true;

                float randomX = Random.Range(-11f, 11f);
                spawnPosition = new Vector3(randomX, 8f, 0f);

                foreach (Vector3 recentPosition in recentSpawnPositions)
                {
                    if (Vector3.Distance(spawnPosition, recentPosition) < minDistance)
                    {
                        validPosition = false;
                        break;
                    }
                }
            }

            if (!validPosition)
            {
                yield return new WaitForSeconds(spawnInterval);
                continue;
            }

            recentSpawnPositions.Add(spawnPosition);
            if (recentSpawnPositions.Count > maxRecentPositions)
                recentSpawnPositions.RemoveAt(0);

            GameObject bug = Instantiate(spaceBug, spawnPosition, Quaternion.identity);
            if (bug == null)
            {
                yield return new WaitForSeconds(spawnInterval);
                continue;
            }

            Rigidbody2D rb = bug.GetComponent<Rigidbody2D>();
            if (rb != null)
                rb.linearVelocity = Vector2.down * bugSpeed;

            passwordScript script = bug.GetComponent<passwordScript>();
            if (script != null)
            {
                int randomTier = Random.Range(0, 4);
                script.Initialize(passwordsByTier, randomTier);
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }


    private void initializePasswords() 
    {
        // Very bad passwords
        passwordsByTier[0].Add("123456");
        passwordsByTier[0].Add("password");
        passwordsByTier[0].Add("qwerty");
        passwordsByTier[0].Add("abc123");
        passwordsByTier[0].Add("letmein");
        passwordsByTier[0].Add("111111");
        passwordsByTier[0].Add("monkey");
        passwordsByTier[0].Add("admin");
        passwordsByTier[0].Add("welcome");
        passwordsByTier[0].Add("iloveyou");
        passwordsByTier[0].Add("dragon");
        passwordsByTier[0].Add("000000");
        passwordsByTier[0].Add("123");
        passwordsByTier[0].Add("villel");
        passwordsByTier[0].Add("starwars");
        passwordsByTier[0].Add("4321");
        passwordsByTier[0].Add("beer1");
        passwordsByTier[0].Add("jamppa");
        passwordsByTier[0].Add("catlover");
        passwordsByTier[0].Add("master");

        // Bad passwords
        passwordsByTier[1].Add("hello2024");
        passwordsByTier[1].Add("summer123");
        passwordsByTier[1].Add("password1");
        passwordsByTier[1].Add("justme");
        passwordsByTier[1].Add("michael99");
        passwordsByTier[1].Add("testtest");
        passwordsByTier[1].Add("gamer123");
        passwordsByTier[1].Add("chocolate");
        passwordsByTier[1].Add("ilovemusic");
        passwordsByTier[1].Add("letmein123");
        passwordsByTier[1].Add("abcabc123");
        passwordsByTier[1].Add("spider77");
        passwordsByTier[1].Add("dolphin22");
        passwordsByTier[1].Add("batman99");
        passwordsByTier[1].Add("baseball1");
        passwordsByTier[1].Add("guitarlove");
        passwordsByTier[1].Add("coffee11");
        passwordsByTier[1].Add("newyork12");
        passwordsByTier[1].Add("cheese55");
        passwordsByTier[1].Add("unicorn9");

        // Good passwords
        passwordsByTier[2].Add("P@ssw0rd2023");
        passwordsByTier[2].Add("PurpleElephant#88");
        passwordsByTier[2].Add("G00dLuck4U");
        passwordsByTier[2].Add("Soccer*Ball42");
        passwordsByTier[2].Add("Happy_Dog_7");
        passwordsByTier[2].Add("SecureItNow12!");
        passwordsByTier[2].Add("MarvelFan_19");
        passwordsByTier[2].Add("Ocean$Wave90");
        passwordsByTier[2].Add("B3tterD@yz");
        passwordsByTier[2].Add("FunnyBunny44!");
        passwordsByTier[2].Add("Rainy.Day.2020");
        passwordsByTier[2].Add("Keyboard_Warrior7");
        passwordsByTier[2].Add("8BitGaming#2022");
        passwordsByTier[2].Add("AlWayS2LearN!2");
        passwordsByTier[2].Add("Pizza!Lover34");
        passwordsByTier[2].Add("Dino_Park_44");
        passwordsByTier[2].Add("ThinkTw!ce88");
        passwordsByTier[2].Add("CyberKnight_9");
        passwordsByTier[2].Add("GoF!sh#555");
        passwordsByTier[2].Add("Happy_Monkey99");

        // Very good passwords
        passwordsByTier[3].Add("tK7$uPl4!r9XmZ2");
        passwordsByTier[3].Add("G#o82Lx*pRw1mQe9");
        passwordsByTier[3].Add("P@55w0rd_G3n!0uS2025");
        passwordsByTier[3].Add("H3ll0_Th3r3#X9zW7!");
        passwordsByTier[3].Add("R@nD0mM!x_4482#k!");
        passwordsByTier[3].Add("Cyber$ecurityR0x!993");
        passwordsByTier[3].Add("V1rus_3xpl0d3s@N1t3");
        passwordsByTier[3].Add("Trust_N0_0n3!x92F");
        passwordsByTier[3].Add("gH!8Trx$#o9vLxP2");
        passwordsByTier[3].Add("D0g&K1tt3nz#2025");
        passwordsByTier[3].Add("QwErTy!P@55M0n$ter");
        passwordsByTier[3].Add("!UnBr3@k@bl3_P@$$_23");
        passwordsByTier[3].Add("S0ph0s_4Eva!_Zx99");
        passwordsByTier[3].Add("99Lu$$yG0_!cHy_Tiger");
        passwordsByTier[3].Add("M@tr1x#Reb00t_xY7");
        passwordsByTier[3].Add("Cr@zy#Tr@in_88zzL");
        passwordsByTier[3].Add("Sh3rlock!C0d3_Bak3r");
        passwordsByTier[3].Add("2M@ny$C0ps_1B@dw0lf");
        passwordsByTier[3].Add("Ru1nTh3M@chine!_v2");
        passwordsByTier[3].Add("Xx$CyberWarden88!xX");
    }
}