using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Stats")]
    public int curHp;
    public int maxHp;

    [Header("Movement")]
    public float moveSpeed;             // movement speed in units per second
    public float jumpForce;             // force applied upwards

    [Header("Camera")]
    public float lookSensitivity;       // mouse look sensitvity
    public float maxLookX;              // lowest down we can look
    public float minLookX;              // highest up we can look
    private float rotX;                 // current x rotation of the camera

    [Header("Elements")]
    public int fireDamage;
    public float fireDamageRate;
    private float lastFireDamageTime;

    private Camera cam;                 // Main camera GameObject instance
    private Rigidbody rig;              // Rigibody GameObject instance
    private Weapon weapon;

    private void Awake()
    {
        // Get all the components of current GameObject attached into
        cam = Camera.main;
        rig = GetComponent<Rigidbody>();
        weapon = GetComponent<Weapon>();

        // Disable cursor
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (GameManager.instance.isPausedGame)
            return;

        Move();

        if (Input.GetButtonDown("Jump"))
            TryJump();

        if (Input.GetButtonDown("Fire1"))
            if (weapon.CanShoot())
                weapon.Shoot();

        CameraLook();
    }

    private void Start()
    {
        GameUI.instance.UpdateHealthBar(curHp, maxHp);
        GameUI.instance.UpdateScoreText(0);
        GameUI.instance.UpdateAmmoText(weapon.curAmmo, weapon.maxAmmo);
    }

    /**
     * When you are staying on specific triggering it will call this
     * @return void
     */
    private void OnTriggerStay(Collider colider)
    {
        // Setting up last time fire damage time
        lastFireDamageTime += Time.deltaTime;
        // If this is fire tag, and met the rate we can damage the user
        if (colider.CompareTag("Fire") && lastFireDamageTime > fireDamageRate)
        {
            lastFireDamageTime -= fireDamageRate;
            TakeDamage(fireDamage);
        }
    }

    private void Move()
    {
        // Get key pressed and multiply by how much speed we want it to be fast
        float inputX = Input.GetAxis("Horizontal") * moveSpeed;
        float inputZ = Input.GetAxis("Vertical") * moveSpeed;

        // Get the direction of from player right coords multiply by keypressed x Axis and player front coords 
        Vector3 direction = (transform.right * inputX) + (transform.forward * inputZ);
        // Set the direction to current velecotiy.Y
        direction.y = rig.velocity.y;

        // Apply the key pressed if X, Z. Keep Y to what is concurrently
        rig.velocity = direction;
    }

    void CameraLook()
    {
        // Get current mouse rotation from Y and multiple how much sensitivity should it be
        float mouseY = Input.GetAxis("Mouse X") * lookSensitivity;
        // Get current mouse rotation from X and multiple how much sensitivity should it be
        rotX += Input.GetAxis("Mouse Y") * lookSensitivity;

        // It gives in between the rotX value in between of minLookX and maxLookX, so it doesnt exceed the limit and keeps between both ranges
        rotX = Mathf.Clamp(rotX, minLookX, maxLookX);

        // Set the location rotation of the camera. We need Euler to set the rotation with x, y,z
        cam.transform.localRotation = Quaternion.Euler(-rotX, 0, 0);
        // Assign to current GameObject as player, the angle up times to mouse Y coords
        transform.eulerAngles += Vector3.up * mouseY;
    }

    void TryJump()
    {
        // Create Ray to determine the distance between player position to downwards
        Ray ray1 = new Ray(transform.position, Vector3.down);

        // Check if is on the ground. 1.1f is the default between player and ground difference. This to prevent multiple jumps like PWI
        if (Physics.Raycast(ray1, 1.1f))
            // Add a push force to the ground that will make up jump upwards. We need this impulsive way, not like a spaceship which moves slowly, should be instant as possible.
            rig.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    public void TakeDamage(int damage)
    {
        curHp -= damage;

        // Update Game UI HP text
        GameUI.instance.UpdateHealthBar(curHp, maxHp);

        if (curHp <= 0)
        {
            Die();
        }
    }

    /**
     * Kill the enemy
     * @return void
     */
    public void Die()
    {
        GameManager.instance.LoseGame();
    }

    public void GiveHealth(int amount)
    {
        // Using Clamp so you make sure you do not acceed the x amount of max hp.
        curHp = Mathf.Clamp(curHp + amount, 0, maxHp);

        // Update Game UI HP text
        GameUI.instance.UpdateHealthBar(curHp, maxHp);
    }

    public void GiveAmmo(int amount)
    {
        // Using Clamp so you make sure you do not acceed the x amount of max ammo.
        weapon.curAmmo = Mathf.Clamp(weapon.curAmmo + amount, 0, weapon.maxAmmo);

        // Update Game UI AMMO text
        GameUI.instance.UpdateAmmoText(weapon.curAmmo, weapon.maxAmmo);
    }
}
