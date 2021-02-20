using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Bullet")]
    public ObjectPool bulletPool;
    [Header("Gun Muzzle")]
    public Transform muzzle;

    [Header("Stats")]
    public int curAmmo;
    public int maxAmmo;
    public bool infiniteAmmo;

    public float bulletSpeed;

    public float shootRate;

    private float lastShootTime;
    private bool isPlayer;

    [Header("Audio")]
    public AudioClip shootSfx;
    private AudioSource audioSource;

    private void Awake()
    {
        // Check if attached to the player atm
        if (GetComponent<Player>())
            isPlayer = true;

        audioSource = GetComponent<AudioSource>();
    }

    /**
     * Check if can shoot a bullet or not
     * @return bool
     */
    public bool CanShoot()
    {
        if ((Time.time - lastShootTime) >= shootRate)
        {
            if (curAmmo > 0 || infiniteAmmo)
            {
                return true;
            }
        }

        return false;
    }

    /**
     * Shoot the bullet <Animation>(*) <Damage>(*)
     * @return bool
     */
    public void Shoot()
    {
        // Set last shoting time, and this make it first thing. 0.01ms it can make a lot of diff with rates and don't take it for graunted really, u may encounter bugs sometimes for a stupid 0.01ms
        lastShootTime = Time.time;
        // Reduce the current ammo, so the user has to recharge it later
        curAmmo--;

        if (isPlayer)
        {
            // Update Game UI AMMO text
            GameUI.instance.UpdateAmmoText(curAmmo, maxAmmo);
        }

        audioSource.PlayOneShot(shootSfx);

        // Create a new instance of bullet prefab, we need to create these through the script as we dont need to initalize it within game scene start
        // Why? If you do not use the weapon, or if u got 1000 weps u gonna call them all? Call only what you need and when is used.

        // We set the <bullet model> the current muzzle position where is at and muzzle rotation, as the user can move!
        GameObject bullet = bulletPool.GetObject();

        // Set the properties
        bullet.transform.position = muzzle.position;
        bullet.transform.rotation = muzzle.rotation;

        // Set the speed of the bullet 
        bullet.GetComponent<Rigidbody>().velocity = muzzle.forward * bulletSpeed;
    }

}
