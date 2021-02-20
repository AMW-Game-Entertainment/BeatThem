using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    public int curHp;
    public int maxHp;
    public int scoreReward;

    [Header("Movement")]
    public float moveSpeed;
    public float attackRange;
    public float yPathOffset;

    // Must be initialized when is lists and such, because on next game play will throw reference error
    // This should be taken care of by Unity itself, shows not reliable after all for heavy AAA Games.
    private List<Vector3> path = new List<Vector3>();

    private Weapon weapon;
    private GameObject target;

    private void Start()
    {
        // Get the components needed
        weapon = GetComponent<Weapon>();
        // Since there is single instance of player in the script, we can find the first object in the list. Do not use such in multiplayer lol, don't be nuub.
        target = FindObjectOfType<Player>().gameObject;

        // Performance: We do not use Update() since update() uses frames, so we use InvokeRepeating which it will invoke every 0.5s and it will be much better performance
        InvokeRepeating("UpdatePath", 0.0f, 0.5f);
    }

    private void Update()
    {
        // Get current distance from enemy to target <Player>
        float distance = Vector3.Distance(transform.position, target.transform.position);

        // If distance is lower or equal to the attack range requested. Try Shot
        if (distance <= attackRange)
        {
            if (weapon.CanShoot())
            {
                weapon.Shoot();
            }
        } 
        else
        {
            // Chase the target, as still didnt reach it
            ChaseTarget();
        }

        // Look at the target <Player>
        // Get the normalized direction to what it should look at
        Vector3 direction = (target.transform.position - transform.position).normalized;
        // Get the angle that in degreece format
        float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        // Apply to enemy the angle it should look at
        transform.eulerAngles = Vector3.up * angle;
    }

    /**
     * Chase the target from set of paths to reach the target, for example 
     * Enemy path 1 >  path 2 >  path 3 > target <Player>
     * @return void
     */
    private void ChaseTarget() 
    {
        // Stop when there is no paths, it means u reached your target
        if (path.Count == 0)
            return;

        // Get the target path position
        Vector3 targetPathPosition = path[0] + new Vector3(0, yPathOffset, 0);
        // Move towards the closes path to the Player
        transform.position = Vector3.MoveTowards(transform.position, targetPathPosition, moveSpeed * Time.deltaTime);

        // Check if enemy has reached the path position
        if (transform.position == targetPathPosition)
            // Remove this from the list
            path.RemoveAt(0);
    }

    /**
     * Update the paths list to set how many paths needed to reach the target <Player>
     * This can be calcualted like 1 grid box distance each
     * @return void
     */
    private void UpdatePath()
    {
        // Calculate a path to the specific target <Player>
        NavMeshPath navMeshPath = new NavMeshPath();
        // Calculate the path from the current enemy position to the target position <Player>. 
        // This should check for all areas possible in this case
        // It requires an instance of NavMeshPath()
        NavMesh.CalculatePath(transform.position, target.transform.position, NavMesh.AllAreas, navMeshPath);

        // Convert with linq our corners to list for us to access
        path = navMeshPath.corners.ToList();
    }

    /**
     * Take the damage taken to the enemy from x target
     * @return void
     */
    public void TakeDamage(int damage)
    {
        curHp -= damage;

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
        // Add score to game instance
        GameManager.instance.AddScore(scoreReward);
        // Destroy the object
        Destroy(gameObject);
    }
}
