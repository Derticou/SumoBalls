using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    GameManager gameManager;
    private float rotationSpeed = 100.0f;

    void Start()
    {
        gameManager=GameObject.Find("GameManager").GetComponent<GameManager>();
    }


    void Update()
    {
        if (gameManager.isGameActive)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            transform.Rotate(Vector3.up, horizontalInput * rotationSpeed * Time.deltaTime);
        }
    }
}
