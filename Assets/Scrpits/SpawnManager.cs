using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    GameManager gameManager;

    public GameObject[] miniEnemyPrefabs;
    public GameObject[] powerupPrefabs;

    public GameObject bossPrefabs;
    public int bossRound;

    float spawnRange = 9;
    int enemyCount;
    int waveNumber = 1;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        int randomPowerup = Random.Range(0, powerupPrefabs.Length);       
    }

    void Update()
    {
        enemyCount = FindObjectsOfType<Enemy>().Length;
        if (enemyCount == 0 && gameManager.isGameActive)
        {
            gameManager.WaveCounter(1f);

            //Spawn a boss every x number of waves
            if (waveNumber%bossRound==0)
                SpawnBossWave(waveNumber);

            else
                SpawnEnemyWave(waveNumber);

            waveNumber++;
            
            //Updated to select a random powerup prefab for the Medium Challenge
            int randomPowerup = Random.Range(0, powerupPrefabs.Length);
            Instantiate(powerupPrefabs[randomPowerup], GenerateSpawnPosition(), powerupPrefabs[randomPowerup].transform.rotation);
        }
    }
    void SpawnEnemyWave(int enemiesToSpawn)
    {
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            int randomEnemy = Random.Range(0, miniEnemyPrefabs.Length);
            Instantiate(miniEnemyPrefabs[randomEnemy], GenerateSpawnPosition(), miniEnemyPrefabs[randomEnemy].transform.rotation);
        }
    }

    private Vector3 GenerateSpawnPosition()
    {
        float spawnPosX = Random.Range(-spawnRange, spawnRange);
        float spawnPosZ = Random.Range(-spawnRange, spawnRange);
        Vector3 randomPos = new Vector3(spawnPosX, 0, spawnPosZ);
        return randomPos;
    }
    void SpawnBossWave(int currentRound)
    {
        int miniEnemysToSpawn;
       
        if (bossRound!=0)
            miniEnemysToSpawn = currentRound / bossRound;

        else
            miniEnemysToSpawn = 1;

        GameObject boss = Instantiate(bossPrefabs, GenerateSpawnPosition(), bossPrefabs.transform.rotation);
        boss.GetComponent<Enemy>().miniEnemySpawnCount = miniEnemysToSpawn;
    }

    public void SpawnMiniEnemy(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            int randomMini = Random.Range(0, miniEnemyPrefabs.Length);
            Instantiate(miniEnemyPrefabs[randomMini], GenerateSpawnPosition(), miniEnemyPrefabs[randomMini].transform.rotation);
        }
    }
}
