using System.Collections;
using System.Collections.Generic;
using Modules.CommonEventBus;
using UnityEngine;

public class SlowProjectile : MonoBehaviour
{
    [Header("Instantiation things")]
    public GameObject projectilePrefab;
    public Transform muzzle;
    public PlayerCoopEventBus playerInfoSC;

    [Header("Player transforms")]
    public Transform player_1_Transform;
    public Transform player_2_Transform;

    [Header("Bullet stats")]
    public float timeBetweenBulllets;
    public float cooldown;
    public float bulletSpeed;
    public float numberOfBullets;

    [Header("Serialized for debugging")]
    [SerializeField] bool shooting = false;
    [SerializeField] Transform targetTransform;

    void Start()
    {
        StartCoroutine(WeaponSystem());
        StartCoroutine(TargetSwitch());
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(targetTransform);
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
                Vector3 direction = (targetTransform.position - muzzle.position).normalized;
                projectile.GetComponent<Rigidbody>().velocity = direction * bulletSpeed;

                yield return new WaitForSeconds(timeBetweenBulllets);
            }
            shooting = false;
            yield return new WaitForSeconds(cooldown);
        }
    }

    IEnumerator TargetSwitch() { 
        while (true)
        {
            targetTransform = player_1_Transform;
            yield return new WaitForSeconds(2);
            targetTransform = player_2_Transform;
            yield return new WaitForSeconds(2);
        }
    }

}
