using System.Collections;
using Characters.Player.Global;
using Modules.Loadout.Scripts.Item;
using Modules.Loadout.Scripts.Manager;
using UnityEngine;

namespace Modules.Loadout.Scripts.Guns
{
    public class TriggerWeaponBase: ItemBase
    {
        [SerializeField] private BulletBase _bulletPrefab;
        [SerializeField] private float bulletSpeed = 5;
        [Header("Gun Properties")]
        [SerializeField] protected EnumAllItemType.WeaponTriggerType _weaponTriggerType;
        [SerializeField] protected EnumAllItemType.AmmoType ammoType;
        [SerializeField] private bool AddBulletSpread = true;
        [SerializeField] private Vector3 BulletSpreadVariance = new Vector3(0.1f, 0.1f, 0.1f);
        [SerializeField] private ParticleSystem ShootingSystem;
        [SerializeField] private Transform BulletSpawnPoint;
        [SerializeField] private ParticleSystem ImpactParticleSystem;
        [SerializeField] private TrailRenderer BulletTrail;
        [SerializeField] private float consecutiveShootDelay = 0.5f;
        [SerializeField] private float shootAnimationLength = 0.2f;
        [SerializeField] private LayerMask Mask;
        [SerializeField] private float BulletSpeed = 100;
        [SerializeField] private float bulletRange = 4;
        [SerializeField] private bool spawnTrailOnShootHit;
        
        [Header("Ammo properties")]
        [SerializeField] int currentMags = 5;
        [SerializeField] int ammoPerRound = 30;
        [SerializeField] int currentAmmoLeft=0;
        [SerializeField] int damageValue=5;
        
        protected float lastShootTime;

        public float reloadTimeInSeconds_OnAmmoAvailable;
        public float reloadTimeInSeconds__OnAmmoNotAvailable;

        public bool OnAim;

        [SerializeField] private LineRenderer laserSight;


        public EnumAllItemType.AmmoType GetAmmonType => ammoType;
        
        public override void SetItemOnEquipped()
        {
            // throw new System.NotImplementedException();
        }

        public override void OnItemUse()
        {
            // DebugX.LogWithColorCyan("Use Gun "+transform.name);
            ManualShoot();
        }

        public override void OnItemUseStop()
        {
            // throw new System.NotImplementedException();
        }

        public override int GetItemAmount()
        {
            return currentAmmoLeft;
        }


        public void ManualShoot()
        {
            if (currentAmmoLeft < 1)
            {
                //UnSuccessful Shooting
                DebugX.LogWithColorYellow("No ammo left");
            }
            else
            {
                //Successful Shooting Animation
                if (itemId == EnumAllItemType.ItemId.Pistol)
                {
                    //Pistol
                    // playerRefBase.playerAnimationManager.Interpolate_ArmRigUpper(0.5f, 0.1f);
                    // playerRefBase.playerAnimationManager.PlayPistolShoot();
                    // playerRefBase.playerAnimationManager.StopPistolShootAfterDelayCoroutineC(shootAnimationLength);
                }
                else
                {
                    //Add Rifle
                    
                }
                
                //AMMO
                currentAmmoLeft = currentAmmoLeft > 0 ? currentAmmoLeft - 1 : 0;
                
                //VISUAL
                if (lastShootTime + consecutiveShootDelay < Time.time)
                {
                    // Use an object pool instead for these! To keep this tutorial focused, we'll skip implementing one.
                    // For more details you can see: https://youtu.be/fsDE_mO4RZM or if using Unity 2021+: https://youtu.be/zyzqA_CPz2E

                    // Animator.SetBool("IsShooting", true);
                    ShootingSystem.Play();
                    Vector3 direction = GetDirection();

                    if (_bulletPrefab != null)
                    {
                        var bulletSpawned = Instantiate(_bulletPrefab);
                        bulletSpawned.SetBulletVelocity(bulletSpeed, BulletSpawnPoint);
                    }



                    if (Physics.Raycast(BulletSpawnPoint.position, direction, out RaycastHit hit, bulletRange, Mask))
                    {
                        if (spawnTrailOnShootHit)
                        {
                            TrailRenderer trail = Instantiate(BulletTrail, BulletSpawnPoint.position, Quaternion.identity);
                            StartCoroutine(SpawnTrail(trail, hit.point, hit.normal, true));
                        }

                        
                        Instantiate(ImpactParticleSystem, hit.point, Quaternion.LookRotation(hit.normal));
                        
                        lastShootTime = Time.time;

                        if (hit.transform.gameObject.CompareTag("Enemy"))
                        {
                            // hit.transform.GetComponent<Hitbox>().Stagger(transform.position, 20);
                        }
                        
                        // DebugX.LogWithColorCyan("SHoot got hit");
                    }
                    // this has been updated to fix a commonly reported problem that you cannot fire if you would not hit anything
                    else
                    {
                        if (spawnTrailOnShootHit)
                        {
                            TrailRenderer trail = Instantiate(BulletTrail, BulletSpawnPoint.position,
                                Quaternion.identity);

                            StartCoroutine(SpawnTrail(trail, BulletSpawnPoint.position + GetDirection() * bulletRange,
                                Vector3.zero,
                                false));
                        }

                        lastShootTime = Time.time;
                        
                        // DebugX.LogWithColorCyan("SHoot did not got hit");
                    }
                }
            }
        }

