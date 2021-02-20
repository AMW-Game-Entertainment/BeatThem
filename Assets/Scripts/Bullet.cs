using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;          // damage dealt to the target
    public float lifetime;      // How long until the bullet despawns
    private float shootTime;    // Time the bullet was shot

    public GameObject rockHitParticle;
    public GameObject humanHitParticle;
    public GameObject woodHitParticle;

    private void OnEnable()
    {
        shootTime = Time.time;
    }
    
    private void Update()
    {
        if ((Time.time - shootTime) >= lifetime)
            gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider collider)
    {
        bool isFleshType = collider.CompareTag("Player") || collider.CompareTag("Enemy");
        bool isRockType = collider.CompareTag("Rock") || collider.CompareTag("Ground");

        if (isRockType)
        {
            // Create the hit particle
            GameObject obj = Instantiate(rockHitParticle, transform.position, Quaternion.identity);
            // Destroy it after half a second (How long is the particle to be done)
            Destroy(obj, 0.5f);
        }
        else if (collider.CompareTag("Tree"))
        {
            // Create the hit particle
            GameObject obj = Instantiate(woodHitParticle, transform.position, Quaternion.identity);
            // Destroy it after half a second (How long is the particle to be done)
            Destroy(obj, 0.5f);
        }
        else if (isFleshType)
        {
            // Did we hit the player?
            if (collider.CompareTag("Player"))
                collider.GetComponent<Player>().TakeDamage(damage);
            // Did we hit the Enemy?
            else if (collider.CompareTag("Enemy"))
                collider.GetComponent<Enemy>().TakeDamage(damage);

            // Create the hit particle
            GameObject obj = Instantiate(humanHitParticle, transform.position, Quaternion.identity);
            // Destroy it after half a second (How long is the particle to be done)
            Destroy(obj, 0.5f);
        }

        // Check if life time has been surpassed, which majority is passed
        // Else we take care of it on next frame Update
        if ((Time.time - shootTime) >= lifetime)
        {
            gameObject.SetActive(false);
        }
    }
}
