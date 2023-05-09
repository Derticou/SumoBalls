using UnityEngine;

public class HomingRocket : MonoBehaviour
{
    Transform target;

    float speedRotation = 10f;
    float speed = 15.0f;
    float rocketStrength = 15.0f;

    float aliveTimer = 5.0f;

    public void Fire(Transform newTarget)
    {
        target = newTarget;
        
        Vector3 moveDirection = (target.transform.position - transform.position).normalized;
        transform.position += moveDirection * speed * Time.deltaTime;
        Destroy(gameObject, aliveTimer);
    }   

    void Update()
    {
        if (!target)
        {
            Destroy(gameObject);
            return;
        }
        Vector3 lookDirection = target.transform.position - transform.position;

        Quaternion rotateTarget = Quaternion.LookRotation(lookDirection, Vector3.up) * Quaternion.Euler(90.0f, 0, 0);
        Quaternion rotate = Quaternion.RotateTowards(transform.rotation, rotateTarget, speedRotation);

        transform.rotation = rotate;
        transform.Translate(Vector3.up * Time.deltaTime * speed);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);
            Rigidbody enemyRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = collision.gameObject.transform.position - transform.position;

            enemyRigidbody.AddForce(awayFromPlayer * rocketStrength, ForceMode.Impulse);
        }

        else if (!collision.gameObject.CompareTag("Bullet"))
        {
            Destroy(gameObject);
        }
    }
}
