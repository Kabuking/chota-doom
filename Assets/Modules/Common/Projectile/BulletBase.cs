using System;
using System.Collections;
using System.Collections.Generic;
using Characters.Player.Global;
using Modules.Common;
using UnityEngine;

public class BulletBase : MonoBehaviour
{
    [Header("Bullet stats")]
    public float damage;

    [Header("Bullet type")]
    public DamageType damageType = DamageType.Normal;

    public Rigidbody rbProjectile;

    public GameObject impactPrefab;

    
    public enum DamageType { 
        Normal,
        Bleed,
        Shock
    }

    private float selfDestroyAfterSeconds = 2f;
    public float raycastBuffer = 1f; // Add a small buffer to the raycast
    private bool bulletActive = false;
    
    public void SetVelocity(float bulletSpeed, Transform bulletSpawnPoint, float selfDestroyAfter = 2f)
    {
        transform.position = bulletSpawnPoint.position;
        transform.rotation = bulletSpawnPoint.rotation;
        rbProjectile.velocity = bulletSpawnPoint.forward * bulletSpeed;

        // speed = bulletSpeed;

        // lastPosition = bulletSpawnPoint.position;

        bulletActive = true;
            
        Destroy(gameObject, selfDestroyAfterSeconds);
    }
    

    /*public float speed = 5;
    private Vector3 lastPosition;
    
    void FixedUpdate() // Use FixedUpdate for physics calculations
    {
        if(!bulletActive)
            return;
        
        
        Vector3 currentPosition = transform.position;
        Vector3 movement = currentPosition - lastPosition;
        float distance = movement.magnitude;
        // Use a slightly longer raycast to ensure we don't miss collisions
        RaycastHit hit;
        if (Physics.Raycast(lastPosition, movement.normalized, out hit, distance + raycastBuffer))
        {
            // Collision detected
            HandleCollision(hit);
        }
        else
        {
            // Move the bullet
            transform.Translate(Vector3.forward * speed * Time.fixedDeltaTime);
        }

        lastPosition = currentPosition;
    }
    
    void HandleCollision(RaycastHit hit)
    {
        
        if (hit.collider.gameObject.CompareTag(TagNames.Enemy) ||
            hit.collider.gameObject.CompareTag(TagNames.Player))
        {
            // DebugX.LogWithColorYellow("On trigger bullet");
            rbProjectile.velocity = Vector3.zero;
            Instantiate(impactPrefab, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }*/

    private void OnTriggerEnter(Collider other)
    {
        //TODO:
        /*if(!bulletActive)
            return;*/
        
        if (other.gameObject.CompareTag(TagNames.Enemy) ||
            other.gameObject.CompareTag(TagNames.Player))
        {
            //TODO
            //If Player is ducking do not impact, go through
            
            
            // DebugX.LogWithColorYellow("On trigger bullet");
            rbProjectile.velocity = Vector3.zero;
            if (impactPrefab != null)
            {
                Instantiate(impactPrefab, transform.position, transform.rotation);
            }

            Destroy(gameObject);
        }
    }
}
