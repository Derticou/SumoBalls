using UnityEngine;

public class Enemy : MonoBehaviour
{
    GameManager gameManager;
    SpawnManager spawnManager;

    GameObject player;

    Rigidbody enemyRb;
    public float speed;
    
    public bool isBoss = false;
    public float spawnInterval;
    public int miniEnemySpawnCount;
    float nextSpawn;

    
    // Start is called before the first frame update
    void Start()
    {
        gameManager=GameObject.Find("GameManager").GetComponent<GameManager>();
        enemyRb=GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
        if (isBoss)
        {
            spawnManager = FindObjectOfType<SpawnManager>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.isGameActive)
        {
            Vector3 lookDirection = (player.transform.position - transform.position).normalized;
            enemyRb.AddForce(lookDirection * speed * Time.deltaTime * 100);
        }
        
        if (isBoss)
        {
            if (Time.time>nextSpawn)
            {
                nextSpawn = Time.time + spawnInterval;
                spawnManager.SpawnMiniEnemy(miniEnemySpawnCount);
            }
        }
        
        if (transform.position.y < -5)
        {
            Destroy(gameObject);
            gameManager.FallenBallCounter(1f);
        }
    }
}
