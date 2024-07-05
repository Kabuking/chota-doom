using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowProjectile : MonoBehaviour
{
    [Header("Instantiation things")]
    public GameObject projectilePrefab;
    public Transform muzzle;

    [Header("Player transforms")]
    public Transform playerTransform;

    [Header("Bullet stats")]
    public float timeBetweenBulllets;
    public float cooldown;
    public float bulletSpeed;
    public float numberOfBullets;

    [Header("Serialized for debugging")]
    [SerializeField] bool shooting = false;
    void Start()
    {
        StartCoroutine(WeaponSystem());
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(playerTransform);
    }

    IEnumerator WeaponSystem()
    {
        while (true)
        {
            shooting = true;
            for(int i = 0; i < numberOfBullets; i++)
            {
                GameObject projectile = Instantiate(projectilePrefab, muzzle.position, Quaternion.identity);
                Destroy(projectile, 2);
                Vector3 direction = (playerTransform.position - muzzle.position).normalized;
                projectile.GetComponent<Rigidbody>().velocity = direction * bulletSpeed;

                yield return new WaitForSeconds(timeBetweenBulllets);
            }
            shooting = false;
            yield return new WaitForSeconds(cooldown);
        }
    }
}
