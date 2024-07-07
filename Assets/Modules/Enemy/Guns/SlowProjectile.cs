using System.Collections;
using System.Collections.Generic;
using Modules.CommonEventBus;
using UnityEngine;
using UnityEngine.InputSystem;

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


    private void OnEnable()
    {
        playerInfoSC.PlayerJoinedGameEvent += OnPlayerJoined;
        playerInfoSC.PlayerLeftGameEvent += OnPlayerLeft;

    }

    private void OnDisable()
    {
        playerInfoSC.PlayerJoinedGameEvent -= OnPlayerJoined;
        playerInfoSC.PlayerLeftGameEvent -= OnPlayerLeft;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(targetTransform);
    }


    void OnPlayerJoined(PlayerInput player)
    {
        if(player.playerIndex == 0)
        {
            player_1_Transform = player.transform;
        }
        else if (player.playerIndex == 1)
        {
            player_2_Transform = player.transform;
        }


        //Carefull with multiple process when player joins again after dead
        if(player_1_Transform != null && player_2_Transform != null)
        {
            Debug.Log("Both players joined");
            StartCoroutine(TargetSwitch());
            StartCoroutine(WeaponSystem());
        }
    }

    void OnPlayerLeft(PlayerInput player)
    {
        if (player.playerIndex == 0)
        {
            player_1_Transform = null;
        }
        else if (player.playerIndex == 1)
        {
            player_2_Transform = null;
        }

        //on left any one
    }


    IEnumerator WeaponSystem()
    {
        Debug.Log("Starting weapon system");
        while (true)
        {
            shooting = true;
            for(int i = 0; i < numberOfBullets; i++)
            {
                GameObject projectile = Instantiate(projectilePrefab, muzzle.position, Quaternion.identity);
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
