using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    GameManager gameManager;

    public PowerUpType currentPowerUp= PowerUpType.None;

    public GameObject rocketPrefab;
    GameObject tmpRocket;

    Coroutine powerupCountdown;

    Rigidbody playerRb;
    float speed = 3.0f;

    GameObject focolPoint;
    public GameObject powerupIndicator;
    
    float powerUpStrength = 15.0f;
    public bool hasPowerup = false;

    bool smashing = false;
    float floorY;
    float hangTime = 0.4f;
    float smashSpeed = 15f;
    float explosionForce = 50.0f;
    float explosionRadius = 10f;



    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        playerRb = GetComponent<Rigidbody>();

        focolPoint = GameObject.Find("Focol Point");
    }

    void Update()
    {
        if (gameManager.isGameActive)
        { 
            float VerticalInput = Input.GetAxis("Vertical");

            playerRb.AddForce(focolPoint.transform.forward * Time.deltaTime * 200 * speed * VerticalInput);
            powerupIndicator.transform.position = transform.position + new Vector3(0, -0.5f, 0);

            if (currentPowerUp==PowerUpType.Rockets && Input.GetKeyDown(KeyCode.Space))
            {
                LaunchRockets();
            }

            if (currentPowerUp==PowerUpType.Smash && Input.GetKeyDown(KeyCode.Space)&& !smashing)
            {
                smashing = true;
                StartCoroutine(Smash());
            }
        }

        if (transform.position.y < -5)
        {
            Destroy(gameObject);
            gameManager.GameOver(true);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("powerup"))
        {
            hasPowerup = true;

            currentPowerUp = other.gameObject.GetComponent<PowerUp>().powerUpType;

            if (currentPowerUp==PowerUpType.Pushback)
                gameManager.powerupInfo.text = "Increases knockback";

            else if(currentPowerUp==PowerUpType.Smash)
                gameManager.powerupInfo.text = "Press 'Space' to smash";

            else if(currentPowerUp==PowerUpType.Rockets)
                gameManager.powerupInfo.text = "Press 'Space' to launch rocket";

            powerupIndicator.SetActive(true);
            Destroy(other.gameObject);
            
            if (powerupCountdown!=null)
            {
                StopCoroutine(powerupCountdown);
            }
            powerupCountdown = StartCoroutine(PowerupCountdownRoutine());
        }

    }
    IEnumerator PowerupCountdownRoutine () 
    {
        yield return new WaitForSeconds(7);
        hasPowerup = false;
        currentPowerUp = PowerUpType.None;
        powerupIndicator.SetActive(false);
        gameManager.powerupInfo.text = "";
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Enemy") && currentPowerUp==PowerUpType.Pushback) 
        {
            Rigidbody enemyRigidbody=collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = collision.gameObject.transform.position - transform.position;

            enemyRigidbody.AddForce(awayFromPlayer * powerUpStrength, ForceMode.Impulse);
            Debug.Log("Collided with " + collision.gameObject.name + " With powerup set to " + currentPowerUp.ToString());
        }
    }
    void LaunchRockets()
    {
        foreach (var enemy in FindObjectsOfType<Enemy>())
        {
            tmpRocket = Instantiate(rocketPrefab, transform.position + Vector3.up, Quaternion.identity);
            tmpRocket.GetComponent<HomingRocket>().Fire(enemy.transform);
        }
    }

    IEnumerator Smash()
    { 
        var enemies = FindObjectsOfType<Enemy>();

        //Store the y position before taking off
        floorY = transform.position.y;

        //Calculate the amount of time we will go up
        float jumpTime = Time.time + hangTime;

        while (Time.time<jumpTime)
        {
            //Move the player up
            playerRb.velocity = new Vector2(playerRb.velocity.x, smashSpeed);
            yield return null;
        }

        //Move the player down
        while (transform.position.y>floorY)
        {
            playerRb.velocity = new Vector2(playerRb.velocity.x, -smashSpeed * 2);
            yield return null;
        }

        //Cycle through all enemies
        for (int i = 0; i < enemies.Length; i++)
        {
            //explosion force
            if (enemies[i]!=null)
            {
                enemies[i].GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, explosionRadius, 0.0f, ForceMode.Impulse);
            }
            smashing = false;
        }
    }

}

