using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public Rigidbody playerRb;
    public CustomGravity gravity;
    [SerializeField] private float knockback = 10000;
    private float knockbackTime = 100;
    private float knockbackCounter = 0;
    private float hitWallTime = 100;
    private float hitWallCounter = 0;
    private bool inCooldown = false;
    private bool hitWall = false;
    private bool attacked = false;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Enemy")
        {
            attacked = true;
            gravity.enabled = false;
            Vector3 direction = (playerRb.transform.position - collision.collider.transform.position).normalized;
            if (Vector3.Angle(direction, new Vector3(direction.x, 0, direction.z).normalized) > 45)
            {
                direction -= Vector3.up / direction.magnitude;
            }
            // playerRb.AddExplosionForce(knockback, collision.collider.transform.position, 1000, 0, ForceMode.Acceleration);
            playerRb.AddForce(knockback * direction.normalized, ForceMode.Acceleration);

            if (!inCooldown && !AudioManager.Instance.IsPlaying("EnemyCollideSfx"))
                AudioManager.Instance.PlayClip("EnemyCollideSfx");
        }
        if (collision.collider.tag == "Wall")
        {
            hitWall = true;
            playerRb.AddForce(100 * -playerRb.transform.forward, ForceMode.Acceleration);
            if (!inCooldown && !AudioManager.Instance.IsPlaying("Boing"))
                AudioManager.Instance.PlayClip("Boing");

        }
    }

    void FixedUpdate()
    {
        if (!gravity.enabled)
        {
            if (knockbackCounter > knockbackTime)
            {
                knockbackCounter = 0;
                gravity.enabled = true;
                attacked = false;
            }
            else
            {
                knockbackCounter++;
            }
        }
        if (hitWall)
        {
            if (hitWallCounter > hitWallTime)
            {
                hitWallCounter = 0;
                hitWall = false;
            }
            else
            {
                hitWallCounter++;
            }
        }
    }

    private IEnumerator Cooldown()
    {
        inCooldown = true;
        yield return new WaitForSeconds(5);
        inCooldown = false;
    }
    public bool GetAttacked()
    {
        return attacked;
    }

    public bool GetHitWall()
    {
        return hitWall;
    }
}
