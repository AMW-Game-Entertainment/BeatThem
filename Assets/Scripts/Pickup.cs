using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PICKUP_TYPE {
    Health, 
    Ammo
}

public class Pickup : MonoBehaviour
{
    [Header("Buff")]
    public PICKUP_TYPE type;
    public int value;

    [Header("Animation")]
    public float rotateSpeed;
    public float bobSpeed;
    public float bobHeight;

    private Vector3 startPos;
    private bool bobbingUp;

    [Header("Audio")]
    public AudioClip pickupSfx;

    private void Start()
    {
        // Set the start position at
        startPos = transform.position;
    }

    private void Update()
    {
        TriggerAnimationFrame();
    }

    private void OnTriggerEnter(Collider colider)
    {
        if (colider.CompareTag("Player"))
        {
            Player player = colider.GetComponent<Player>();

            // According type set the effect
            switch(type)
            {
                case PICKUP_TYPE.Health:
                    player.GiveHealth(value);
                    break;

                case PICKUP_TYPE.Ammo:
                    player.GiveAmmo(value);
                    break;
            }

            // Play the sound when picking up
            colider.GetComponent<AudioSource>().PlayOneShot(pickupSfx);

            Destroy(gameObject);
        }
    }

    /**
     * Trigger the animation per frame, to make a nice animation for pick up. This will make it go up and down and rotate a bit
     * @return void
     */
    private void TriggerAnimationFrame()
    {
        // Rotate according the speed
        transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);

        // Bobbig up and down
        Vector3 targetPositionOfffset = (bobbingUp ? new Vector3(0, bobHeight / 2, 0) : new Vector3(0, -bobHeight / 2, 0));
        // Make the pickup move towards to target position (In this case it has to go up or down as animation
        transform.position = Vector3.MoveTowards(transform.position, startPos + targetPositionOfffset, bobSpeed * Time.deltaTime);

        if (transform.position == (startPos + targetPositionOfffset))
            bobbingUp = !bobbingUp;
    }
}
