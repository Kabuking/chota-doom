using System.Collections;
using UnityEngine;

public class BulletBase : MonoBehaviour
{
    [Header("Bullet stats")]
    public float damage;

    [Header("Bullet type")]
    public DamageType damageType = DamageType.Normal;

    public enum DamageType { 
        Normal,
        Bleed,
        Shock
    }
    
    private Rigidbody rb;
    private void Awake()
    {
        rb = transform.GetComponent<Rigidbody>();
    }

    public void SetBulletVelocity(float bulletSpeed, Transform bulletSpawnPoint, float selfDestroyAfter = 2f)
    {
        transform.position = bulletSpawnPoint.position;
        transform.rotation = bulletSpawnPoint.rotation;
        rb.velocity = bulletSpawnPoint.forward * bulletSpeed;
        
        Destroy(gameObject, selfDestroyAfter);
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }

}