        private Vector3 GetDirection()
        {
            Vector3 direction = transform.forward;

            if (AddBulletSpread)
            {
                direction += new Vector3(
                    Random.Range(-BulletSpreadVariance.x, BulletSpreadVariance.x),
                    Random.Range(-BulletSpreadVariance.y, BulletSpreadVariance.y),
                    Random.Range(-BulletSpreadVariance.z, BulletSpreadVariance.z)
                );

                direction.Normalize();
            }

            // Debug.Log("Direction : "+direction);
            return new Vector3(direction.x, 0, direction.z);
        }

        private IEnumerator SpawnTrail(TrailRenderer Trail, Vector3 HitPoint, Vector3 HitNormal, bool MadeImpact)
        {
            // This has been updated from the video implementation to fix a commonly raised issue about the bullet trails
            // moving slowly when hitting something close, and not
            Vector3 startPosition = Trail.transform.position;
            float distance = Vector3.Distance(Trail.transform.position, HitPoint);
            float remainingDistance = distance;

            while (remainingDistance > 0)
            {
                Trail.transform.position = Vector3.Lerp(startPosition, HitPoint, 1 - (remainingDistance / distance));

                remainingDistance -= BulletSpeed * Time.deltaTime;

                yield return null;
            }

            // Animator.SetBool("IsShooting", false);
            Trail.transform.position = HitPoint;
            if (MadeImpact)
            {
                Instantiate(ImpactParticleSystem, HitPoint, Quaternion.LookRotation(HitNormal));
            }

            Destroy(Trail.gameObject, Trail.time);
        }
        
        

        public bool ReloadPossible()
        {
            if (currentMags > 0 && currentAmmoLeft != ammoPerRound)
            {
                Debug.Log("reload possible check true");
                return true;
            }
            else
            {
                Debug.Log("reload possible check false");
                return false;
            }
        }
        
        private bool OnReload()
        {
            if(ReloadPossible()){
                currentAmmoLeft = ammoPerRound;
                currentMags -= 1;
                Debug.Log("Reloading weapon "+currentMags);
                return true;
            }else{
                Debug.Log("Reloading Failed: no Ammo left or Ammo is already Full "+currentMags);
                return false;
            }
        }

        /*public void OnAimingON()
        {
            RaycastHit hit;
            if (Physics.Raycast(BulletSpawnPoint.transform.position, BulletSpawnPoint.transform.forward, out hit, bulletRange))
            {
                laserSight.SetPosition(1, hit.point);
            }
            else
            {
                laserSight.SetPosition(1, laserSight.transform.position + (BulletSpawnPoint.transform.forward * bulletRange));
            }
        }

        public void EnableAim_LineOfSight()
        {
            OnAim = true;
            laserSight.enabled = true;
        }

        public void DisableAim_LineOfSight()
        {
            OnAim = false;
            laserSight.enabled = false;
        }*/
    }
}